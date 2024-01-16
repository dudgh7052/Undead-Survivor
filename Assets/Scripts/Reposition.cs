using System;
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

        switch (transform.tag)
        {
            case "Ground":
                // X, Y 멀어진 거리 구하기
                float _diffX = _playerPos.x - _myPos.x;
                float _diffY = _playerPos.y - _myPos.y;
                float _dirX = _diffX < 0 ? -1 : 1;
                float _dirY = _diffY < 0 ? -1 : 1;
                _diffX = Mathf.Abs(_diffX);
                _diffY = Mathf.Abs(_diffY);

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
                    Vector3 _dist = _playerPos - _myPos;
                    Vector3 _ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(_dist * 2 + _ran );
                }
                break;
        }
    }
}
