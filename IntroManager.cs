using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    #region 组件
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
            "天空之城: " + PlayerPrefs.GetFloat("BattleNode_1").ToString("F2") + "\n\n" +
            "炎泽之都: " + PlayerPrefs.GetFloat("BattleNode_2").ToString("F2") + "\n\n" +
            "深蓝之海: " + PlayerPrefs.GetFloat("BattleNode_3").ToString("F2") + "\n\n" +
            "绿茵之林: " + PlayerPrefs.GetFloat("BattleNode_4").ToString("F2") + "\n\n";
    }

    #region 战绩
    public void ClickBattle() { BattleNode.SetActive(true); }//点击介绍按钮
    public void ShutBattle()
    {
        if (scrollbar2.value == 1)
        {
            BattleNode.SetActive(false);
            scrollbar2.value = 0;
        }
    }
    public void ResetBattleNode()//重置战绩
    {
        PlayerPrefs.SetFloat("BattleNode_1", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_2", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_3", 0.00f);
        PlayerPrefs.SetFloat("BattleNode_4", 0.00f);
        BattleNodeText.text =
            "天空之城: " + PlayerPrefs.GetFloat("BattleNode_1").ToString("F2") + "\n\n" +
            "炎泽之都: " + PlayerPrefs.GetFloat("BattleNode_2").ToString("F2") + "\n\n" +
            "深蓝之海: " + PlayerPrefs.GetFloat("BattleNode_3").ToString("F2") + "\n\n" +
            "绿茵之林: " + PlayerPrefs.GetFloat("BattleNode_4").ToString("F2") + "\n\n";
    }
    #endregion

    #region 介绍
    public void ClickIntro() { Intro.SetActive(true); }//点击介绍按钮
    public void ShutIntro()
    {
        if (scrollbar.value == 1)
        {
            Intro.SetActive(false);
            scrollbar.value = 0;
        }
    }
    #endregion

    #region 音乐
    public void mouseVoice()
    {
        audio.Play();
    }
    #endregion
}
