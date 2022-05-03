using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour
{
    #region 组件
    public GameObject Settings;
    public Transform Player;
    public GameObject 初始面板;
    public Text TimeText;
    public Text FailText;
    public Text SuccessText;
    public Transform 终点;
    public Button nextIndex;
    public AudioSource successaudio;
    public AudioSource mouseaudio;
    #endregion

    #region 变量
    private bool startSetting = false;
    private bool TimeStart = false;
    private int minute = 0;
    private float second = 0;
    private float battleNode = -1;
    private bool failStart = false;
    private float failWaitTime = 0;
    private float distance=0;
    private int winIndex;
    #endregion

    #region 类实例
    public static PlaceManager placemanager;
    #endregion

    void Start()
    {
        Player.gameObject.SetActive(false);
    }

    void Update()
    {
        success();
        SaveBattleNode();
        dealTime();
        dealFailWaitTime();
    }
    #region 设置
    public void ClickSetting() { Settings.SetActive(true); Player.gameObject.SetActive(false); startSetting = true; mouseVoice(); }//打开设置
    public void Continue() { Settings.SetActive(false); Player.gameObject.SetActive(true);startSetting = false; mouseVoice(); }//继续游戏
    public void Return() //返回主页面
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetString("1->2start", "true");
        mouseVoice();
    }
    public void End() { PlayerPrefs.SetString("fly", "false"); Application.Quit(); }//结束游戏
    #endregion

    #region 游戏核心
    //初始面板
    public void ClickRunYou()//点击走你
    {
        初始面板.SetActive(false);
        Player.gameObject.SetActive(true);
        TimeStart = true;
        TimeText.text = "Time: 0.00";
        mouseVoice();
    }
    public void ClickReturn()//点击返回
    {
        mouseVoice();
        SceneManager.LoadScene(2);
    }
    private void dealTime()//计时处理
    {
        if (TimeStart == true&&startSetting==false)
        {
            second += (float)Time.deltaTime;
            if (second > 60.000f)//累计时间
            {
                minute += 1;
                second = 0;
            }
            if (minute >= 10)//挑战失败
            {
                minute = 0; second = 0;
                TimeStart = false;
                failStart = true;//失败等待
                FailText.gameObject.SetActive(true);
            }
            TimeText.text = "Time: " + minute + "." + second.ToString("F0");
        }
    }
    private void success()//挑战成功
    {
        distance = Vector3.Distance(Player.position, 终点.position);
        if(distance<8&&failStart==false)
        {
            SuccessVoice();
            battleNode = minute + second/100f;//记录战绩
            SuccessText.gameObject.SetActive(true);
            TimeStart = false;
            nextIndex.gameObject.SetActive(true);
            winIndex = PlayerPrefs.GetInt("curScene");//获取当前场景序号
            switch(winIndex)
            {
                case 1:PlayerPrefs.SetString("天空之城", "true");break;
                case 2: PlayerPrefs.SetString("炎泽之都", "true"); break;
                case 3: PlayerPrefs.SetString("深蓝之海", "true"); break;
                case 4: PlayerPrefs.SetString("绿茵之林", "true"); break;
            }
        }
        else nextIndex.gameObject.SetActive(false);
    }
    private void SaveBattleNode()//保存战绩
    {
        if(battleNode>0)
        {
            switch (winIndex)
            {
                case 1: if(battleNode<PlayerPrefs.GetFloat("BattleNode_1")||PlayerPrefs.GetFloat("BattleNode_1")==0.00f) 
                        PlayerPrefs.SetFloat("BattleNode_1",battleNode); break;
                case 2: if (battleNode < PlayerPrefs.GetFloat("BattleNode_2") || PlayerPrefs.GetFloat("BattleNode_2") == 0.00f) 
                        PlayerPrefs.SetFloat("BattleNode_2", battleNode); break;
                case 3: if (battleNode < PlayerPrefs.GetFloat("BattleNode_3") || PlayerPrefs.GetFloat("BattleNode_3") == 0.00f)
                        PlayerPrefs.SetFloat("BattleNode_3", battleNode); break;
                case 4: if (battleNode < PlayerPrefs.GetFloat("BattleNode_4") || PlayerPrefs.GetFloat("BattleNode_4") == 0.00f)
                        PlayerPrefs.SetFloat("BattleNode_4", battleNode); break;
            }
            battleNode = -1;
        }
    }
    public void ClickNext()//点击下一关
    {
        SceneManager.LoadScene(2);
    }
    private void dealFailWaitTime()//失败计时等待
    {
        if (failStart == true)
        {
            failWaitTime += Time.deltaTime;
            if (failWaitTime > 3f)
            {
                failWaitTime = 0;
                failStart = false;
                Player.position = new Vector3(0, 0, 0);//超时重开
                初始面板.SetActive(true);
                Player.gameObject.SetActive(false);
                FailText.gameObject.SetActive(false);
            }
            if (!GetComponents<AudioSource>()[3].isPlaying) GetComponents<AudioSource>()[3].Play();//失败音效
        }
    }
    public void die()//死亡
    {
        minute = 0; second = 0;
        TimeStart = false;
        failStart = true;//失败等待
        FailText.gameObject.SetActive(true);
    }
    #endregion

    #region 音乐
    public void SuccessVoice()//胜利之歌
    {
        if (!successaudio.isPlaying) successaudio.Play();
    }
    public void mouseVoice()//鼠标点击音乐
    {
        mouseaudio.Play();
    }
    #endregion
}
