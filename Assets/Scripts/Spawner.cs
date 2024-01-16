using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 스폰 포인터 배열
    /// </summary>
    public Transform[] m_spawnPoints;

    /// <summary>
    /// 스폰데이터 배열
    /// </summary>
    public SpawnData[] m_spawnData; 

    /// <summary>
    /// 게임 레벨
    /// </summary>
    [SerializeField] int m_level = 0;

    /// <summary>
    /// 시간 흐름 타이머
    /// </summary>
    [SerializeField] float m_timer;

    [SerializeField] float m_levelTime;

    void Awake()
    {
        m_spawnPoints = GetComponentsInChildren<Transform>(); // 스폰 포인트 가져오기
        m_levelTime = GameManager.Instance.m_maxGameTime / m_spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.Instance.m_isLive) return;

        m_timer += Time.deltaTime;
        // Mathf.FloorToInt()로 소수점 아래 버리기 반대는 CeilToInt()
        // 게임 시간에 따라 스폰 레벨 변화
        m_level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.m_gameTime / m_levelTime), m_spawnData.Length - 1);

        if (m_timer > m_spawnData[m_level].m_spawnTime)
        {
            m_timer = 0.0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject _enemy = GameManager.Instance.m_poolManager.Get(0);
        _enemy.transform.position = m_spawnPoints[Random.Range(1, m_spawnPoints.Length)].position;
        _enemy.GetComponent<Enemy>().Init(m_spawnData[m_level]);
    }
}

[System.Serializable] //직렬화 속성 부여
public class SpawnData
{
    /// <summary>
    /// 소환시간
    /// </summary>
    public float m_spawnTime = 0.0f;

    /// <summary>
    /// 스프라이트 타입
    /// </summary>
    public int m_spriteType = 0;

    /// <summary>
    /// 체력
    /// </summary>
    public int m_health = 0;

    /// <summary>
    /// 이동속도
    /// </summary>
    public float m_moveSpeed = 0.0f;
}
