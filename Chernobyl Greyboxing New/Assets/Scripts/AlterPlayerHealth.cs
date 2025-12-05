using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterPlayerHealth : MonoBehaviour
{
    public int changeValue; //by how much is this object healing/damaging the player
    public bool consumable; //will this object get consumed/destroyed upon use, or will it remain in the scene even once it is used?
    public Type type; //creates a dropdown in the inspector so we can choose the object type in unity
    public enum Type //is this object going to heal or damage the player? 
    {
        Heal,
        Damage
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(type == Type.Heal) //if this object heals and touches an object with the Player tag, execute what is inside these brackets
            {
                other.GetComponent<PlayerHealth>().Heal(changeValue); //since the PlayerHealth script is on the Player object, we can go inside the Player object and find specific scripts. Tells the PlayerHealth script to heal by the changeValue in the inspector
                if (consumable)
                {
                    Destroy(this.gameObject); //if we set the object to be consumable, the object will be destroyed upon use
                }
            }
            else
            {
                other.GetComponent<PlayerHealth>().TakeDamage(changeValue); //if this object is a Damage type instead of a Heal type, tell the PlayerHealth script to damage the player's health by the changeValue
            }
        }
    }

}
