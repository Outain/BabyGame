using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public MasterScript ms;

    public Text gameOverText;

    public DataController dataController;

    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        dataController = GetComponent<DataController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ms.health <= 0&&!gameOver)
        {
            print("Game Over");
            //dataController.SubmitNewPlayerScore(ms.score);
            ms.highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highestScore");
            ms.enabled = false;
            
            Audio.AudioMaster.gameObject.SetActive(false);
            gameOverText.enabled = true;
            gameOver = true;
        }
        
      
        if (Input.GetKey(KeyCode.R))
        {
            ResetScene();
        }
    }
    
    private void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
