using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    /// <summary>
    /// ��ĵ �� ����
    /// </summary>
    public float m_scanRange = 0.0f;

    /// <summary>
    /// ���̾� ����ũ
    /// </summary>
    public LayerMask m_mask;

    /// <summary>
    /// ���� �� Ÿ�� �迭
    /// </summary>
    public RaycastHit2D[] m_targets = null;

    /// <summary>
    /// ���� ������ Ÿ�� Ʈ������
    /// </summary>
    public Transform m_nearestTarget = null;

    void FixedUpdate()
    {
        // Physics2D.CorcleCastAll(ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�)
        m_targets = Physics2D.CircleCastAll(transform.position, m_scanRange, Vector2.zero, 0, m_mask);
        m_nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform _result = null;
        float _diff = 100.0f;

        foreach(RaycastHit2D target in m_targets)
        {
            Vector3 _myPos = transform.position;
            Vector3 _targetPos = target.transform.position;

            // Ÿ�ٰ��� �Ÿ� ���ϱ� Vector3.Distance(MyVector, TargetVector); => return float
            float _curDiff = Vector3.Distance(_myPos, _targetPos); 

            if (_curDiff < _diff)
            {
                _diff = _curDiff;
                _result = target.transform;
            }
        }

        return _result;
    }
}
