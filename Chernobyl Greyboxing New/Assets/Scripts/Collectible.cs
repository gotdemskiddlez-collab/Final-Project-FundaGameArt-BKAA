using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        // What should happen if something with the Player tag enters the trigger box?
        if (other.gameObject.tag == "Player")
        {
            Debug.Log($"Collect: {this.name}");
            isCollected = true;

            Destroy(this.gameObject);
        }
    }
}