using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    /// <summary>
    /// 스캔 할 범위
    /// </summary>
    public float m_scanRange = 0.0f;

    /// <summary>
    /// 레이어 마스크
    /// </summary>
    public LayerMask m_mask;

    /// <summary>
    /// 범위 내 타겟 배열
    /// </summary>
    public RaycastHit2D[] m_targets = null;

    /// <summary>
    /// 제일 근접한 타겟 트랜스폼
    /// </summary>
    public Transform m_nearestTarget = null;

    void FixedUpdate()
    {
        // Physics2D.CorcleCastAll(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
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

            // 타겟과의 거리 구하기 Vector3.Distance(MyVector, TargetVector); => return float
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
