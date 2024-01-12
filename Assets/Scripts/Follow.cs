using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform m_rectTran;

    void Awake()
    {
        m_rectTran = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // WorldToScreenPoint() => 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        m_rectTran.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.transform.position);
    }
}
