using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Reference for soundmanager, can be accessed anywhere
    public static SoundManager Instance = null;
    //The audio source
    private AudioSource soundEffectAudio;

    //Initialize sound effects
    public AudioClip gunFire;
    public AudioClip upgradedGunFire; public AudioClip hurt;
    public AudioClip alienDeath;
    public AudioClip marineDeath;
    public AudioClip victory;
    public AudioClip elevatorArrived;
    public AudioClip powerUpPickup;
    public AudioClip powerUpAppear;

    // Start is called before the first frame update
    void Start()
    {
        //If this is the first SoundManager created, sets Instance to current object
        if (Instance == null)
        {
            Instance = this;
        }
        //if second soundmanager exists, destroy it
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (source.clip == null)
            {
                soundEffectAudio = source;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayOneShot(AudioClip clip)
    {
        soundEffectAudio.PlayOneShot(clip);
    }
}
