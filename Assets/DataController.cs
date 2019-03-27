using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files

public class DataController : MonoBehaviour 
{
    
    private PlayerProgress playerProgress;

    private string gameDataFileName = "data.json";

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        
        LoadPlayerProgress();
    }
  

    public void SubmitNewPlayerScore(int newScore)
    {
        print(playerProgress.highestScore);
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if(newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            print("Saved!");
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

   

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    public void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if(PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }
}
