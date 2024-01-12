using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType m_itemType;
    public int m_itemId; // ������ ���̵�
    public string m_itemName; // ������ �̸�
    [TextArea] // [TextArea]�� ���� �ν����Ϳ� �ؽ�Ʈ�� ����������
    public string m_itemDesc; // ������ ����
    public Sprite m_itemIcon; // ������ ������

    [Header("# Level Data")]
    public float m_baseDamage;
    public int m_baseCount;
    public float[] m_damages;
    public int[] m_count;

    [Header("# Weapon")]
    public GameObject m_projectile;
    public Sprite m_handSprite;
}
