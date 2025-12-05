using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Your Game Manager should keep track of the state of the elements in your game
// This includes things like score and win/lose conditions
public class GameManager : MonoBehaviour
{
    // UI Elements
    public Image blackScreenImage;
    public GameObject winScreen;
    public TMP_Text countText;
    
    // Counts
    private int collectibleCount;
    public int winCount;

    // Game Elements
    public List<Collectible> collectibles;

    // Start is called before the first frame update
    void Start()
    {
        // Reset the UI for the start of the game
        blackScreenImage.color = Color.clear;
        countText.text = "Collected: 0";
    }

    // Update is called once per frame
    void Update()
    {
        TrackCollectibles();
    }

    // Check our list of Collectibles to see if any of them have been collected by the player
    private void TrackCollectibles()
    {
        foreach (Collectible collectible in collectibles)
        {
            if (collectible.isCollected)
            {
                AddToCollectibleCount();
            }

            // If the collectible is destroyed, remove it from the list and start the loop over
            // Without this check, we will keep adding the same collectible to the count every time TrackCollectibles() is called
            if (collectible == null)
            {
                collectibles.Remove(collectible);
                break;
            }
        }
    }

    private void AddToCollectibleCount()
    {
        Debug.Log("Add to the Collectible Count!");

        collectibleCount++; // ++ adds one to the current value of an int variable
        countText.text = "Collected: " + collectibleCount.ToString();
        Debug.Log($"New Collectible Count is {collectibleCount}");

        // We've collected a new collectible! Do we have enough collectibles to win the game?
        if (collectibleCount == winCount)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game over!");
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // These three functions can be added to buttons in editor to run them

    public void ExitGame()
    {
        Debug.Log("Exit Game to Main Menu");
        StartCoroutine(SceneLoadTimer(0));
    }
    public void StartGame()
    {
        Debug.Log("Start Game");
        StartCoroutine(SceneLoadTimer(1));
    }

    public void CloseApplication()
    {
        Debug.Log("Close Application");
        Application.Quit();
    }

    // This is a Coroutine! It allows us to execute code over a period of time rather than all at once
    // This one fades the alpha of the blackScreenImage rather than having it pop in
    IEnumerator SceneLoadTimer(int scene)
    {
        float timer = 0f; // Create a float to increment up over time
        float duration = 1f; // How long should the fade last?

        while(timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float lerp = timer / duration;

            blackScreenImage.color = Color.Lerp(Color.clear, Color.black, lerp);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.8f);

        SceneManager.LoadScene(scene);
    }
}