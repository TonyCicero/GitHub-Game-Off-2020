using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class computerAudio : MonoBehaviour
{

    [SerializeField]
    AudioSource src;
    [SerializeField]
    AudioClip[] clips;


    public void playClip(int c)
    {
        src.clip = clips[c];
        src.Play();
    }
}
