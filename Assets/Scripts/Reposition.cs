using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    /// <summary>
    /// 기본 도형의 모든 콜라이더2D 포함 
    /// </summary>
    Collider2D m_coll = null;

    void Awake()
    {
        m_coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        // 타일맵이 Area태그에서 벗어났을 때
        Vector3 _playerPos = GameManager.Instance.Player.transform.position;
        Vector3 _myPos = transform.position;

        // X, Y 멀어진 거리 구하기
        float _diffX = MathF.Abs(_playerPos.x - _myPos.x);
        float _diffY = MathF.Abs(_playerPos.y - _myPos.y);

        // 플레이어 입력에 따른 타일배치
        Vector3 _playerDir = GameManager.Instance.Player.m_inputVec;
        float _dirX = _playerDir.x < 0 ? -1 : 1;
        float _dirY = _playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (_diffX > _diffY)
                {
                    transform.Translate(Vector3.right * _dirX * 40); // 타일맵 크기가 20이기에 20 + 20 => 40
                }
                else if (_dirX < _diffY)
                {
                    transform.Translate(Vector3.up * _dirY * 40);
                }
                break;
            case "Enemy":
                if (m_coll.enabled)
                {
                    transform.Translate(_playerDir * 20 + new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f));
                }
                break;
        }
    }
}
