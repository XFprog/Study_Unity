using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour
{
    #region ���
    public GameObject Settings;
    public Transform Player;
    public GameObject ��ʼ���;
    public Text TimeText;
    public Text FailText;
    public Text SuccessText;
    public Transform �յ�;
    public Button nextIndex;
    public AudioSource successaudio;
    public AudioSource mouseaudio;
    #endregion

    #region ����
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

    #region ��ʵ��
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
    #region ����
    public void ClickSetting() { Settings.SetActive(true); Player.gameObject.SetActive(false); startSetting = true; mouseVoice(); }//������
    public void Continue() { Settings.SetActive(false); Player.gameObject.SetActive(true);startSetting = false; mouseVoice(); }//������Ϸ
    public void Return() //������ҳ��
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetString("1->2start", "true");
        mouseVoice();
    }
    public void End() { PlayerPrefs.SetString("fly", "false"); Application.Quit(); }//������Ϸ
    #endregion

    #region ��Ϸ����
    //��ʼ���
    public void ClickRunYou()//�������
    {
        ��ʼ���.SetActive(false);
        Player.gameObject.SetActive(true);
        TimeStart = true;
        TimeText.text = "Time: 0.00";
        mouseVoice();
    }
    public void ClickReturn()//�������
    {
        mouseVoice();
        SceneManager.LoadScene(2);
    }
    private void dealTime()//��ʱ����
    {
        if (TimeStart == true&&startSetting==false)
        {
            second += (float)Time.deltaTime;
            if (second > 60.000f)//�ۼ�ʱ��
            {
                minute += 1;
                second = 0;
            }
            if (minute >= 10)//��սʧ��
            {
                minute = 0; second = 0;
                TimeStart = false;
                failStart = true;//ʧ�ܵȴ�
                FailText.gameObject.SetActive(true);
            }
            TimeText.text = "Time: " + minute + "." + second.ToString("F0");
        }
    }
    private void success()//��ս�ɹ�
    {
        distance = Vector3.Distance(Player.position, �յ�.position);
        if(distance<8&&failStart==false)
        {
            SuccessVoice();
            battleNode = minute + second/100f;//��¼ս��
            SuccessText.gameObject.SetActive(true);
            TimeStart = false;
            nextIndex.gameObject.SetActive(true);
            winIndex = PlayerPrefs.GetInt("curScene");//��ȡ��ǰ�������
            switch(winIndex)
            {
                case 1:PlayerPrefs.SetString("���֮��", "true");break;
                case 2: PlayerPrefs.SetString("����֮��", "true"); break;
                case 3: PlayerPrefs.SetString("����֮��", "true"); break;
                case 4: PlayerPrefs.SetString("����֮��", "true"); break;
            }
        }
        else nextIndex.gameObject.SetActive(false);
    }
    private void SaveBattleNode()//����ս��
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
    public void ClickNext()//�����һ��
    {
        SceneManager.LoadScene(2);
    }
    private void dealFailWaitTime()//ʧ�ܼ�ʱ�ȴ�
    {
        if (failStart == true)
        {
            failWaitTime += Time.deltaTime;
            if (failWaitTime > 3f)
            {
                failWaitTime = 0;
                failStart = false;
                Player.position = new Vector3(0, 0, 0);//��ʱ�ؿ�
                ��ʼ���.SetActive(true);
                Player.gameObject.SetActive(false);
                FailText.gameObject.SetActive(false);
            }
            if (!GetComponents<AudioSource>()[3].isPlaying) GetComponents<AudioSource>()[3].Play();//ʧ����Ч
        }
    }
    public void die()//����
    {
        minute = 0; second = 0;
        TimeStart = false;
        failStart = true;//ʧ�ܵȴ�
        FailText.gameObject.SetActive(true);
    }
    #endregion

    #region ����
    public void SuccessVoice()//ʤ��֮��
    {
        if (!successaudio.isPlaying) successaudio.Play();
    }
    public void mouseVoice()//���������
    {
        mouseaudio.Play();
    }
    #endregion
}
