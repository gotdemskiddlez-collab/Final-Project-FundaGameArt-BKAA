using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoreEntry
{
    public GameObject loreItemPrefab;  // The pickup item prefab
    public GameObject loreUI;          // The corresponding UI to show
    public AudioClip openSound;
    public AudioClip closeSound;
}

public class LoreUIPopup : MonoBehaviour
{
    public AudioSource audioSource;        // Optional AudioSource for sounds
    public LoreEntry[] loreEntries;        // Array of all lore items

    private GameObject currentUI;          // Currently active lore UI
    private GameObject currentItem;        // The item that triggered the popup
    private AudioClip currentCloseSound;
    private bool isLoreOpen = false;

    // Call this when player picks up a lore item
    public void ShowLore(GameObject pickedUpItem)
    {
        if (isLoreOpen) return;

        // Find the corresponding lore entry
        foreach (var entry in loreEntries)
        {
            if (entry.loreItemPrefab == pickedUpItem)
            {
                currentUI = entry.loreUI;
                currentItem = pickedUpItem;
                currentCloseSound = entry.closeSound;

                if (currentUI != null)
                    currentUI.SetActive(true);

                if (audioSource != null && entry.openSound != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(entry.openSound);
                }

                isLoreOpen = true;
                return;
            }
        }

        Debug.LogWarning("LorePopup: No matching lore entry found for " + pickedUpItem.name);
    }

    void Update()
    {
        if (isLoreOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseLore();
        }
    }

    public void CloseLore()
    {
        if (currentUI != null)
            currentUI.SetActive(false);

        if (audioSource != null && currentCloseSound != null)
            audioSource.PlayOneShot(currentCloseSound);

        if (currentItem != null)
            Destroy(currentItem);

        currentUI = null;
        currentItem = null;
        currentCloseSound = null;
        isLoreOpen = false;
    }
}