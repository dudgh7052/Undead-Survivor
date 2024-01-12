using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    public float m_moveSpeed = 0.0f;

    /// <summary>
    /// 현재체력
    /// </summary>
    public float m_health;

    /// <summary>
    /// 최대체력
    /// </summary>
    public float m_maxHealth;

    /// <summary>
    /// 런타임에 애니메이터를 바꾸기 위한 배열
    /// </summary>
    public RuntimeAnimatorController[] m_animCon;

    /// <summary>
    /// 타겟
    /// </summary>
    public Rigidbody2D m_target = null;

    /// <summary>
    /// 죽음 플래그
    /// </summary>
    bool m_isLive = false;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D m_rigid = null;

    /// <summary>
    /// 콜라이더 2D
    /// </summary>
    Collider2D m_coll = null;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator m_animator = null;

    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer m_sprite = null;

    /// <summary>
    /// 물리 업데이트 기다리는 시간
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
        m_coll.enabled = true; // 콜라이더 켜기
        m_rigid.simulated = true; // 리지드바디 시뮬레이션 켜기
        m_sprite.sortingOrder = 2; // 솔팅레이어 감소
        m_animator.SetBool("Dead", false); // 애니메이터 bool 속성 true로 바꾸기
        m_health = m_maxHealth;
    }


    void FixedUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        if (!m_isLive || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return; //GetCurrentAnimatorStateInfo(): 현재 상태 정보 가져오는 함수
        
        Vector2 _dirVec = m_target.position - m_rigid.position; // 방향 벡터 구하기
        Vector2 _nextVec = _dirVec.normalized * m_moveSpeed * Time.fixedDeltaTime; // 방향 벡터 normalized 한 뒤 속도와 fixedDeltaTime 곱하기
        m_rigid.MovePosition(m_rigid.position + _nextVec); // Rigidbody2D의 MovePosition(현재위치 + 이동 할 벡터값)으로 위치 바꾸기
        m_rigid.velocity = Vector2.zero; // 속도는 제로
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        if (!m_isLive) return;

        m_sprite.flipX = m_target.position.x < m_rigid.position.x;
    }

    /// <summary>
    /// 초기화
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
    /// 죽었을 때의 처리
    /// </summary>
    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return m_waitTime; // 하나의 물리 프레임 딜레이 주기
        Vector3 _playerPos = GameManager.Instance.Player.transform.position;
        Vector3 _dirVec = transform.position - _playerPos;
        m_rigid.AddForce(_dirVec.normalized * 3, ForceMode2D.Impulse); // 즉발적인 힘 == Impulse
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
            m_coll.enabled = false; // 콜라이더 끄기
            m_rigid.simulated = false; // 리지드바디 시뮬레이션 끄기
            m_sprite.sortingOrder = 1; // 솔팅레이어 감소
            m_animator.SetBool("Dead", true); // 애니메이터 bool 속성 true로 바꾸기
            GameManager.Instance.m_kill++;
            GameManager.Instance.GetExp();
        }
    }
}
