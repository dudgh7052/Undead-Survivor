using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float m_moveSpeed = 0.0f;

    /// <summary>
    /// ����ü��
    /// </summary>
    public float m_health;

    /// <summary>
    /// �ִ�ü��
    /// </summary>
    public float m_maxHealth;

    /// <summary>
    /// ��Ÿ�ӿ� �ִϸ����͸� �ٲٱ� ���� �迭
    /// </summary>
    public RuntimeAnimatorController[] m_animCon;

    /// <summary>
    /// Ÿ��
    /// </summary>
    public Rigidbody2D m_target = null;

    /// <summary>
    /// ���� �÷���
    /// </summary>
    bool m_isLive = false;

    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody2D m_rigid = null;

    /// <summary>
    /// �ݶ��̴� 2D
    /// </summary>
    Collider2D m_coll = null;

    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator m_animator = null;

    /// <summary>
    /// ��������Ʈ ������
    /// </summary>
    SpriteRenderer m_sprite = null;

    /// <summary>
    /// ���� ������Ʈ ��ٸ��� �ð�
    /// </summary>
    WaitForFixedUpdate m_waitTime;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_coll = GetComponent<Collider2D>();
        m_animator = GetComponent<Animator>();
        m_sprite = GetComponent<SpriteRenderer>();
        m_waitTime = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        m_target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        m_isLive = true;
        m_coll.enabled = true; // �ݶ��̴� �ѱ�
        m_rigid.simulated = true; // ������ٵ� �ùķ��̼� �ѱ�
        m_sprite.sortingOrder = 2; // ���÷��̾� ����
        m_animator.SetBool("Dead", false); // �ִϸ����� bool �Ӽ� true�� �ٲٱ�
        m_health = m_maxHealth;
    }


    void FixedUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        if (!m_isLive || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return; //GetCurrentAnimatorStateInfo(): ���� ���� ���� �������� �Լ�
        
        Vector2 _dirVec = m_target.position - m_rigid.position; // ���� ���� ���ϱ�
        Vector2 _nextVec = _dirVec.normalized * m_moveSpeed * Time.fixedDeltaTime; // ���� ���� normalized �� �� �ӵ��� fixedDeltaTime ���ϱ�
        m_rigid.MovePosition(m_rigid.position + _nextVec); // Rigidbody2D�� MovePosition(������ġ + �̵� �� ���Ͱ�)���� ��ġ �ٲٱ�
        m_rigid.velocity = Vector2.zero; // �ӵ��� ����
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        if (!m_isLive) return;

        m_sprite.flipX = m_target.position.x < m_rigid.position.x;
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="argData"></param>
    public void Init(SpawnData argData)
    {
        m_animator.runtimeAnimatorController = m_animCon[argData.m_spriteType];
        m_moveSpeed = argData.m_moveSpeed;
        m_maxHealth = argData.m_health;
        m_health = m_maxHealth;
    }


    /// <summary>
    /// �׾��� ���� ó��
    /// </summary>
    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return m_waitTime; // �ϳ��� ���� ������ ������ �ֱ�
        Vector3 _playerPos = GameManager.Instance.Player.transform.position;
        Vector3 _dirVec = transform.position - _playerPos;
        m_rigid.AddForce(_dirVec.normalized * 3, ForceMode2D.Impulse); // ������� �� == Impulse
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !m_isLive) return;

        m_health -= collision.GetComponent<Bullet>().m_damage;
        StartCoroutine(KnockBack());

        if (m_health > 0)
        {
            // Hit Effect
            m_animator.SetTrigger("Hit");
        }
        else
        {
            // Die
            m_isLive = false;
            m_coll.enabled = false; // �ݶ��̴� ����
            m_rigid.simulated = false; // ������ٵ� �ùķ��̼� ����
            m_sprite.sortingOrder = 1; // ���÷��̾� ����
            m_animator.SetBool("Dead", true); // �ִϸ����� bool �Ӽ� true�� �ٲٱ�
            GameManager.Instance.m_kill++;
            GameManager.Instance.GetExp();
        }
    }
}
