using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 m_inputVec = Vector2.zero; // �Է� ����
    public float m_moveSpeed = 0.0f; // ĳ���� �̵��ӵ�
    public Scanner m_scanner = null; // ��ĳ�� ��ũ��Ʈ
    public Hand[] m_hands; // �ڵ� ��ũ��Ʈ

    Rigidbody2D m_rigid = null; // ������ٵ�
    SpriteRenderer m_charSprite = null; // ĳ���� ��������Ʈ������
    Animator m_animator = null; // �ִϸ�����
    public RuntimeAnimatorController[] m_animController = null;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_charSprite = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_scanner = GetComponent<Scanner>();
        m_hands = GetComponentsInChildren<Hand>(true); // ������Ʈ�� �����ö� ��Ȱ��ȭ ��������� ()��ȣ �ȿ� true�� ������ ������
    }

    void OnEnable()
    {
        m_moveSpeed *= Character.Speed;
        m_animator.runtimeAnimatorController = m_animController[GameManager.Instance.m_playerId];
    }

    void FixedUpdate()
    {
        #region Other Movement Method
        // 1. ���� �ش�.
        //m_rigid.AddForce(m_inputVec);

        // 2. �ӵ� ����
        //m_rigid.velocity = m_inputVec;
        #endregion

        if (!GameManager.Instance.m_isLive) return;

        //PlayerInput���� �Է°��� Normalized ���ְ� �ֱ⿡ ���� m_inputVec.normalized �����൵ ��.
        Vector2 _nextVec = m_inputVec * m_moveSpeed * Time.fixedDeltaTime;
        m_rigid.MovePosition(m_rigid.position + _nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.m_isLive) return;

        m_animator.SetFloat("MoveSpeed", m_inputVec.magnitude); // vector2.magnitude: ������ ������ ũ�Ⱚ

        if (m_inputVec.x != 0)
        {
            m_charSprite.flipX = m_inputVec.x < 0;
        }
    }

    // Player Input�� OnMove(InputValue)�� �Է¹ޱ�
    void OnMove(InputValue argValue)
    {
        //argValue���� Vector2�� �ޱ�
        m_inputVec = argValue.Get<Vector2>();
    }

    /// <summary>
    /// �÷��̾ ���Ϳ� �浹 ������ üũ => ������Ʈ�� �����
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
