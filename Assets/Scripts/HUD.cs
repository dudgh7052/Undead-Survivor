using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType m_infoType;

    Text m_myText;
    Slider m_mySlider;
    
    void Awake()
    {
        m_myText = GetComponent<Text>();
        m_mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (m_infoType)
        {
            case InfoType.Exp:
                float _curExp = GameManager.Instance.m_exp;
                float _maxExp = GameManager.Instance.m_nextExp[Mathf.Min(GameManager.Instance.m_level, GameManager.Instance.m_nextExp.Length - 1)];
                m_mySlider.value = _curExp / _maxExp;
                break;
            case InfoType.Level:
                m_myText.text = string.Format("Lv. {0:F0}", GameManager.Instance.m_level); // {0:F0} F1,F2로 소수점 표시 설정
                break;
            case InfoType.Kill:
                m_myText.text = string.Format("{0:F0}", GameManager.Instance.m_kill);
                break;
            case InfoType.Time:
                float _remainTime = GameManager.Instance.m_maxGameTime - GameManager.Instance.m_gameTime;
                int _min = Mathf.FloorToInt(_remainTime / 60); // FloorToInt()로 소수점 버리기
                int _sec = Mathf.FloorToInt(_remainTime % 60);
                m_myText.text = string.Format("{0:D2}:{1:D2}", _min, _sec); // D2로 자리수 2자리로 고정
                break;
            case InfoType.Health:
                float _curHealth = GameManager.Instance.m_health;
                float _maxHealth = GameManager.Instance.m_maxHealth;
                m_mySlider.value = _curHealth / _maxHealth;
                break;
        }
    }
}
