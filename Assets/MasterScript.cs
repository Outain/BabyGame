﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MasterScript : MonoBehaviour
{
    public string testing;
    public int[] pinkCheck = {0,1,2,3}; 
    public int health = 3;
    private float timePerAction;
    public float maxTimePerAction;
    public float minTimePerAction;
    public float timeIncrement;
    public int score;
    //public Slider slidey;
    // controler side one active buttions
    public KeyCode[] controlerOneArray  = {KeyCode.Joystick1Button0, KeyCode.Joystick1Button1,KeyCode.Joystick1Button2,KeyCode.Joystick1Button3 };
    // controler two acctive buttions 
    public KeyCode[] controlerTwoArray = { KeyCode.Joystick1Button12, KeyCode.Joystick1Button13, KeyCode.Joystick1Button14, KeyCode.Joystick1Button15 };
    // index of shapes 
    public GameObject[] shapeIndex;
    // spsawn point for shapes 
    public Transform[] spawnPoints;
    // colors to use for pink and blue
    public Color[] colors;
    //intput indexes 
    int[] index = new int[2]; // current buttons to press    
    GameObject[] created = new GameObject[2];
    // last index for a repetative shape check
    int lastIndex = 5;
    //current controler index 
    int controlerOne = 5;
    // current colorler index 2
    int controlerTwo = 5; 
    // for testing purposes 
    public KeyCode testingButtions;
    public Image hourGlassBottom, hourGlassTop;
    public float hourGlassFillAmount;
    private bool initialised=false; //using this to make sure time is only incremented after first shapes and not before.
    void Awake()
    {
        timePerAction = maxTimePerAction;
    }
    private void Start()
    {
        StartCoroutine(Initialiser());

    }
    private void Update()
    {
        hourGlassFillAmount = (maxTimePerAction - timePerAction) / maxTimePerAction;
        //print(hourGlassFillAmount);
        hourGlassBottom.fillAmount = hourGlassFillAmount;
        hourGlassTop.fillAmount = (1 - hourGlassFillAmount);
        bool completedCheck = false;
        timePerAction -= Time.deltaTime;
        //slidey.maxValue = maxTimePerAction;
       // slidey.value = timePerAction;
        if (Input.GetKey(KeyCode.K))
        {
            
            Debug.Log("Inputworking");
        }
        
        for( int i =0; i < shapeIndex.Length; i ++){

            // if the key is active
            // check if block in in place (blocks are in order of array )
            if(Input.GetKey(controlerOneArray[i]))
            {
             Debug.Log(i);
             //Debug.Log("Testing working");
                if (i == index[0]){
                    //Debug.Log("Yep This One");
                    // this code is working with a basic controler 
                    completedCheck = true; 
                    
                    

                } 

                
            }
        }
        
        


        // check the axis input, each axis is -1 to 1 and coraspond to a index number 
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        //Debug.Log(horz + " " + vert);
        //public int[] pinkcheck;
        int finalIndex = 5; 
        //right// square
        if( horz > 0){
            finalIndex = pinkCheck[0];
        }
        //left/ cycle
        else if(horz < 0){
            finalIndex = pinkCheck[1];
        }
        //u[// triangle
        else if(vert > 0){
            finalIndex = pinkCheck[2];
        }
        //down star
        else if(vert < 0){
            finalIndex = pinkCheck[3];
        }
        else{
            finalIndex = 5;
        }

        //Debug.Log(horz);
        Debug.Log(finalIndex + " , " +   index[1]);
        // checks the active indexes againt the one need 
        if (completedCheck && finalIndex == index[1]){
            score += 100;
            //Debug.Log("GG");
            // this code is also working with a basic controler 
            ClearShapes(); // shapes rest score and health ++ ? 

        }

        if (timePerAction <= 0)
        {
            health--;
            ClearShapes();
        }

      
            
    }
    // calls new shaped to be used for the next index check 
    void NewShapes(){
        Audio.AudioMaster.PlayAudioClipBackGround(0);
        for (int i =0; i < index.Length; i ++){
            index[0] = Random.Range(0, shapeIndex.Length);
        }
        // if index is the same retry untill its not :) 
        while(index[0] == index[1] || index[1] == lastIndex){
            index[1] = Random.Range(0, shapeIndex.Length);
        }
        for(int i =0; i < index.Length; i ++){
            created[i] = Instantiate(shapeIndex[index[i]], spawnPoints[i].transform.position, Quaternion.identity);
            created[i].transform.SetParent(spawnPoints[i].transform);
            created[i].GetComponent<Image>().color = colors[i]; 
        }



        lastIndex = index[1];
    } 

    // clears old shapes and starts the new spawning section 
    void ClearShapes(){

        for( int i =0; i <created.Length; i ++){

            Destroy(created[i]);

        }
        timePerAction = maxTimePerAction;
        if (maxTimePerAction > minTimePerAction)
        {
            if (initialised)
            {
                maxTimePerAction -= timeIncrement;
            }

            float sendToAudio = 1;
            sendToAudio = (float) Mathf.Abs((1 / maxTimePerAction) - (maxTimePerAction * 0.2f));
            Audio.AudioMaster.timeBetweenShiftChanges = sendToAudio;
            Audio.AudioMaster.audioPitchChange = 1 / maxTimePerAction; 
        }

        NewShapes(); 
    }
    
    private void OnGUI()
    {
        //GUI.Label(new Rect(10, 10, 1000, 20), actionToDisplay + " " + buttonPressString);
        GUI.Label(new Rect(10, 30, 1000, 20), "Time: "+ maxTimePerAction.ToString("F2"));
        GUI.Label(new Rect(10, 50, 1000, 20), "Health: "+ health);
        GUI.Label(new Rect(10, 70, 1000, 20), "Score: "+ score);
        //GUI.Label(new Rect(10, 90, 1000, 20), buttonString1 + " " + buttonString2);
    }

    float timeBeforeStart = 2f; 
    IEnumerator Initialiser()
    {
        hourGlassTop.gameObject.SetActive(false);
        hourGlassBottom.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeBeforeStart);
        hourGlassTop.gameObject.SetActive(true);
        hourGlassBottom.gameObject.SetActive(true);
        ClearShapes();
        initialised = true;
        //NewShapes();
    }   
}
