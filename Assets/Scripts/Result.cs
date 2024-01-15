using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] m_titles;

    public void Lose()
    {
        m_titles[0].SetActive(true);
    }

    public void Win()
    {
        m_titles[1].SetActive(true);
    }
}
