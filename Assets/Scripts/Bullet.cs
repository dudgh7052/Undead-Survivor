using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// ??????
    /// </summary>
    public float m_damage = 0.0f;

    /// <summary>
    /// ???? ??
    /// </summary>
    public int m_per = 0;

    Rigidbody2D m_rigid = null;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ????
    /// </summary>
    /// <param name="argDamage">??????</param>
    /// <param name="argPer">???? ??</param>
    /// /// <param name="argDir">????</param>
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
