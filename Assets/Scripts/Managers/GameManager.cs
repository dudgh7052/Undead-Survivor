using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ���� �Ŵ���
    /// </summary>
    private static GameManager g_instance = null;

    [Header("# Game Control")]
    public bool m_isLive; // ���� ���� �÷���

    /// <summary>
    /// �帣�� ���� �ð�
    /// </summary>
    public float m_gameTime = 0.0f;

    /// <summary>
    /// �ִ� ���� �ð�
    /// </summary>
    public float m_maxGameTime = 2 * 10.0f;

    [Header("# Player Info")]
    public int m_playerId = 0;
    public float m_health = 0;
    public float m_maxHealth = 0;

    /// <summary>
    /// ����
    /// </summary>
    public int m_level = 0;

    /// <summary>
    /// óġ �� ��
    /// </summary>
    public int m_kill = 0;

    /// <summary>
    /// ����ġ
    /// </summary>
    public int m_exp = 0;

    /// <summary>
    /// ������ �ִ����ġ �迭
    /// </summary>
    public int[] m_nextExp = new int[10];

    [Header("# Game Object")]
    /// <summary>
    /// Ǯ �Ŵ���
    /// </summary>
    public PoolManager m_poolManager = null;

    /// <summary>
    /// �÷��̾� ��ũ��Ʈ
    /// </summary>
    [SerializeField] Player m_player = null;

    /// <summary>
    /// ���� �Ŵ��� ������Ƽ
    /// </summary>
    public static GameManager Instance { get { return g_instance; } }

    /// <summary>
    /// �÷��̾� ������Ƽ
    /// </summary>
    public Player Player { get { return m_player; } }

    /// <summary>
    /// ������ UI
    /// </summary>
    public LevelUp m_uiLevelUp;

    /// <summary>
    /// ��õ� UI
    /// </summary>
    public Result m_uiResult;

    /// <summary>
    /// ���̽�ƽ UI
    /// </summary>
    public Transform m_uiJoy;

    /// <summary>
    /// ���ʹ� Ŭ����
    /// </summary>
    public GameObject m_enemyCleaner = null;

    void Awake()
    {
        g_instance = this;
        Application.targetFrameRate = 60; // ���ø����̼� ���� �� Ÿ�� �������� 60���� ���� - �� �� �� 30���� ������
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
    /// ���� ����
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
    /// ���� ����
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
    /// ���� �¸�
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
    /// ���� �����
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void GameQuit()
    {
        Application.Quit();
    }

    public void GetExp()
    {
        if (!m_isLive) return;

        m_exp++;

        // Mathf.Min() �Լ��� ���� �� �߿� ���� ���� ���� => ���� �� ���� ���� �ݺ�
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
