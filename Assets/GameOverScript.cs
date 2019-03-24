using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public MasterScript ms;

    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ms.health <= 0)
        {
            ms.enabled = false;
            Audio.AudioMaster.gameObject.SetActive(false);
            gameOverText.enabled = true;
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
