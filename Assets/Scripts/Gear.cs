using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType m_type; // 타입
    public float m_rate; // 레벨별 데이터

    public void Init(ItemData argData)
    {
        // Basic Setting
        name = "Gear " + argData.m_itemId;
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // Property Setting
        m_type = argData.m_itemType;
        m_rate = argData.m_damages[0];
        ApplyGear();
    }

    public void LevelUp(float argRate)
    {
        this.m_rate = argRate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (m_type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    /// <summary>
    /// 무기 속도 업
    /// </summary>
    void RateUp()
    {
        Weapon[] _weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in _weapons)
        {
            switch (weapon.m_id)
            {
                case 0:
                    float _speed = 150 * Character.WeaponSpeed;
                    weapon.m_speed = _speed + (_speed * m_rate);
                    break;
                default:
                    _speed = 0.5f * Character.WeaponRate;
                    weapon.m_speed = _speed * (1f - m_rate);
                    break;
            }
        }
    }

    /// <summary>
    /// 이동속도 업
    /// </summary>
    void SpeedUp()
    {
        float _moveSpeed = 3 * Character.Speed;
        GameManager.Instance.Player.m_moveSpeed = _moveSpeed + _moveSpeed * m_rate;
    }
}
