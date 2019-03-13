using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InputScript : MonoBehaviour
{
    public KeyCode blueSquare, blueCircle, blueTriangle, blueStar,redSquare, redCircle, redTriangle, redStar;
    public KeyCode Button1, Button2;
    public KeyCode[] Keys;
    private int actionState;
    public string actionToDisplay, buttonPressString;
    public int health = 100;
    private float timePerAction;
    public float maxTimePerAction;
    public float minTimePerAction;
    public float timeIncrement;

    // Start is called before the first frame update
    void Start()
    {
        Keys = new KeyCode[] {blueSquare, blueCircle, blueTriangle, blueStar,redSquare, redCircle, redTriangle, redStar};
        NewAction();
        timePerAction = maxTimePerAction;

    }

    // Update is called once per frame
    void Update()
    {
        TextToDisplay();
        timePerAction -= Time.deltaTime;

        if (Input.GetKey(Button1) && Input.GetKey(Button2))
        {
            NewAction();
        }

        if (timePerAction <= 0)
        {
            health -= 10;
           
           NewAction();
        }
    }

    void TextToDisplay()
    {
        if (actionState == 0)
        {
            actionToDisplay = "Feed the baby!";
        }
        if (actionState == 1)
        {
            actionToDisplay = "Play with the baby!";
        }
        
        if (actionState == 2)
        {
            actionToDisplay = "Calm the baby!!!";
        }
        
        if (actionState == 3)
        {
            actionToDisplay = "Change the baby!";
        }
    }

    void NewAction()
    {
        timePerAction = maxTimePerAction;
        reshuffle(Keys);
        Button1 = Keys[0];
        Button2 = Keys[1];
        print(Keys[0]);
        print(Keys[1]);
        buttonPressString = "Press " + Button1 + " and " + Button2;
        print(buttonPressString);
        actionState = Random.Range(0, 4);
        if (maxTimePerAction > minTimePerAction)
        {
            maxTimePerAction -= timeIncrement;
        }

    }
    
    void reshuffle(KeyCode[] Keys)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < Keys.Length; t++ )
        {
            KeyCode tmp = Keys[t];
            int r = Random.Range(t, Keys.Length);
            Keys[t] = Keys[r];
            Keys[r] = tmp;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 20), actionToDisplay + " " + buttonPressString);
        GUI.Label(new Rect(10, 30, 1000, 20), ""+ timePerAction);
        GUI.Label(new Rect(10, 50, 1000, 20), "Health: "+ health);
    }
}
