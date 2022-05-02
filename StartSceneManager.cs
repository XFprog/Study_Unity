using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    #region ����
    private bool scater = false;
    #endregion

    #region ���
    public Transform L;
    public Transform R;
    public GameObject remy;
    public AudioSource audio;
    #endregion

    void Start()
    {
        Initialized();//��ʼ��
        if (PlayerPrefs.GetString("1->2start") == "true") scater = true;
    }

    void Update()
    {
        if (scater == true) Scater();
    }

    #region ��������
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetString("1->2start", "true");
    }
    public void LoadGame() { SceneManager.LoadScene(2); }
    #endregion
    private void Scater()//ͼƬ�ֿ�
    {
        L.Translate(-15f, 0, 0);
        R.Translate(15f, 0, 0);
        if (R.position.x > 3000)//�ֿ���һ������ֹͣ�ֿ�������
        {
            scater = false;
            PlayerPrefs.SetString("1->2start", "false");
        }
    }

    public void Rutern()//������ҳ
    {
        SceneManager.LoadScene(0);
    }

    public void Initialized()//��ʼ�����ش洢
    {
        PlayerPrefs.SetInt("curScene",-1);
        PlayerPrefs.SetString("���֮��", "false");
        PlayerPrefs.SetString("����֮��", "false");
        PlayerPrefs.SetString("����֮��", "false");
        PlayerPrefs.SetString("����֮��", "false");
    }

    #region ����չʾ
    public void Run() { remy.GetComponent<Animator>().SetTrigger("Run"); }
    public void Fly() { remy.GetComponent<Animator>().SetTrigger("Fly"); }
    public void Downslide() { remy.GetComponent<Animator>().SetTrigger("Downslide"); }
    public void Jump() { remy.GetComponent<Animator>().SetTrigger("Jump"); }
    public void Die() { remy.GetComponent<Animator>().SetTrigger("Die"); }
    #endregion

    #region ��ת
    public void Reset() { remy.transform.eulerAngles = new Vector3(0, 180f, 0); }
    public void Lrotate() { remy.transform.Rotate(0, 30f, 0); }
    public void Rrotate() { remy.transform.Rotate(0, -30f, 0); }
    #endregion

    #region ����
    public void mouseVoice()
    {
        audio.Play();
    }
    #endregion
}
