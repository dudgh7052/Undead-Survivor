using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// ���� ID
    /// </summary>
    public int m_id = 0;

    /// <summary>
    /// ������ ID
    /// </summary>
    public int m_prefabId = 0;

    /// <summary>
    /// ������
    /// </summary>
    public float m_damage = 0.0f;

    /// <summary>
    /// ����, ���� ��
    /// </summary>
    public int m_count = 0;

    /// <summary>
    /// �ӵ�
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// Ÿ�̸�
    /// </summary>
    float m_timer = 0.0f;

    /// <summary>
    /// �÷��̾�
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
            case 0: // ��������
                transform.Rotate(Vector3.back * m_speed * Time.deltaTime);
                break;
            case 1: // ���Ÿ�����
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
    /// ������
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
        Hand _hand = m_player.m_hands[(int)argData.m_itemType]; // �������� enum �ε����� ���� ����, ���Ÿ� ���� ��������
        _hand.gameObject.SetActive(true); // Ȱ��ȭ
        _hand.m_spriter.sprite = argData.m_handSprite; // �����Ϳ� ������ ��������Ʈ �־��ֱ�

        // BroadcastMessage() : Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        m_player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < m_count; i++)
        {
            Transform _bullet = null;

            if (i < transform.childCount)
            {
                _bullet = transform.GetChild(i); // i�� 0�϶� �ڽĿ��� ���Ⱑ �ִٸ� ������.
            }
            else
            {
                // ���� �� ����
                _bullet = GameManager.Instance.m_poolManager.Get(m_prefabId).transform;
                _bullet.parent = transform;
            }

            // ��ġ, ȸ�� �ʱ�ȭ
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
        // Quaternion.FromToRotation()�� ���� �������� Rotation �����ֱ�
        _bullet.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        _bullet.GetComponent<Bullet>().Init(m_damage, m_count, _dir);
    }
}
