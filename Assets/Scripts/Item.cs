using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData m_data; // 아이템 데이터
    public int m_level; // 레벨
    public Weapon m_weapon; // 무기
    public Gear m_gear;

    Image m_icon; // 아이콘
    Text m_textLevel; // 레벨 텍스트
    Text m_textName; 
    Text m_textDesc;

    void Awake()
    {
        m_icon = GetComponentsInChildren<Image>()[1];
        m_icon.sprite = m_data.m_itemIcon;

        Text[] _texts = GetComponentsInChildren<Text>();
        m_textLevel = _texts[0];
        m_textName = _texts[1];
        m_textDesc = _texts[2];
        m_textName.text = m_data.m_itemName;
    }

    void OnEnable()
    {
        m_textLevel.text = "Lv." + (m_level + 1);

        switch (m_data.m_itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                m_textDesc.text = string.Format(m_data.m_itemDesc, m_data.m_damages[m_level] * 100, m_data.m_damages[m_level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                m_textDesc.text = string.Format(m_data.m_itemDesc, m_data.m_damages[m_level] * 100);
                break;
            default:
                m_textDesc.text = string.Format(m_data.m_itemDesc);
                break;
        }
    }

    /// <summary>
    /// 클릭 시
    /// </summary>
    public void OnClick()
    {
        switch (m_data.m_itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (m_level == 0)
                {
                    GameObject _newWeapon = new GameObject();
                    m_weapon = _newWeapon.AddComponent<Weapon>();
                    m_weapon.Init(m_data);
                }
                else
                {
                    float _nextDamage = m_data.m_baseDamage;
                    int _nextCount = 0;

                    _nextDamage += m_data.m_baseDamage * m_data.m_damages[m_level];
                    _nextCount += m_data.m_count[m_level];

                    m_weapon.LevelUp(_nextDamage, _nextCount);
                }
                m_level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (m_level == 0)
                {
                    GameObject _newGear = new GameObject();
                    m_gear = _newGear.AddComponent<Gear>();
                    m_gear.Init(m_data);
                }
                else
                {
                    float _nextRate = m_data.m_damages[m_level];
                    m_gear.LevelUp(_nextRate);
                }
                m_level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.Instance.m_health = GameManager.Instance.m_maxHealth;
                break;
        }

        // 버튼이 Damage의 길이와 같을 경우 버튼 비활성화
        if (m_level == m_data.m_damages.Length)
        {
            switch (m_data.m_itemType)        
            {
                case ItemData.ItemType.Melee:
                    AchieveManager.Instance.ClearAchieve(AchieveManager.Instance.Achieves[2]);
                    break;
                case ItemData.ItemType.Range:
                    AchieveManager.Instance.ClearAchieve(AchieveManager.Instance.Achieves[3]);
                    break;
            }

            GetComponent<Button>().interactable = false;
        }
    }
}
