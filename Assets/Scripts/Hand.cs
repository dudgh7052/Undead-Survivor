using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool IsLeft = false;
    public SpriteRenderer m_spriter;

    SpriteRenderer m_playerSpriter; // 플레이어 스프라이트

    Vector3 m_rightHandPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 m_rightHandPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Vector3 m_leftHandPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 m_leftHandPosReverse = new Vector3(0.1f, -0.2f, 0);
    Quaternion m_leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion m_leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        m_playerSpriter = GetComponentsInParent<SpriteRenderer>()[1]; // 부모의 것이 두번째기에 [1]
    }

    void LateUpdate()
    {
        bool _isReverse = m_playerSpriter.flipX;

        if (IsLeft) // 근접
        {
            transform.localRotation = _isReverse ? m_leftRotReverse : m_leftRot;
            m_spriter.flipY = _isReverse;
            m_spriter.sortingOrder = _isReverse ? 4 : 6;
        }
        else if (!IsLeft && GameManager.Instance.Player.m_scanner.m_nearestTarget) // 가까운적이 있을때
        {
            Vector3 _targetPos = GameManager.Instance.Player.m_scanner.m_nearestTarget.position;
            Vector3 _dir = _targetPos - transform.position;
            transform.localRotation = Quaternion.FromToRotation(Vector3.right, _dir);

            bool _isRotA = transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 270;
            bool _isRotB = transform.localRotation.eulerAngles.z < -90 && transform.localRotation.eulerAngles.z > -270;
            m_spriter.flipY = _isRotA || _isRotB;
            m_spriter.flipX = _isReverse;
            transform.localPosition = _isReverse ? m_leftHandPosReverse : m_leftHandPos;
            m_spriter.sortingOrder = 6;
        }
        else // 원거리
        {
            transform.localPosition = _isReverse ? m_leftHandPosReverse : m_leftHandPos;
            m_spriter.flipX = _isReverse;
            m_spriter.sortingOrder = _isReverse ? 6 : 4;
        }
    }
}
