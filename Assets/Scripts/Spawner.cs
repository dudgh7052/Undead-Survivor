using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// ���� ������ �迭
    /// </summary>
    public Transform[] m_spawnPoints;

    /// <summary>
    /// ���������� �迭
    /// </summary>
    public SpawnData[] m_spawnData; 

    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField] int m_level = 0;

    /// <summary>
    /// �ð� �帧 Ÿ�̸�
    /// </summary>
    [SerializeField] float m_timer;

    void Awake()
    {
        m_spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.Instance.m_isLive) return;

        m_timer += Time.deltaTime;
        // Mathf.FloorToInt()�� �Ҽ��� �Ʒ� ������ �ݴ�� CeilToInt()
        m_level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.m_gameTime / 10.0f), m_spawnData.Length - 1);

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

[System.Serializable] //����ȭ �Ӽ� �ο�
public class SpawnData
{
    /// <summary>
    /// ��ȯ�ð�
    /// </summary>
    public float m_spawnTime = 0.0f;

    /// <summary>
    /// ��������Ʈ Ÿ��
    /// </summary>
    public int m_spriteType = 0;

    /// <summary>
    /// ü��
    /// </summary>
    public int m_health = 0;

    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float m_moveSpeed = 0.0f;
}
