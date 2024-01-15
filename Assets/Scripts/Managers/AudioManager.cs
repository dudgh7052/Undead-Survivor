using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager g_instance;

    /// <summary>
    /// 오디오 매니저
    /// </summary>
    public static AudioManager Instance { get { return g_instance; } }

    [Header("#BGM")]
    public AudioClip m_bgmClip;
    public float m_bgmVolume;
    AudioSource m_bgmPlayer;
    AudioHighPassFilter m_bgmEffect;

    [Header("#SFX")]
    public AudioClip[] m_sfxClips;
    public float m_sfxVolume;
    // 다량의 효과음을 낼 수 있도록 채널 개수 선언
    public int m_channels;
    AudioSource[] m_sfxPlayers;
    int m_channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win}

    void Awake()
    {
        g_instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject _bgmObject = new GameObject("BgmPlayer");
        _bgmObject.transform.parent = transform;
        m_bgmPlayer = _bgmObject.AddComponent<AudioSource>();
        m_bgmPlayer.playOnAwake = false;
        m_bgmPlayer.loop = true;
        m_bgmPlayer.volume = m_bgmVolume;
        m_bgmPlayer.clip = m_bgmClip;
        m_bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject _sfxObject = new GameObject("SfxPlayer");
        _sfxObject.transform.parent = transform;
        m_sfxPlayers = new AudioSource[m_channels];

        for (int i = 0; i < m_sfxPlayers.Length; i++)
        {
            m_sfxPlayers[i] = _sfxObject.AddComponent<AudioSource>();
            m_sfxPlayers[i].playOnAwake = false;
            m_sfxPlayers[i].bypassListenerEffects = true; //AudioListener의 음향효과 영향 받지 않게 하기
            m_sfxPlayers[i].volume = m_sfxVolume;
        }
    }

    /// <summary>
    /// 배경음 실행, 정지 - true: 실행, false: 정지
    /// </summary>
    /// <param name="argIsPlay">실행 플래그</param>
    public void PlayBgm(bool argIsPlay)
    {
        if (argIsPlay)
        {
            m_bgmPlayer.Play();
        }
        else
        {
            m_bgmPlayer.Stop();
        }
    }

    /// <summary>
    /// 배경음 효과 실행 - true: 켜기, false: 끄기
    /// </summary>
    /// <param name="argIsPlay">실행 플래그</param>
    public void EffectBgm(bool argIsPlay)
    {
        m_bgmEffect.enabled = argIsPlay;
    }

    /// <summary>
    /// 효과음 실행
    /// </summary>
    /// <param name="argSfx">Sfx enum타입</param>
    public void PlaySfx(Sfx argSfx)
    {
        for (int i = 0; i < m_sfxPlayers.Length; i++)
        {
            int _loopIndex = (i + m_channelIndex) % m_sfxPlayers.Length;

            if (m_sfxPlayers[_loopIndex].isPlaying) continue;

            int _ranIndex = 0;
            if (argSfx == Sfx.Hit || argSfx == Sfx.Melee)
            {
                _ranIndex = Random.Range(0, 2);
            }

            m_channelIndex = _loopIndex;
            m_sfxPlayers[_loopIndex].clip = m_sfxClips[(int)argSfx + _ranIndex];
            m_sfxPlayers[_loopIndex].Play();
            break;
        }
    }
}
