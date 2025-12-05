using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GeigerCounterSystem_Procedural : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Radiation Detection")]
    public float maxDetectDistance = 20f;
    public LayerMask radiationMask;

    [Header("Needle Settings")]
    public Transform ticker;
    public float minAngle = -90f;
    public float maxAngle = 90f;
    public float tickerSmooth = 5f;
    public float tickerShakeAmount = 3f;
    public float tickerShakeSpeed = 25f;

    [Header("Warning Light")]
    public Light warningLight;
    public Color warningColor = Color.red;
    public float maxLightIntensity = 5f;
    public float minPulseSpeed = 0.5f;
    public float maxPulseSpeed = 8f;

    [Header("Warning LED Emission")]
    public Renderer lEDRenderer;
    public float maxEmission = 5f;

    [Header("Procedural Audio Clicks")]
    public float rangeForClicks = 1f;
    public float clickVolume = 0.7f;
    public float clickLength = 0.002f;   // 2 milliseconds click
    public float minTickRate = 0.3f;     // slower clicks
    public float maxTickRate = 25f;      // faster, frantic clicks

    public float radiationLevel;

    // Internal information; doesn't need to be changed via the Inspector
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Material lEDMaterial;
    [SerializeField] private float nextTickTime;
    [SerializeField] private bool makeClick = false;
    [SerializeField] private int clickSamplesRemaining = 0;

    // Safe RNG for audio thread
    private System.Random rng = new System.Random();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.2f;
        audioSource.Play();

        if (lEDRenderer != null)
            lEDMaterial = lEDRenderer.material;

        nextTickTime = Time.time;
    }

    void Update()
    {
        radiationLevel = DetectRadiation();

        UpdateTicker();
        UpdateWarningLight();
        UpdateWarningEmission();
        UpdateClicks();
    }

    // ----------------------------------------------------------
    // 1. Radiation Detection
    // ----------------------------------------------------------
    float DetectRadiation()
    {
        Collider[] hits = Physics.OverlapSphere(
            player.position,
            maxDetectDistance,
            radiationMask
        );

        float highest = 0f;

        foreach (var hit in hits)
        {
            RadiationSource source = hit.GetComponent<RadiationSource>();
            if (source != null)
            {
                float distance = Vector3.Distance(player.position, hit.transform.position);
                float level = source.radiationStrength / distance;
                highest = Mathf.Max(highest, level);
            }
        }

        return Mathf.Clamp01(highest);
    }

    // ----------------------------------------------------------
    // 2. Ticker (with jittering needle)
    // ----------------------------------------------------------
    void UpdateTicker()
    {
        if (ticker == null) return;

        float angle = Mathf.Lerp(minAngle, maxAngle, radiationLevel);

        float shake = Mathf.Sin(Time.time * tickerShakeSpeed) * (tickerShakeAmount * radiationLevel);

        Quaternion target = Quaternion.Euler(0, 0, angle + shake);

        ticker.localRotation = Quaternion.Lerp(
            ticker.localRotation,
            target,
            Time.deltaTime * tickerSmooth
        );
    }

    // ----------------------------------------------------------
    // 3. Warning light Pulse
    // ----------------------------------------------------------
    void UpdateWarningLight()
    {
        if (warningLight == null) return;

        float pulseSpeed = Mathf.Lerp(minPulseSpeed, maxPulseSpeed, radiationLevel);

        float pulsedIntensity =
            Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) *
            (maxLightIntensity * radiationLevel);

        warningLight.color = warningColor;
        warningLight.intensity = pulsedIntensity;
    }

    // ----------------------------------------------------------
    // 4. LED Emissive Glow (from bulb material)
    // ----------------------------------------------------------
    void UpdateWarningEmission()
    {
        if (lEDMaterial == null) return;

        float speed = Mathf.Lerp(minPulseSpeed, maxPulseSpeed, radiationLevel);
        float emissionStrength =
            Mathf.Abs(Mathf.Sin(Time.time * speed)) *
            (maxEmission * radiationLevel);

        lEDMaterial.SetColor("_EmissionColor", warningColor * emissionStrength);
    }

    // ----------------------------------------------------------
    // 5. Click Sound
    // ----------------------------------------------------------
    void UpdateClicks()
    {
        if (radiationLevel <= rangeForClicks)
        {
            audioSource.enabled = false;
            return;
        }

        if (!audioSource.isActiveAndEnabled)
        {
            audioSource.enabled = true;
        }

        float tickRate = Mathf.Lerp(minTickRate, maxTickRate, radiationLevel);
        float interval = 1f / tickRate;

        if (Time.time >= nextTickTime)
        {
            TriggerClick();
            nextTickTime = Time.time + interval;
        }
    }

    // Called when we want a click sound
    void TriggerClick()
    {
        Debug.Log("Trigger Click");
        makeClick = true;
        clickSamplesRemaining = Mathf.RoundToInt(clickLength * AudioSettings.outputSampleRate);
    }

    // Audio Method called to fill the audio buffer
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!makeClick)
            return;

        makeClick = false;

        for (int i = 0; i < data.Length && clickSamplesRemaining > 0; i += channels)
        {
            float rand = (float)rng.NextDouble(); // 0–1
            float impulse = (rand * 0.4f + 0.6f) * clickVolume; // 0.6–1.0 range

            for (int c = 0; c < channels; c++)
                data[i + c] += impulse;

            clickSamplesRemaining--;
        }
    }
}
