using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpoopySound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] spookyClips;      // Array of spooky sounds
    public AudioSource audioSource;      // AudioSource to play the sounds

    [Header("Timing Settings")]
    public float minInterval = 15f;      // Minimum time between sounds
    public float maxInterval = 30f;      // Maximum time between sounds

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(PlaySpookySounds());
    }

    IEnumerator PlaySpookySounds()
    {
        while (true)
        {
            // Wait a random interval
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Play a random sound
            if (spookyClips.Length > 0)
            {
                int index = Random.Range(0, spookyClips.Length);

                // Random pitch/volume for creepiness
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = Random.Range(0.7f, 1f);

                audioSource.PlayOneShot(spookyClips[index]);
            }
        }
    }
}