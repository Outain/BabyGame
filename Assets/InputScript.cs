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
    private string actionToDisplay, buttonPressString, buttonString1, buttonString2;
    private int buttonNumber;
    public int health = 100;
    private float timePerAction;
    public float maxTimePerAction;
    public float minTimePerAction;
    public float timeIncrement;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
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
            score += 100;
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

        
        // this code checks if both buttons are using the same piece and reassigns them if so
        if (Button1 == blueSquare && Button2 == redSquare)
        {
            Button2 = Keys[2];
        }
        if (Button1 == blueCircle && Button2 == redCircle)
        {
            Button2 = Keys[2];
        }
        
        if (Button1 == blueTriangle && Button2 == redTriangle)
        {
            Button2 = Keys[2];
        }
        
        if (Button1 == blueStar && Button2 == redStar)
        {
            Button2 = Keys[2];
        }
        ChangeText();

    }
    
    void reshuffle(KeyCode[] Keys)
    {
        
        for (int t = 0; t < Keys.Length; t++ )
        {
            KeyCode tmp = Keys[t];
            int r = Random.Range(t, Keys.Length);
            Keys[t] = Keys[r];
            Keys[r] = tmp;
        }
    }

    void ChangeText()
    {
        if (Button1 == blueSquare)
        {
            buttonString1 = "Blue Square";
        }
        
        if (Button1 == blueCircle)
        {
            buttonString1 = "Blue Circle";
        }
        
        if (Button1 == blueTriangle)
        {
            buttonString1 = "Blue Triangle";
        }
        
        if (Button1 == blueStar)
        {
            buttonString1 = "Blue Star";
        }
        
        if (Button1 == redSquare)
        {
            buttonString1 = "Red Square";
        }
        
        if (Button1 == redCircle)
        {
            buttonString1 = "Red Circle";
        }
        
        if (Button1 == redTriangle)
        {
            buttonString1 = "Red Triangle";
        }
        
        if (Button1 == redStar)
        {
            buttonString1 = "Red Star";
        }
        
        
        if (Button2 == blueSquare)
        {
            buttonString2 = "Blue Square";
        }
        
        if (Button2 == blueCircle)
        {
            buttonString2 = "Blue Circle";
        }
        
        if (Button2 == blueTriangle)
        {
            buttonString2 = "Blue Triangle";
        }
        
        if (Button2 == blueStar)
        {
            buttonString2 = "Blue Star";
        }
        
        if (Button2 == redSquare)
        {
            buttonString2 = "Red Square";
        }
        
        if (Button2 == redCircle)
        {
            buttonString2 = "Red Circle";
        }
        
        if (Button2 == redTriangle)
        {
            buttonString2 = "Red Triangle";
        }
        
        if (Button2 == redStar)
        {
            buttonString2 = "Red Star";
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 20), actionToDisplay + " " + buttonPressString);
        GUI.Label(new Rect(10, 30, 1000, 20), ""+ timePerAction);
        GUI.Label(new Rect(10, 50, 1000, 20), "Health: "+ health);
        GUI.Label(new Rect(10, 70, 1000, 20), "Score: "+ score);
        GUI.Label(new Rect(10, 90, 1000, 20), buttonString1 + " " + buttonString2);
    }
}
