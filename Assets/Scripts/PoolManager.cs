using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Prefab�� ���� �� ����
    /// </summary>
    public GameObject[] m_prefabs;

    /// <summary>
    /// Ǯ ����� �ϴ� ����Ʈ
    /// </summary>
    List<GameObject>[] m_pools;

    /// <summary>
    /// Ǯ ������ �θ� ������Ʈ
    /// </summary>
    List<GameObject> m_poolParents;

    void Awake()
    {
        m_pools = new List<GameObject>[m_prefabs.Length];
        m_poolParents = new List<GameObject>();

        for (int i = 0; i < m_pools.Length; i++)
        {
            m_pools[i] = new List<GameObject>();

            // Ǯ ���� ���� �� ����Ʈ �ʱ�ȭ
            GameObject _tmpObj = new GameObject(); // �� ������Ʈ ���� �� �ڵ�
            _tmpObj.name = m_prefabs[i].name + "Pool"; // name ������Ƽ�� ���� �̸� ����
            _tmpObj.transform.parent = transform; // ������Ʈ�� �θ� ���� transform.parent
            m_poolParents.Add(_tmpObj);
        }
    }

    public GameObject Get(int argIndex)
    {
        GameObject _selectObj = null;

        // ������ Ǯ�� ��� �ִ�(��Ȱ��ȭ) GameObject ����
        foreach (GameObject item in m_pools[argIndex])
        {
            // �߰��ϸ� _selectObj�� �Ҵ�
            if (!item.activeSelf) // GameObject.activeSelf�� Ȱ��ȭ üũ
            {
                _selectObj = item;
                _selectObj.SetActive(true);
                break;
            }
        }

        // �� ã����?
        if (_selectObj == null)
        {
            // ���Ӱ� �����ϰ� select�� �Ҵ�
            _selectObj = Instantiate(m_prefabs[argIndex], m_poolParents[argIndex].transform);
            m_pools[argIndex].Add(_selectObj); // Ǯ ����Ʈ�� ����
        }

        return _selectObj;
    }
}
