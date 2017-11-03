using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Obj;

    public GameController GameControllerRef
    {
        get { return GameController.Obj; }
    }

    public enum BGM
    {
        I = 0,
        II,
    }

    public enum SoundEffect
    {
        Bubbles = 0,
        Bubbling_Sound,
        DNA_Extraction,
        Large_Bubble,
        Massage_Char,
        Scrub,
        Suck_Up,
        Suction,
    }

    private string[] BGM_names =
    {
        "Background Music I",
        "Background Music II",
    };

    private string[] SFX_names =
    {
        "Bubbles",
        "Bubbling Sound",
        "DNA Extraction",
        "Large Bubble",
        "Massage Chair",
        "Scrub",
        "Suck Up",
        "Suction",
    };

    private const string BGM_FOLDER = "BGM/";
    private const string SFX_FOLDER = "SFX/";
    private const float BGM_FADE_TIME = 1f;

    private float timeToWaitUntil;

    private AudioClip[] BGM_clips;
    private AudioClip[] SFX_clips;

    [SerializeField] private AudioSource BGM_player;
    [SerializeField] private AudioSource SFX_player;

    private bool areClipsLoaded = false;

    [SerializeField] private bool fadeOutBGM = false;
    [SerializeField] private bool fadeInBGM = false;
    private AudioClip nextBGM = null;

    private List<AudioClip> nextSFX = null;

    void Awake()
    {
        if (Obj == null)
        {
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Setup();
    }

    public bool AreClipsLoaded
    {
        get { return areClipsLoaded; }
    }

    private void Setup()
    {
        LoadAudioClips();
        nextSFX = new List<AudioClip>();
        BGM_player.loop = true;
        SFX_player.loop = true;
        timeToWaitUntil = 0f;
    }

    private void LoadAudioClips()
    {
        BGM_clips = new AudioClip[BGM_names.Length];
        for(int i = 0; i < BGM_names.Length; i++)
        {
            BGM_clips[i] = Resources.Load<AudioClip>(BGM_names[i]);
        }

        SFX_clips = new AudioClip[SFX_names.Length];
        for (int i = 0; i < SFX_names.Length; i++)
        {
            SFX_clips[i] = Resources.Load<AudioClip>(SFX_FOLDER + SFX_names[i]);
        }

        areClipsLoaded = true;
    }

    private AudioClip GetBGM(BGM clip)
    {
        return BGM_clips[(int)clip];
    }

    private AudioClip GetSFX(SoundEffect clip)
    {
        return SFX_clips[(int)clip];
    }

    public void PlayBGM(BGM bgmToPlay)
    {
        if (GameControllerRef.allowBGM)
        {
            AudioClip newClip = GetBGM(bgmToPlay);
            // if conditions are fulfilled, swap to new BGM by fading into it
            if (BGM_player.clip == null || !BGM_player.isPlaying)
            {
                BGM_player.volume = 0f;
                BGM_player.clip = newClip;
                fadeInBGM = true;
            }
            else if (BGM_player.isPlaying && BGM_player.clip != newClip)
            {
                nextBGM = newClip;
                fadeOutBGM = true; // Fade in will come in after
                timeToWaitUntil = Time.time + BGM_FADE_TIME;
            }
        }
    }

    public void StopBGM()
    {
        fadeOutBGM = true;
        fadeInBGM = false;
        timeToWaitUntil = Time.time + BGM_FADE_TIME;
    }

    public void PlaySFXOnce(SoundEffect sfxToPlay)
    {
        if (GameControllerRef.allowSFX)
        {
            AudioClip newClip = GetSFX(sfxToPlay);
            foreach (AudioClip clip in nextSFX)
            {
                if (newClip == clip)
                {
                    return; // Don't do anything
                }
            }
            // if make it to the end without a duplicate
            nextSFX.Add(newClip);
        }
    }

    /// <summary>
    /// Used to loop sound effects continously (one can be looped at a time)
    /// For example, alarm constantly ringing, etc
    /// </summary>
    /// <param name="sfxToPlay"></param>
    public void PlaySFXContinuously(SoundEffect sfxToPlay)
    {
        if (GameControllerRef.allowSFX)
        {
            StopContinousSFX();
            SFX_player.clip = GetSFX(sfxToPlay);
            SFX_player.volume = GameControllerRef.sfxMaximumVolume;
            SFX_player.Play();
        }
    }

    /// <summary>
    /// Used to loop sound effects continously at a specific player (one can be looped at a time)
    /// Can be used independently from PlaySFXContinously (meaning, a player can play a different sound effect continously even while one is being played generally)
    /// </summary>
    /// <param name="sfxToPlay"></param>
    public void PlaySFXContinuouslyAtPlayer(SoundEffect sfxToPlay, Player player)
    {
        if (GameControllerRef.allowSFX)
        {
            StopContinousSFXAtPlayer(player);
            SFX_player.clip = GetSFX(sfxToPlay);
            SFX_player.volume = GameControllerRef.sfxMaximumVolume;
            SFX_player.Play();
        }
    }

    /// <summary>
    /// Used to loop sound effects continously at a specific player (one can be looped at a time)
    /// Can be used independently from PlaySFXContinously (meaning, a player can play a different sound effect continously even while one is being played generally)
    /// </summary>
    /// <param name="sfxToPlay"></param>
    public void PlaySFXContinuouslyAtPlayer(SoundEffect sfxToPlay, int playerNum)
    {
        PlaySFXContinuouslyAtPlayer(sfxToPlay, GameControllerRef.GetPlayerNumber(playerNum));
    }

    public void StopContinousSFX()
    {
        if (SFX_player.isPlaying)
        {
            SFX_player.Stop();
        }
    }

    public void StopContinousSFXAtPlayer(Player player)
    {
        if (player.controller.audioOutput.isPlaying)
        {
            player.controller.audioOutput.Stop();
        }
    }

    public void StopContinousSFXAtPlayer(int playerNumber)
    {
        StopContinousSFXAtPlayer(GameControllerRef.GetPlayerNumber(playerNumber));
    }

    // Updates once every frame
    void Update()
    {
        #region BGM Handlers
        if (GameControllerRef.allowBGM)
        {
            if (fadeOutBGM && BGM_player.isPlaying)
            {
                float timeLeft = timeToWaitUntil - Time.time;
                float timeSpent = BGM_FADE_TIME - timeLeft;
                BGM_player.volume = Mathf.Lerp(GameControllerRef.bgmMaximumVolume, 0f, timeSpent / BGM_FADE_TIME);
                if (BGM_player.volume <= 0f)
                {
                    fadeOutBGM = false;
                    BGM_player.Stop();
                    BGM_player.clip = nextBGM;
                    fadeInBGM = true;
                }
            }
            else if (fadeInBGM)
            {
                if (!BGM_player.isPlaying)
                {
                    BGM_player.volume = 0f;
                    BGM_player.Play();
                    timeToWaitUntil = Time.time + BGM_FADE_TIME;
                }

                float timeLeft = timeToWaitUntil - Time.time;
                float timeSpent = BGM_FADE_TIME - timeLeft;
                BGM_player.volume = Mathf.Lerp(0f, GameControllerRef.bgmMaximumVolume, timeSpent / BGM_FADE_TIME);
                if (BGM_player.volume >= GameControllerRef.bgmMaximumVolume)
                {
                    BGM_player.volume = GameControllerRef.bgmMaximumVolume;
                    fadeInBGM = false;
                }
            }
        }
        #endregion

        // This prevents a clip from being played more than once per frame
        if (GameControllerRef.allowSFX && nextSFX.Count > 0)
        {
            foreach(AudioClip clip in nextSFX)
            {
                SFX_player.PlayOneShot(clip, GameControllerRef.sfxMaximumVolume);
            }
            nextSFX.Clear();
        }
    }
}
