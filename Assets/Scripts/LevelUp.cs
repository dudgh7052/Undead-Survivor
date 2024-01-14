using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform m_rect;
    Item[] m_items;

    void Awake()
    {
        m_rect = GetComponent<RectTransform>();
        m_items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        m_rect.localScale = Vector3.one;
        GameManager.Instance.Stop();
    }

    public void Hide()
    {
        m_rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();
    }

    /// <summary>
    /// 아이템 버튼 클릭
    /// </summary>
    /// <param name="argIndex">버튼 인덱스</param>
    public void Select(int argIndex)
    {
        m_items[argIndex].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화 
        foreach (Item item in m_items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 3개 아이템만 활성화
        int[] _random = new int[3];
        while(true)
        {
            _random[0] = Random.Range(0, m_items.Length);
            _random[1] = Random.Range(0, m_items.Length);
            _random[2] = Random.Range(0, m_items.Length);

            if (_random[0] != _random[1] && _random[1] != _random[2] && _random[0] != _random[2]) 
            { 
                break;
            }
        }

        for (int i = 0; i <_random.Length; i++)
        {
            Item _ranItem = m_items[_random[i]];

            // 3. 만렙 아이템의 경우 소비 아이템으로 대체
            if (_ranItem.m_level == _ranItem.m_data.m_damages.Length)
            {
                m_items[4].gameObject.SetActive(true);
            }
            else
            {
                _ranItem.gameObject.SetActive(true);
            }
        }
    }
}
