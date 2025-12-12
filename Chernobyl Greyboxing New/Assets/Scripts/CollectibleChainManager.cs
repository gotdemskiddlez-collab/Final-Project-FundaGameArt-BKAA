using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleChainManager : MonoBehaviour
{
    [Header("Ordered list of collectible prefabs (prefab assets)")]
    public List<GameObject> collectibleChain = new List<GameObject>();

    [Header("Spawn points (must have same count as chain)")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Behavior")]
    public bool loop = false;
    public Transform spawnedParent;

    private int currentIndex = 0;
    private GameObject currentInstance;

    void Start()
    {
        // Validate simple, strict requirements
        if (collectibleChain == null || collectibleChain.Count == 0)
        {
            Debug.LogError("[ChainManager] collectibleChain is empty. Fill it with prefab assets.", this);
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Count < collectibleChain.Count)
        {
            Debug.LogError("[ChainManager] spawnPoints count is less than collectibleChain count. Provide one spawn point per chain index.", this);
            enabled = false;
            return;
        }

        SpawnAt(currentIndex);
    }

    void SpawnAt(int index)
    {
        if (index < 0 || index >= collectibleChain.Count)
        {
            Debug.LogError($"[ChainManager] SpawnAt called with invalid index {index}.", this);
            return;
        }

        var prefab = collectibleChain[index];
        if (prefab == null)
        {
            Debug.LogError($"[ChainManager] Prefab at index {index} is null.", this);
            return;
        }

        var spawnPoint = spawnPoints[index];
        if (spawnPoint == null)
        {
            Debug.LogError($"[ChainManager] Spawn point at index {index} is null.", this);
            return;
        }

        Debug.Log($"[ChainManager] Spawning '{prefab.name}' at spawn point {index} ({spawnPoint.position})", this);
        currentInstance = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        if (spawnedParent != null) currentInstance.transform.SetParent(spawnedParent, true);

        // Ensure prefab already has Collectible script on it in the project (no runtime add)
        var collectibleComp = currentInstance.GetComponent<Collectible>();
        if (collectibleComp == null)
        {
            Debug.LogError($"[ChainManager] Spawned prefab '{prefab.name}' does NOT have a Collectible component. Add it to the prefab.", this);
            // We don't add it automatically in this strict version.
        }
        else
        {
            collectibleComp.chainManager = this;
        }
    }

    public void NotifyCollected(Collectible collected)
    {
        if (collected == null)
        {
            Debug.LogError("[ChainManager] NotifyCollected received null.", this);
            return;
        }

        if (collected.gameObject != currentInstance)
        {
            Debug.LogWarning($"[ChainManager] Collected object ({collected.name}) is not the current instance ({(currentInstance ? currentInstance.name : "null")}). Ignoring.", this);
            return;
        }

        Debug.Log($"[ChainManager] Received collection of index {currentIndex} ({collected.name}).", this);

        // advance index
        currentIndex++;

        if (currentIndex >= collectibleChain.Count)
        {
            if (loop)
            {
                currentIndex = 0;
            }
            else
            {
                Debug.Log("[ChainManager] Chain finished. No more spawns.", this);
                currentInstance = null;
                enabled = false;
                return;
            }
        }

        currentInstance = null;
        SpawnAt(currentIndex);
    }
}