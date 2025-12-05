using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiationBarLogic : MonoBehaviour
{
    public Slider radiationBar;
    public float fillDuration = 300f; //300 seconds = 5 minute duration
    public float elapsedTime = 0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (radiationBar != null)
        {
            radiationBar.minValue = 0f; // 0 seconds
            radiationBar.maxValue = 300f; // 300 seconds (5 minutes)
            radiationBar.value = 0f; // begins at 0 seconds
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (radiationBar == null) return;

        if (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime; // fills with each passing second
            radiationBar.value = Mathf.Lerp(radiationBar.minValue, radiationBar.maxValue, elapsedTime / fillDuration);
        }

        if (radiationBar.value == 300f) // if the radiation bar reaches 300 seconds...
        {
            Death(); // ... the character dies
        }
    }

    public void Death()
    {
        // Something will happen here when character dies
    }
}
