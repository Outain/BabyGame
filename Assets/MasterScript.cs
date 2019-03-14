using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MasterScript : MonoBehaviour
{
    public string testing;
    KeyCode[] controlerOneArray  = {KeyCode.Joystick1Button0, KeyCode.Joystick1Button1,KeyCode.Joystick1Button2,KeyCode.Joystick1Button3 };
    KeyCode[] controlerTwoArray = { KeyCode.Joystick1Button12, KeyCode.Joystick1Button13, KeyCode.Joystick1Button14, KeyCode.Joystick1Button15 };
    public GameObject[] shapeIndex;
    public Transform[] spawnPoints;
    public Color[] colors;
    int[] index = new int[2];
    GameObject[] created = new GameObject[2];
    int lastIndex = 5;
    int controlerOne = 5;
    int controlerTwo = 5; 
    public KeyCode testingButtions;
    private void Start()
    {
        NewShapes(); 
    }
    void Update(){
        bool completedCheck = false;
        for( int i =0; i < shapeIndex.Length; i ++){

            // if the key is active
            if(Input.GetKey(controlerOneArray[i]))
            {

                if (i == index[0]){
                    completedCheck = true; 

                } 

                
            }
        }



        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        //Debug.Log(horz + " " + vert);
        int finalIndex = 5; 
        
        if( horz > 0){
            finalIndex = 0;
        }
        else if(horz < 0){
            finalIndex = 1;
        }
        else if(vert > 0){
            finalIndex = 2;
        }
        else if(vert <0){
            finalIndex = 3;
        }
        else{
            finalIndex = 5;
        }

        Debug.Log(finalIndex);
        if (completedCheck && finalIndex == index[1]){
            ClearShapes();

        }
        
            
    }

    void NewShapes(){

        for(int i =0; i < index.Length; i ++){
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
    void ClearShapes(){

        for( int i =0; i <created.Length; i ++){

            Destroy(created[i]);

        }

        //NewShapes(); 
    }

    void ControlerOneInputCheck(){

  





    }
}
