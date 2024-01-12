using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    /// <summary>
    /// �⺻ ������ ��� �ݶ��̴�2D ���� 
    /// </summary>
    Collider2D m_coll = null;

    void Awake()
    {
        m_coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        // Ÿ�ϸ��� Area�±׿��� ����� ��
        Vector3 _playerPos = GameManager.Instance.Player.transform.position;
        Vector3 _myPos = transform.position;

        // X, Y �־��� �Ÿ� ���ϱ�
        float _diffX = MathF.Abs(_playerPos.x - _myPos.x);
        float _diffY = MathF.Abs(_playerPos.y - _myPos.y);

        // �÷��̾� �Է¿� ���� Ÿ�Ϲ�ġ
        Vector3 _playerDir = GameManager.Instance.Player.m_inputVec;
        float _dirX = _playerDir.x < 0 ? -1 : 1;
        float _dirY = _playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (_diffX > _diffY)
                {
                    transform.Translate(Vector3.right * _dirX * 40); // Ÿ�ϸ� ũ�Ⱑ 20�̱⿡ 20 + 20 => 40
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
