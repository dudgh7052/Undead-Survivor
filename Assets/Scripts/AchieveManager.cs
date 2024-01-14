using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    private static AchieveManager g_instance = null;

    /// <summary>
    /// 업적 매니저
    /// </summary>
    public static AchieveManager Instance
    {
        get { return g_instance; }
    }

    public enum Achieve { UnlockPotato, UnlockBean, MaxShovel, MaxGun}

    /// <summary>
    /// 잠금 캐릭터 배열
    /// </summary>
    public GameObject[] m_lockCharacter;

    /// <summary>
    /// 잠금 해제 캐릭터 배열
    /// </summary>
    public GameObject[] m_unlockCharacter;

    /// <summary>
    /// 알림 UI
    /// </summary>
    public GameObject m_uiNotice;

    /// <summary>
    /// 실제 시간 WaitForSeconds -> WaitForSeconds와 다르게 Time.TimeScale 영향을 받지 않음
    /// </summary>
    WaitForSecondsRealtime m_waitSecond;

    Achieve[] m_achieves;

    public Achieve[] Achieves { get { return m_achieves; } }

    void Awake()
    {
        g_instance = this;

        // GetValues(typoeof(EnumType))으로 Enum의 타입을 배열로 가져온다 -> 캐스팅을 통해 enum 배열 타입으로 변환
        m_achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        m_waitSecond = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    void Init()
    {
        // PlayerPrefs: 간단한 저장 기능을 제공하는 유니티 제공 클래스
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in m_achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in m_achieves)
        {
            CheckAchieve(achieve);
        }
    }

    /// <summary>
    /// 캐릭터 잠금 해제
    /// </summary>
    void UnlockCharacter()
    {
        for (int i = 0; i < m_lockCharacter.Length; i++)
        {
            string _achiveName = m_achieves[i].ToString();
            bool _isUnlock = PlayerPrefs.GetInt(_achiveName) == 1;

            // _isUnlock이 false라면 Lock ,true라면 Unlock 
            m_lockCharacter[i].SetActive(!_isUnlock);  
            m_unlockCharacter[i].SetActive(_isUnlock); 
        }
    }

    /// <summary>
    /// 업적 달성 체크
    /// </summary>
    /// <param name="argAchieve">업적</param>
    void CheckAchieve(Achieve argAchieve)
    {
        bool _isAchieve = false;

        switch (argAchieve)
        {
            case Achieve.UnlockPotato: // 몬스터 10마리 처치 시
                _isAchieve = GameManager.Instance.m_kill >= 10;
                break;
            case Achieve.UnlockBean: // maxGameTime동안 생존 시
                _isAchieve = GameManager.Instance.m_gameTime == GameManager.Instance.m_maxGameTime;
                break;
        }

        // 업적 달성, PlayerPrefs 데이터가 0일때
        if (_isAchieve && PlayerPrefs.GetInt(argAchieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(argAchieve.ToString(), 1);

            for (int i = 0; i < m_uiNotice.transform.childCount; i++)
            {
                bool _isActive = i == (int)argAchieve; // Achieve enumType의 인덱스와 i 가 같다면 _isActive true
                m_uiNotice.transform.GetChild(i).gameObject.SetActive(_isActive); // Notice UI에 순서대로 설정돼있기에 가져와서 켜주기
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    
    /// <summary>
    /// 업적 클리어
    /// </summary>
    /// <param name="argAchieveIndex">업적 인덱스</param>
    public void ClearAchieve(Achieve argAchieve)
    {
        if (PlayerPrefs.GetInt(argAchieve.ToString()) == 1) return; // 업적이 클리어 된 상태라면 return

        PlayerPrefs.SetInt(argAchieve.ToString(), 1);

        for (int i = 0; i < m_uiNotice.transform.childCount; i++)
        {
            bool _isActive = i == (int)argAchieve; // Achieve enumType의 인덱스와 i 가 같다면 _isActive true
            m_uiNotice.transform.GetChild(i).gameObject.SetActive(_isActive); // Notice UI에 순서대로 설정돼있기에 가져와서 켜주기
        }

        StartCoroutine(NoticeRoutine());
    }

    IEnumerator NoticeRoutine()
    {
        m_uiNotice.SetActive(true);

        yield return m_waitSecond;

        m_uiNotice.SetActive(false);
    }
}
