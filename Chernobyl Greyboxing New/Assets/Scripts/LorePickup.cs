using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorePickup : MonoBehaviour
{
    public LoreUIPopup lorePopup;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lorePopup.ShowLore(gameObject);
        }
    }
}
