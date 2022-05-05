using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    #region ���
    public GameObject Intro;
    public Scrollbar scrollbar;
    public GameObject BattleNode;
    public Scrollbar scrollbar2;
    public Text BattleNodeText;
    public AudioSource audio;
    #endregion

    void Start()
    {
        BattleNodeText.text =
            "���֮��: " + PlayerPrefs.GetFloat("BattleNode_1").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_2").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_3").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_4").ToString("F2") + "\n\n";
    }

    #region ս��
    public void ClickBattle() { BattleNode.SetActive(true); }//������ܰ�ť
    public void ShutBattle()
    {
        if (scrollbar2.value == 1)
        {
            BattleNode.SetActive(false);
            scrollbar2.value = 0;
        }
    }
    public void ResetBattleNode()//����ս��
    {
        PlayerPrefs.SetFloat("BattleNode_1", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_2", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_3", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_4", 0.00f);
        BattleNodeText.text =
            "���֮��: " + PlayerPrefs.GetFloat("BattleNode_1").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_2").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_3").ToString("F2") + "\n\n" +
            "����֮��: " + PlayerPrefs.GetFloat("BattleNode_4").ToString("F2") + "\n\n";
    }
    #endregion

    #region ����
    public void ClickIntro() { Intro.SetActive(true); }//������ܰ�ť
    public void ShutIntro()
    {
        if (scrollbar.value == 1)
        {
            Intro.SetActive(false);
            scrollbar.value = 0;
        }
    }
    #endregion

    #region ����
    public void mouseVoice()
    {
        audio.Play();
    }
    #endregion
}
