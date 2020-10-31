using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for audio management.
/// </summary>

public class AudioManager : MonoBehaviour
{
    static private AudioManager _inst;
    static public AudioManager Inst
    {
        get { return _inst; }
    }


    /// <summary>
    /// Audio Source component for the various sound effects.
    /// </summary>
    [SerializeField] AudioSource audioSource;


    /// <summary>
    /// Audio Source component only for the music.
    /// </summary>
    [SerializeField] AudioSource musicSource;


    private void Awake()
    {
        _inst = this;
    }


    /// <summary>
    /// Function playing a sound, depending on the type passed as a parameter
    /// </summary>
    /// <param name="st">Type of sound</param>
    public void PlaySound (SoundType st)
    {
        switch (st)
        {
            case (SoundType.Bounce):
                audioSource.clip = Resources.Load("Sounds/Bump sound") as AudioClip;
                break;

            case (SoundType.Bonus):
                audioSource.clip = Resources.Load("Sounds/Bonus collect") as AudioClip;
                break;

            case (SoundType.Speed):
                audioSource.clip = Resources.Load("Sounds/Speed sound") as AudioClip;
                break;

            case (SoundType.Rewind):
                audioSource.clip = Resources.Load("Sounds/rewind") as AudioClip;
                break;

            case (SoundType.Fail):
                audioSource.clip = Resources.Load("Sounds/Fail sound") as AudioClip;
                break;

            default: break;
        }

        audioSource.Play();
    }


    /// <summary>
    /// The different types of sounds being played during the game
    /// </summary>
    public enum SoundType
    {
        Bounce,
        Bonus,
        Speed,
        Rewind,
        Fail,
    }


    public void PauseMusic ()
    {
        musicSource.Pause();
    }


    public void PlayMusic ()
    {
        musicSource.Play();
     }
}
