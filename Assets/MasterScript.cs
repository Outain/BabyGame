using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour {
    public string testing;
    public int[] pinkCheck = { 0, 1, 2, 3 };
    public int health = 3;
    private float timePerAction;
    public float maxTimePerAction;
    public float minTimePerAction;
    public float timeIncrement;
    public float shapeChangeDelay;
    public int score, highScore;
    //public Slider slidey;
    // controler side one active buttions
    public KeyCode[] controlerOneArray = { KeyCode.Joystick1Button0, KeyCode.Joystick1Button1, KeyCode.Joystick1Button2, KeyCode.Joystick1Button3 };
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
    public Text livesText, scoreText, highScoreText, timeText;
    private bool initialised = false; //using this to make sure time is only incremented after first shapes and not before.
    private bool waitingForShapes;

    private DataController dataController;
    private PlayerProgress playerProgress;

    public GameObject rattle, bottle, nappy, sleep;
    public GameObject thoughtBubble;

    public GameObject happyHead, sadHead;
    KeyCode[] fristHalf = { KeyCode.A, KeyCode.Z, KeyCode.Q, KeyCode.X, KeyCode.S, KeyCode.W, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.V, KeyCode.R, KeyCode.B, KeyCode.G };
    KeyCode[] secondHalf = { KeyCode.T, KeyCode.N, KeyCode.H, KeyCode.Y, KeyCode.M, KeyCode.J, KeyCode.U, KeyCode.K, KeyCode.I, KeyCode.L, KeyCode.O, KeyCode.P };

    char[] fristChars = { 'A', 'Z', 'Q', 'X', 'S', 'W', 'C', 'D', 'E', 'F', 'V', 'R', 'B', 'G' };
    char[] secondChars = { 'T', 'N', 'H', 'Y', 'M', 'J', 'U', 'K', 'I', 'L', 'O', 'P' };

    public Text[] activeTextFeild;
    KeyCode[] activeKeys = new KeyCode[2];
    char[] activeChar = new char[2];
    public enum BabyWants {
        bottle,
        rattle,
        nappy,
        sleep
    };

    public BabyWants currentWant;
    void Awake() {
        timePerAction = maxTimePerAction;
    }
    private void Start() {
        dataController = GetComponent<DataController>();


        StartCoroutine(Initialiser());

        highScore = PlayerPrefs.GetInt("highestScore");
        ClearObjects();

    }
    bool inputActive = true; 
    private void Update() {

 


        bool completedCheck = false;
        if (!waitingForShapes) {
            timePerAction -= Time.deltaTime;
        }
        //slidey.maxValue = maxTimePerAction;
        // slidey.value = timePerAction;



        // checks the active indexes againt the one need 
        if (Input.GetKey(activeKeys[0]) && Input.GetKey(activeKeys[1]) && inputActive) {
            waitingForShapes = true;
            inputActive = false; 
            score += 100;
            CompletedSoundEffect();
            dataController.SubmitNewPlayerScore(score);
            HappyFace();
            //Debug.Log("GG");
            // this code is also working with a basic controler 
            StartCoroutine(ShapeDelay(true)); // shapes rest score and health ++ ? 

        }

        if (timePerAction <= 0 && !waitingForShapes) {
            waitingForShapes = true;
            health--;
            dataController.SubmitNewPlayerScore(score);
            StartCoroutine(ShapeDelay(false));
        }


        UpdateUI();
        if (activeTextFeild[0] != null) {
            activeTextFeild[0].text = activeChar[0].ToString();
        }
        if (activeTextFeild[1] != null) {
            activeTextFeild[1].text = activeChar[1].ToString();
        }
        hourGlassFillAmount = (maxTimePerAction - timePerAction) / maxTimePerAction;
        //print(hourGlassFillAmount);
        if (hourGlassBottom != null) {
            hourGlassBottom.fillAmount = hourGlassFillAmount;
        }
        if (hourGlassTop != null) {
            hourGlassTop.fillAmount = (1 - hourGlassFillAmount);
        }
    }
    // calls new shaped to be used for the next index check
    bool audioType = false;
    void NewShapes() {
        timePerAction = maxTimePerAction;
        ChooseObject();
        Audio.AudioMaster.PlayAudioClipBackGround(0);
        int x = Mathf.RoundToInt(Random.Range(0, fristChars.Length));
        activeKeys[0] = fristHalf[x];
        activeChar[0] = fristChars[x];
        int y = Mathf.RoundToInt(Random.Range(0, secondChars.Length));
        activeKeys[1] = secondHalf[y];
        activeChar[1] = secondChars[y];
        SadFace();
        inputActive = true; 
    }

    // clears old shapes and starts the new spawning section 
    void ClearShapes(bool x) {


        if (x) {
            Audio.AudioMaster.GetComponent<Audio>().PlayAudioClipFX(1); // this is how to play audio mark :) 
        } else {
            Audio.AudioMaster.GetComponent<Audio>().PlayAudioClipFX(2);
        }

        if (maxTimePerAction > minTimePerAction) {
            if (initialised) {
                maxTimePerAction -= timeIncrement;
            }

            float sendToAudio = 1;
            sendToAudio = (float)Mathf.Abs((1 / maxTimePerAction) - (maxTimePerAction * 0.2f));
            Audio.AudioMaster.timeBetweenShiftChanges = sendToAudio;
            Audio.AudioMaster.audioPitchChange = 1 / maxTimePerAction;
        } else if (maxTimePerAction >= 3.5f) {
            maxTimePerAction -= 0.05f;
        }
        activeChar[0] = ' ';
        activeChar[1] = ' ';
        ClearObjects();
        //NewShapes(); 
    }

    void ClearObjects() {
        if(thoughtBubble != null) 
            thoughtBubble.SetActive(false);
        if (bottle != null)
            bottle.SetActive(false);
        rattle.SetActive(false);
        if (nappy != null)
            nappy.SetActive(false);
        if (sleep != null)
            sleep.SetActive(false);
    }
    [Header("Bottle, Rattle, nappy, sleep")]
    public GameObject[] visualAsset; 
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

        for (int x = 0; x < visualAsset.Length; x++) {


            if (x == i) {
                if (visualAsset[x] != null) {
                    visualAsset[x].SetActive(true);
                }
            } else {
                if (visualAsset[x] != null) {
                    visualAsset[x].SetActive(false);
                }
            } 

        }
        if (thoughtBubble != null) {
            thoughtBubble.SetActive(true);
        }
        
        
    }

    void HappyFace()
    {
        if (happyHead != null) {
            happyHead.SetActive(true);
        }
        if (sadHead != null) {
            sadHead.SetActive(false);
        }

    }

    void SadFace()
    {
        if (sadHead != null) {
            sadHead.SetActive(true);
        }
        if (happyHead != null) {
            happyHead.SetActive(false);
        }
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
        hourGlassBottom.fillAmount =0f;
        yield return new WaitForSeconds(timeBeforeStart);
        hourGlassTop.gameObject.SetActive(true);
        //hourGlassBottom.gameObject.SetActive(true);
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
