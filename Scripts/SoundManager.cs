using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    #region Singleton
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    public AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;

    int channelidx;

    public enum Sfx { Down, BreakTile, Click, GameOver }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    void Init()
    {
        //BGM 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay) bgmPlayer.Play();
        else bgmPlayer.Stop();
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < channels; i++)
        {
            int loopindex = (i + channelidx) % channels;

            if (sfxPlayers[loopindex].isPlaying) //실행중일땐 끊기지 않고 실행
                continue;

            channelidx = loopindex;

            sfxPlayers[loopindex].clip = sfxClip[(int)sfx];
            sfxPlayers[loopindex].Play();
            break;
        }
    }

    public void MuteSound()
    {
        bgmPlayer.mute = !bgmPlayer.mute;
        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i].mute = !sfxPlayers[i].mute;
        }
    }
}
