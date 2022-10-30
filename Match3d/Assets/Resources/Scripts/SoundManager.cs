using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Header("Audio source to Play Sounds")]
    public AudioSource MusicSource;
    public AudioSource SoundSource;

    [Header("Audio Clips")]
    public AudioClip Music;

    [UnityEngine.Serialization.FormerlySerializedAs("wrongmatch")]
    public AudioClip WrongMatch;
    public AudioClip ButtonSound;
    [UnityEngine.Serialization.FormerlySerializedAs("levelcompeletesound")]
    public AudioClip LevelCompleteSound;

    [UnityEngine.Serialization.FormerlySerializedAs("starcollect1")]
    public AudioClip StarCollect1;
    [UnityEngine.Serialization.FormerlySerializedAs("starcollect2")]
    public AudioClip StarCollect2;
    [UnityEngine.Serialization.FormerlySerializedAs("starcollect3")]
    public AudioClip StarCollect3;

    [UnityEngine.Serialization.FormerlySerializedAs("matchsound")]  
    public AudioClip MatchSound;

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
        PlayMusic();
        SetSoundSource();
        SetMusicSource();
    }

    public void SetSoundSource()
    {
        if (PrefManager.s_Instance.GetSoundsValue() == 1)
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
        if (PrefManager.s_Instance.GetMusicValue() == 1)
        {
            MusicSource.mute = false;
        }
        else
        {
            MusicSource.mute = true;
        }
    }

    public void PlayButtonSound()
    {
        SoundSource.PlayOneShot(ButtonSound);
    }

    public void PlayWrongMatchSound()
    {
        SoundSource.PlayOneShot(WrongMatch);
    }

    public void PlayBottleFillSound()
    {
        SoundSource.PlayOneShot(MatchSound);
    }

    public void PlayStarCollectSound()
    {
        SoundSource.PlayOneShot(StarCollect1);
        Invoke("PlayStar2", 0.2f);
    }

    void PlayStar2()
    {
        SoundSource.PlayOneShot(StarCollect2);
        Invoke("PlayStar3", 0.2f);
    }

    void PlayStar3()
    {
        SoundSource.PlayOneShot(StarCollect3);      
    }

    public void PlayMusic()
    {
        MusicSource.clip=Music;
        MusicSource.Play();
    }
}
