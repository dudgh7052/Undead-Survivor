using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType m_itemType;
    public int m_itemId; // 아이템 아이디
    public string m_itemName; // 아이템 이름
    [TextArea] // [TextArea]를 통해 인스펙터에 텍스트를 넣을수있음
    public string m_itemDesc; // 아이템 설명
    public Sprite m_itemIcon; // 아이템 아이콘

    [Header("# Level Data")]
    public float m_baseDamage;
    public int m_baseCount;
    public float[] m_damages;
    public int[] m_count;

    [Header("# Weapon")]
    public GameObject m_projectile;
    public Sprite m_handSprite;
}
