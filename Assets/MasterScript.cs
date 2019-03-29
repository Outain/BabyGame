using System.Collections;
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
    public float shapeChangeDelay;
    public int score,highScore;
    //public Slider slidey;
    // controler side one active buttions
    public KeyCode[] controlerOneArray  = {KeyCode.Joystick1Button0, KeyCode.Joystick1Button1,KeyCode.Joystick1Button2,KeyCode.Joystick1Button3 };
    // controler two acctive buttions 
    // point less keep just in case :) public KeyCode[] controlerTwoArray = { KeyCode.Joystick1Button12, KeyCode.Joystick1Button13, KeyCode.Joystick1Button14, KeyCode.Joystick1Button15 };
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
    public Text livesText, scoreText,highScoreText, timeText;
    private bool initialised=false; //using this to make sure time is only incremented after first shapes and not before.
    private bool waitingForShapes;

    private DataController dataController;
    private PlayerProgress playerProgress;

    public GameObject rattle, bottle, nappy,sleep;
    public GameObject thoughtBubble;

    public GameObject happyHead, sadHead;

    enum BabyWants
    {
        bottle,
        rattle,
        nappy,
        sleep
    };

    private BabyWants currentWant;
    void Awake()
    {
        timePerAction = maxTimePerAction;
    }
    private void Start()
    {
        dataController = GetComponent<DataController>();
      
        
        StartCoroutine(Initialiser());
        
        highScore = PlayerPrefs.GetInt("highestScore");
        ClearObjects();
        
    }
    private void Update()
    {
        hourGlassFillAmount = (maxTimePerAction - timePerAction) / maxTimePerAction;
        //print(hourGlassFillAmount);
        hourGlassBottom.fillAmount = hourGlassFillAmount;
        hourGlassTop.fillAmount = (1 - hourGlassFillAmount);
        bool completedCheck = false;
        if (!waitingForShapes)
        {
            timePerAction -= Time.deltaTime;
        }
        //slidey.maxValue = maxTimePerAction;
       // slidey.value = timePerAction;

        
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
        if (completedCheck && finalIndex == index[1] && !waitingForShapes)
        {
            waitingForShapes = true;
            score += 100;
            CompletedSoundEffect();
            dataController.SubmitNewPlayerScore(score);
            HappyFace();
            //Debug.Log("GG");
            // this code is also working with a basic controler 
            StartCoroutine(ShapeDelay(true)); // shapes rest score and health ++ ? 

        }

        if (timePerAction <= 0 && !waitingForShapes)
        {
            waitingForShapes = true;
            health--;
            dataController.SubmitNewPlayerScore(score);
            StartCoroutine(ShapeDelay(false));
        }

      
        UpdateUI();
    }
    // calls new shaped to be used for the next index check
    bool audioType = false; 
    void NewShapes(){
        timePerAction = maxTimePerAction;
        ChooseObject();
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
        
        SadFace();
    } 

    // clears old shapes and starts the new spawning section 
    void ClearShapes(bool x)
    {
        // the bool is used so it ether right or wrong :) 

        if (x)
        {
            // corret ans
            Audio.AudioMaster.GetComponent<Audio>().PlayAudioClipFX(1); // this is how to play audio mark :) 
        }
        else {
            Audio.AudioMaster.GetComponent<Audio>().PlayAudioClipFX(2);
        }

        
        for( int i =0; i <created.Length; i ++){

            Destroy(created[i]);

        }
        
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
        else if (maxTimePerAction >= 3.5f)
        {
            maxTimePerAction -= 0.05f;
        }
        ClearObjects();
        //NewShapes(); 
    }

    void ClearObjects()
    {
        thoughtBubble.SetActive(false);
        bottle.SetActive(false);
        rattle.SetActive(false);
        nappy.SetActive(false);
        sleep.SetActive(false);
    }
    void ChooseObject()
    {
        int i;
        i = Random.Range(0, 4);
        if (i == 0)
        {
            currentWant = BabyWants.bottle;
        }
        else if (i == 1)
        {
            currentWant = BabyWants.rattle;
        }
        else if (i == 2)
        {
            currentWant = BabyWants.nappy;
        }
        else if (i == 3)
        {
            currentWant = BabyWants.sleep;
        }
        
        if (currentWant == BabyWants.bottle)
        {
            bottle.SetActive(true);
            
        }
        
        else if (currentWant == BabyWants.rattle)
        {
            rattle.SetActive(true);
            
        }
        
        else if (currentWant == BabyWants.nappy)
        {
            nappy.SetActive(true);
            
        }
        
        else if (currentWant == BabyWants.sleep)
        {
            sleep.SetActive(true);
           
        }
        thoughtBubble.SetActive(true);
        
        
    }

    void HappyFace()
    {
        happyHead.SetActive(true);
        sadHead.SetActive(false);
    }

    void SadFace()
    {
        sadHead.SetActive(true);
        happyHead.SetActive(false);
    }

    void CompletedSoundEffect()
    {
        if (currentWant == BabyWants.bottle)
        {
           
            Audio.AudioMaster.PlayAudioClipFX(0);
        }
        
        else if (currentWant == BabyWants.rattle)
        {
           
            Audio.AudioMaster.PlayAudioClipFX(5);
        }
        
        else if (currentWant == BabyWants.nappy)
        {
            
            Audio.AudioMaster.PlayAudioClipFX(3);
        }
        
        else if (currentWant == BabyWants.sleep)
        {
            
            Audio.AudioMaster.PlayAudioClipFX(4);
        }
    }
    void UpdateUI()
    {
        livesText.text = "Lives: " + health;
        scoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
        timeText.text = "Time: " + Mathf.Round(maxTimePerAction*100f)/100f; //occasional bug caused imperfect rounding
    }
    
//    private void OnGUI()
//    {
//        //GUI.Label(new Rect(10, 10, 1000, 20), actionToDisplay + " " + buttonPressString);
//        GUI.Label(new Rect(10, 30, 1000, 20), "Time: "+ maxTimePerAction.ToString("F2"));
//        GUI.Label(new Rect(10, 50, 1000, 20), "Health: "+ health);
//        GUI.Label(new Rect(10, 70, 1000, 20), "Score: "+ score);
//        //GUI.Label(new Rect(10, 90, 1000, 20), buttonString1 + " " + buttonString2);
//    }

    float timeBeforeStart = 2f; 
    IEnumerator Initialiser()
    {
        hourGlassTop.gameObject.SetActive(false);
        hourGlassBottom.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeBeforeStart);
        hourGlassTop.gameObject.SetActive(true);
        hourGlassBottom.gameObject.SetActive(true);
        ClearShapes(true);
        initialised = true;
        NewShapes();
    }

    IEnumerator ShapeDelay(bool x)
    {
        //waitingForShapes = true;
        ClearShapes(x);
       yield return new WaitForSeconds(shapeChangeDelay);
       if (health > 0)
       {
           
           Debug.Log("Woking");
           NewShapes();
       }

       waitingForShapes = false;
    }
}
