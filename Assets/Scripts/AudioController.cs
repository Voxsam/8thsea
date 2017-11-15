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
        None = -1,
        I,
        II,
    }

    public enum SoundEffect
    {
        None = -1,
        Bubbles,
        Bubbling_Sound,
        Camera_Shutter_I,
        Camera_Shutter_II,
        DNA_Extraction,
        Large_Bubble,
        Massage_Char,
        Scrap,
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
        "Camera Shutter I",
        "Camera Shutter II",
        "DNA Extraction",
        "Large Bubble",
        "Massage Chair",
        "Scrap",
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

    private bool fadeOutBGM = false;
    private bool fadeInBGM = false;
    
    public BGM CurrentBGM = BGM.None;
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
            BGM_clips[i] = Resources.Load<AudioClip>(BGM_FOLDER + BGM_names[i]);
        }

        SFX_clips = new AudioClip[SFX_names.Length];
        for (int i = 0; i < SFX_names.Length; i++)
        {
            SFX_clips[i] = Resources.Load<AudioClip>(SFX_FOLDER + SFX_names[i]);
        }

        areClipsLoaded = true;
    }

    /// <summary>
    /// Returns the appropriate AudioClip. If None, then returns null
    /// </summary>
    private AudioClip GetBGM(BGM clip)
    {
        if (clip == BGM.None)
        {
            return null;
        }
        return BGM_clips[(int)clip];
    }

    /// <summary>
    /// Returns the appropriate AudioClip. If None, then returns null
    /// </summary>
    private AudioClip GetSFX(SoundEffect clip)
    {
        if (clip == SoundEffect.None)
        {
            return null;
        }
        return SFX_clips[(int)clip];
    }

    #region BGM handler
    public void PlayBGM(BGM bgmToPlay)
    {
        if (GameControllerRef.allowBGM && bgmToPlay != BGM.None)
        {
            AudioClip newClip = GetBGM(bgmToPlay);
            // if conditions are fulfilled, swap to new BGM by fading into it
            if (BGM_player.clip == null || !BGM_player.isPlaying)
            {
                BGM_player.volume = 0f;
                BGM_player.clip = newClip;
                CurrentBGM = bgmToPlay;
                fadeInBGM = true;
            }
            else if (BGM_player.isPlaying && BGM_player.clip != newClip)
            {
                nextBGM = newClip;
                CurrentBGM = bgmToPlay;
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
    #endregion

    #region General SFX handler
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
    
    public void StopContinousSFX()
    {
        if (SFX_player.isPlaying)
        {
            SFX_player.Stop();
            SFX_player.clip = null;
        }
    }
    public bool IsSFXPlaying()
    {
        return SFX_player.isPlaying;
    }
    #endregion

    #region Player SFX handlers
    /// <summary>
    /// Used to loop sound effects continously at a specific player (one can be looped at a time)
    /// Can be used independently from PlaySFXContinously (meaning, a player can play a different sound effect continously even while one is being played generally)
    /// </summary>
    /// <param name="sfxToPlay"></param>
    public void PlaySFXContinuouslyAtPlayer(SoundEffect sfxToPlay, PlayerController pc)
    {
        if (GameControllerRef.allowSFX)
        {
            AudioClip newClip = GetSFX(sfxToPlay);
            if (pc.audioOutput.clip != newClip)
            {
                StopContinousSFXAtPlayer(pc);
                pc.audioOutput.clip = newClip;
                pc.audioOutput.volume = GameControllerRef.sfxMaximumVolume;
                pc.audioOutput.Play();
            }
        }
    }

    /// <summary>
    /// Used to loop sound effects continously at a specific player (one can be looped at a time)
    /// Can be used independently from PlaySFXContinously (meaning, a player can play a different sound effect continously even while one is being played generally)
    /// </summary>
    /// <param name="sfxToPlay"></param>
    public void PlaySFXContinuouslyAtPlayer(SoundEffect sfxToPlay, Player player)
    {
        PlaySFXContinuouslyAtPlayer(sfxToPlay, player.controller);
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

    public void StopContinousSFXAtPlayer(PlayerController pc)
    {
        if (IsSFXPlayingAtPlayer(pc))
        {
            pc.audioOutput.Stop();
            pc.audioOutput.clip = null;
        }
    }

    public void StopContinousSFXAtPlayer(Player player)
    {
        StopContinousSFXAtPlayer(player.controller);
    }

    public void StopContinousSFXAtPlayer(int playerNumber)
    {
        StopContinousSFXAtPlayer(GameControllerRef.GetPlayerNumber(playerNumber));
    }

    public bool IsSFXPlayingAtPlayer(PlayerController pc)
    {
        return pc.audioOutput.isPlaying;
    }

    public bool IsSFXPlayingAtPlayer(Player player)
    {
        return IsSFXPlayingAtPlayer(player.controller);
    }

    public bool IsSFXPlayingAtPlayer(int playerNumber)
    {
        return IsSFXPlayingAtPlayer(GameControllerRef.GetPlayerNumber(playerNumber));
    }
    #endregion

    // Updates once every frame
    void Update()
    {
        #region BGM Handlers
        if (GameControllerRef.allowBGM)
        {
            if (CurrentBGM != BGM.None && 
                GetBGM(CurrentBGM) != nextBGM && 
                GetBGM(CurrentBGM) != BGM_player.clip)
            {
                PlayBGM(CurrentBGM);
            }

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
                    nextBGM = null;
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
