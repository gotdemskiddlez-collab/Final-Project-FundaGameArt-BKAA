using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool isCollected = false;
    public CollectibleChainManager chainManager;
    public GameObject highlightLight;

    public void Collect()
    //private void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Player")) return;

        Debug.Log($"[Collectible] Collected: {name}", this);
        isCollected = true;

        if (chainManager != null)
            chainManager.NotifyCollected(this);
        else
            Debug.LogError($"[Collectible] No chainManager assigned on {name}", this);

        Destroy(gameObject);
    }

    public void ToggleHighlight(bool turnOn)
    {
        highlightLight.SetActive(turnOn);
    }
}