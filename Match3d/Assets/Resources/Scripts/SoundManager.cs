using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Header("Audio source to Play Sounds")]
    public AudioSource MusicSource;
    public AudioSource SoundSource;

    [Header("Audio Clips")]
    public AudioClip Music;
    public AudioClip wrongmatch;
    public AudioClip ButtonSound;
    public AudioClip waterdropsound;
    public AudioClip levelcompeletesound;

    public AudioClip starcollect1;
    public AudioClip starcollect2;
    public AudioClip starcollect3;

    public AudioClip matchsound;
    public AudioClip confettiparticle;





    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playmusic();
        SetSoundSource();
        SetMusicSource();
    }

    public void SetSoundSource()
    {
        if (prefmanager.instance.Getsoundsvalue()==1)
        {
            SoundSource.mute = false;
        }
        else
        {
            SoundSource.mute = true;
        }
    }
    public void SetMusicSource()
    {
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            MusicSource.mute = false;
        }
        else
        {
            MusicSource.mute = true;
        }
    }
    public void MakeVibaration()
    {
        if (prefmanager.instance.Getvibrationsvalue() == 1)
        {
            Handheld.Vibrate();
        }
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonSOund()
    {
        SoundSource.PlayOneShot(ButtonSound);
    }
    public void PlaywaterdropSoundplay()
    {
        SoundSource.clip = waterdropsound;
        SoundSource.Play();
    }
    public void PlaywaterdropSOundstop()
    {
        SoundSource.clip = null;
    }

    

    public void PlayconfettiSOund()
    {
        SoundSource.PlayOneShot(confettiparticle);
    }



    public void wrongmatchsound()
    {

        SoundSource.PlayOneShot(wrongmatch);
    }

    public void PlaybottlfillSOund()
    {

        SoundSource.PlayOneShot(matchsound);
    }
    public void PlaylevelcompeleteSOund()
    {
        SoundSource.PlayOneShot(levelcompeletesound);
    }
    public void PlaystarcollectSOund()
    {
        SoundSource.PlayOneShot(starcollect1);
        Invoke("PlayStar2", 0.2f);
    }
     void PlayStar2()
    {
        SoundSource.PlayOneShot(starcollect2);
        Invoke("PlayStar3", 0.2f);
    }
    void PlayStar3()
    {
        SoundSource.PlayOneShot(starcollect3);
      
    }
    public void playmusic()
    {
        MusicSource.clip=Music;
        MusicSource.Play();
    }












}
