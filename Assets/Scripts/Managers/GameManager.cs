using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    private static GameManager g_instance = null;

    [Header("# Game Control")]
    public bool m_isLive; // 게임 시작 플래그

    /// <summary>
    /// 흐르는 게임 시간
    /// </summary>
    public float m_gameTime = 0.0f;

    /// <summary>
    /// 최대 게임 시간
    /// </summary>
    public float m_maxGameTime = 2 * 10.0f;

    [Header("# Player Info")]
    public int m_playerId = 0;
    public float m_health = 0;
    public float m_maxHealth = 0;

    /// <summary>
    /// 레벨
    /// </summary>
    public int m_level = 0;

    /// <summary>
    /// 처치 한 수
    /// </summary>
    public int m_kill = 0;

    /// <summary>
    /// 경험치
    /// </summary>
    public int m_exp = 0;

    /// <summary>
    /// 레벨별 최대경험치 배열
    /// </summary>
    public int[] m_nextExp = new int[10];

    [Header("# Game Object")]
    /// <summary>
    /// 풀 매니저
    /// </summary>
    public PoolManager m_poolManager = null;

    /// <summary>
    /// 플레이어 스크립트
    /// </summary>
    [SerializeField] Player m_player = null;

    /// <summary>
    /// 게임 매니저 프로퍼티
    /// </summary>
    public static GameManager Instance { get { return g_instance; } }

    /// <summary>
    /// 플레이어 프로퍼티
    /// </summary>
    public Player Player { get { return m_player; } }

    /// <summary>
    /// 레벨업 UI
    /// </summary>
    public LevelUp m_uiLevelUp;

    /// <summary>
    /// 재시도 UI
    /// </summary>
    public Result m_uiResult;

    /// <summary>
    /// 조이스틱 UI
    /// </summary>
    public Transform m_uiJoy;

    /// <summary>
    /// 에너미 클리너
    /// </summary>
    public GameObject m_enemyCleaner = null;

    void Awake()
    {
        g_instance = this;
        Application.targetFrameRate = 60; // 어플리케이션 실행 시 타겟 프레임을 60으로 지정 - 안 할 시 30으로 지정됨
    }

    void Update()
    {
        if (!m_isLive) return;

        m_gameTime += Time.deltaTime;

        if (m_gameTime > m_maxGameTime)
        {
            m_gameTime = m_maxGameTime;
            GameVictory();
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart(int argId)
    {
        m_playerId = argId;
        m_health = m_maxHealth;

        m_player.gameObject.SetActive(true);
        m_uiLevelUp.Select(m_playerId % 2);

        Resume();

        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }

    /// <summary>
    /// 게임 오버
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        m_isLive = false;

        yield return new WaitForSeconds(0.5f);

        m_uiResult.gameObject.SetActive(true);
        m_uiResult.Lose();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    /// <summary>
    /// 게임 승리
    /// </summary>
    public void GameVictory()
    {
        StartCoroutine(GameVictoryCoroutine());
    }

    IEnumerator GameVictoryCoroutine()
    {
        m_isLive = false;
        m_enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        m_uiResult.gameObject.SetActive(true);
        m_uiResult.Win();
        Stop();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
        AudioManager.Instance.PlayBgm(false);
    }

    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void GameQuit()
    {
        Application.Quit();
    }

    public void GetExp()
    {
        if (!m_isLive) return;

        m_exp++;

        // Mathf.Min() 함수를 통해 둘 중에 낮은 수를 리턴 => 만렙 시 무한 레벨 반복
        if (m_exp == m_nextExp[Mathf.Min(m_level, m_nextExp.Length - 1)]) 
        {
            m_level++;
            m_exp = 0;
            m_uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        m_isLive = false;
        Time.timeScale = 0;
        m_uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        m_isLive = true;
        Time.timeScale = 1;
        m_uiJoy.localScale = Vector3.one;
    }
}
