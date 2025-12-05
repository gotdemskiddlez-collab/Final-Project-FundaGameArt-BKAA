using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public int maxHealth; //public so that in the inspector we can choose what we want maximum health to be
    private int currentHealth; //private because we are only handling this value inside this script, so we don't want it to be accessible in the inspector

    public GameObject gameOverScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        currentHealth = maxHealth;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal(int value) //healing requires a value to add to the player's health, which we get when the AlterPlayerHealth script tells this script to heal. 
    {
        currentHealth += value; //add the healing value to existing player health value
        if(currentHealth >= healthBar.maxValue)
        {
            currentHealth = maxHealth; //if our health is at 70, and we heal for a value of 50, the new health value will be 120. but if our max health value is 100, we want to make sure if the health value exceeds the max health, we set it to exactly the max health.  
        }
        healthBar.value = currentHealth; //sets the health bar ui element to the updated health value
    }

    public void TakeDamage(int value) //works the same as the heal method
    {
        currentHealth -= value;
        healthBar.value = currentHealth;
        if (currentHealth <= 0) //trigger the Die event if health value gets to 0 or below (if we are at 20 health and take damage of 30, we would be at -10 health, so we are accounting for the possibility that our health is below zero)
        {
            Die();
        }
    }

    public void Die()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
}
