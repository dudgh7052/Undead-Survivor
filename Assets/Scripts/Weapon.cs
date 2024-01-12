using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 무기 ID
    /// </summary>
    public int m_id = 0;

    /// <summary>
    /// 프리펩 ID
    /// </summary>
    public int m_prefabId = 0;

    /// <summary>
    /// 데미지
    /// </summary>
    public float m_damage = 0.0f;

    /// <summary>
    /// 개수, 관통 수
    /// </summary>
    public int m_count = 0;

    /// <summary>
    /// 속도
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// 타이머
    /// </summary>
    float m_timer = 0.0f;

    /// <summary>
    /// 플레이어
    /// </summary>
    public Player m_player;

    void Awake()
    {
        m_player = GameManager.Instance.Player;
    }

    void Update()
    {
        if (!GameManager.Instance.m_isLive) return;

        switch (m_id)
        {
            case 0: // 근접무기
                transform.Rotate(Vector3.back * m_speed * Time.deltaTime);
                break;
            case 1: // 원거리무기
                m_timer += Time.deltaTime;

                if (m_timer > m_speed)
                {
                    m_timer = default;

                    Fire();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 레벨업
    /// </summary>
    public void LevelUp(float argDamage, int argCount)
    {
        this.m_damage = argDamage;
        this.m_count += argCount;

        if (m_id == 0)
        {
            Batch();
        }

        m_player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData argData)
    {
        // Basic Setting
        name = "Weapon" + argData.m_itemId;
        transform.parent = m_player.transform;
        transform.localPosition = Vector3.zero;

        // Property Setting
        m_id = argData.m_itemId;
        m_damage = argData.m_baseDamage;
        m_count = argData.m_baseCount;

        for (int i = 0; i < GameManager.Instance.m_poolManager.m_prefabs.Length; i++)
        {
            if (argData.m_projectile == GameManager.Instance.m_poolManager.m_prefabs[i])
            {
                m_prefabId = i;
            }
        }

        switch(m_id)
        {
            case 0:
                m_speed = 150.0f;
                Batch();
                break;
            default:
                m_speed = 0.2f;
                break;
        }

        // Hand Setting
        Hand _hand = m_player.m_hands[(int)argData.m_itemType]; // 데이터의 enum 인덱싱을 통해 근접, 원거리 순서 가져오기
        _hand.gameObject.SetActive(true); // 활성화
        _hand.m_spriter.sprite = argData.m_handSprite; // 데이터에 설정한 스프라이트 넣어주기

        // BroadcastMessage() : 특정 함수 호출을 모든 자식에게 방송하는 함수
        m_player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < m_count; i++)
        {
            Transform _bullet = null;

            if (i < transform.childCount)
            {
                _bullet = transform.GetChild(i); // i가 0일때 자식에서 무기가 있다면 가져옴.
            }
            else
            {
                // 없을 시 생성
                _bullet = GameManager.Instance.m_poolManager.Get(m_prefabId).transform;
                _bullet.parent = transform;
            }

            // 위치, 회전 초기화
            _bullet.localPosition = Vector3.zero;
            _bullet.localRotation = Quaternion.identity;

            Vector3 _rotVec = Vector3.forward * 360 * i / m_count;
            _bullet.Rotate(_rotVec);
            _bullet.Translate(_bullet.up * 1.5f, Space.World);
            _bullet.GetComponent<Bullet>().Init(m_damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (m_player.m_scanner.m_nearestTarget == null) return;

        Vector3 _targetPos = m_player.m_scanner.m_nearestTarget.position;
        Vector3 _dir = _targetPos - transform.position;
        _dir = _dir.normalized;

        Transform _bullet = GameManager.Instance.m_poolManager.Get(m_prefabId).transform;
        _bullet.position = transform.position;
        // Quaternion.FromToRotation()을 통해 방향으로 Rotation 돌려주기
        _bullet.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        _bullet.GetComponent<Bullet>().Init(m_damage, m_count, _dir);
    }
}
