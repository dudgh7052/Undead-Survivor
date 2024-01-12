using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Prefab을 보관 할 변수
    /// </summary>
    public GameObject[] m_prefabs;

    /// <summary>
    /// 풀 담당을 하는 리스트
    /// </summary>
    List<GameObject>[] m_pools;

    /// <summary>
    /// 풀 정리용 부모 오브젝트
    /// </summary>
    List<GameObject> m_poolParents;

    void Awake()
    {
        m_pools = new List<GameObject>[m_prefabs.Length];
        m_poolParents = new List<GameObject>();

        for (int i = 0; i < m_pools.Length; i++)
        {
            m_pools[i] = new List<GameObject>();

            // 풀 별로 정리 할 리스트 초기화
            GameObject _tmpObj = new GameObject(); // 빈 오브젝트 생성 시 코드
            _tmpObj.name = m_prefabs[i].name + "Pool"; // name 프로퍼티를 통한 이름 변경
            _tmpObj.transform.parent = transform; // 오브젝트의 부모 설정 transform.parent
            m_poolParents.Add(_tmpObj);
        }
    }

    public GameObject Get(int argIndex)
    {
        GameObject _selectObj = null;

        // 선택한 풀의 놀고 있는(비활성화) GameObject 접근
        foreach (GameObject item in m_pools[argIndex])
        {
            // 발견하면 _selectObj에 할당
            if (!item.activeSelf) // GameObject.activeSelf로 활성화 체크
            {
                _selectObj = item;
                _selectObj.SetActive(true);
                break;
            }
        }

        // 못 찾으면?
        if (_selectObj == null)
        {
            // 새롭게 생성하고 select에 할당
            _selectObj = Instantiate(m_prefabs[argIndex], m_poolParents[argIndex].transform);
            m_pools[argIndex].Add(_selectObj); // 풀 리스트에 저장
        }

        return _selectObj;
    }
}
