using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSourseBackGround, audioSourseFX;
    public static Audio AudioMaster;
    [Range(0f,2f)]
    public float audioPitchChange = 0.2f, timeBetweenShiftChanges = 0.2f; 
    void Start()
    {
        Audio.AudioMaster = gameObject.GetComponent<Audio>(); 
        //PlayAudioClipBackGround();
        //audioS = GetComponent<AudioSource>(); 
    }
    // list of items should be commented here for future Refrence
    public AudioClip[] audioClip, audioClipBackGround;

    void PlayAudioClipFX(int x = 0){
        

        audioSourseFX.PlayOneShot(audioClip[x]);
    }
    public void PlayAudioClipBackGround(int x = 0)
    {
        Debug.Log("Working");
        audioSourseBackGround.clip = audioClipBackGround[x];
        audioSourseBackGround.Play();
        audioSourseBackGround.pitch = 1f;
        if (x == 0)
        {

            InvokeRepeating("ToneShift", timeBetweenShiftChanges, timeBetweenShiftChanges);
        }
        else
        {
            CancelInvoke();

        }
    }
    void ToneShift(){

        if(audioSourseBackGround.pitch < 2){
            audioSourseBackGround.pitch += audioPitchChange;
        }

    } 
}
