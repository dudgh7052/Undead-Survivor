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
        // WorldToScreenPoint() => ���� ���� ������Ʈ ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        m_rectTran.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.transform.position);
    }
}
