using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 m_inputVec = Vector2.zero; // 입력 벡터
    public float m_moveSpeed = 0.0f; // 캐릭터 이동속도
    public Scanner m_scanner = null; // 스캐너 스크립트
    public Hand[] m_hands; // 핸드 스크립트

    Rigidbody2D m_rigid = null; // 리지드바디
    SpriteRenderer m_charSprite = null; // 캐릭터 스프라이트렌더러
    Animator m_animator = null; // 애니메이터
    public RuntimeAnimatorController[] m_animController = null;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_charSprite = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_scanner = GetComponent<Scanner>();
        m_hands = GetComponentsInChildren<Hand>(true); // 컴포넌트를 가져올때 비활성화 되있을경우 ()괄호 안에 true를 넣으면 가져옴
    }

    void OnEnable()
    {
        m_moveSpeed *= Character.Speed;
        m_animator.runtimeAnimatorController = m_animController[GameManager.Instance.m_playerId];
    }

    void FixedUpdate()
    {
        #region Other Movement Method
        // 1. 힘을 준다.
        //m_rigid.AddForce(m_inputVec);

        // 2. 속도 제어
        //m_rigid.velocity = m_inputVec;
        #endregion

        if (!GameManager.Instance.m_isLive) return;

        //PlayerInput에서 입력값을 Normalized 해주고 있기에 따로 m_inputVec.normalized 안해줘도 됨.
        Vector2 _nextVec = m_inputVec * m_moveSpeed * Time.fixedDeltaTime;
        m_rigid.MovePosition(m_rigid.position + _nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        m_animator.SetFloat("MoveSpeed", m_inputVec.magnitude); // vector2.magnitude: 벡터의 순수한 크기값

        if (m_inputVec.x != 0)
        {
            m_charSprite.flipX = m_inputVec.x < 0;
        }
    }

    // Player Input의 OnMove(InputValue)로 입력받기
    void OnMove(InputValue argValue)
    {
        //argValue값을 Vector2로 받기
        m_inputVec = argValue.Get<Vector2>();
    }

    /// <summary>
    /// 플레이어가 몬스터와 충돌 중인지 체크 => 업데이트와 비슷함
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.m_isLive) return;

        GameManager.Instance.m_health -= Time.deltaTime * 10;

        if (GameManager.Instance.m_health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            m_animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
