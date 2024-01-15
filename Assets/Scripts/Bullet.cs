using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 데미지
    /// </summary>
    public float m_damage = 0.0f;

    /// <summary>
    /// 관통 수
    /// </summary>
    public int m_per = 0;

    Rigidbody2D m_rigid = null;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="argDamage">데미지</param>
    /// <param name="argPer">관통 수</param>
    /// /// <param name="argDir">방향</param>
    public void Init(float argDamage, int argPer, Vector3 argDir)
    {
        this.m_damage = argDamage;
        this.m_per = argPer;

        if (m_per > -1)
        {
            m_rigid.velocity = argDir * 15.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || m_per == -1) return;

        m_per--;

        if (m_per == -1)
        {
            m_rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
