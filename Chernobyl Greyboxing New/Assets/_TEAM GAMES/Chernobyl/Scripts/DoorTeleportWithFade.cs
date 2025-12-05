using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class DoorTeleportWithFade : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // Reference to the player
    public Transform teleportTarget;    // The target location to teleport the player to
    public Image fadeImage;             // UI Image used for fade effect
    public float fadeDuration = 1f;     // Duration of the fade effect
    public float interactionRange = 2f; // Range within which the player can interact with the door

    public bool isFading = false;


    // Start is called before the first frame update
    void Start()
    {
        
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || teleportTarget == null || fadeImage == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E) && !isFading)
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    public IEnumerator FadeAndTeleport()
    {
        isFading = true;

        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Teleport the player
        player.position = teleportTarget.position;
        player.rotation = teleportTarget.rotation;

        // Small pause before fading back in
        yield return new WaitForSeconds(0.5f);

        // Fade back in
        yield return StartCoroutine(Fade(0f));

        isFading = false;
    }

    public IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        // Ensure the final alpha is set
        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
}
