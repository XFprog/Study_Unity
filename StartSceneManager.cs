using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    #region 参数
    private bool scater = false;
    #endregion

    #region 组件
    public Transform L;
    public Transform R;
    public GameObject remy;
    public AudioSource audio;
    #endregion

    void Start()
    {
        Initialized();//初始化
        if (PlayerPrefs.GetString("1->2start") == "true") scater = true;
    }

    void Update()
    {
        if (scater == true) Scater();
    }

    #region 场景加载
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetString("1->2start", "true");
    }
    public void LoadGame() { SceneManager.LoadScene(2); }
    #endregion
    private void Scater()//图片分开
    {
        L.Translate(-15f, 0, 0);
        R.Translate(15f, 0, 0);
        if (R.position.x > 3000)//分开到一定距离停止分开并重置
        {
            scater = false;
            PlayerPrefs.SetString("1->2start", "false");
        }
    }

    public void Rutern()//返回上页
    {
        SceneManager.LoadScene(0);
    }

    public void Initialized()//初始化本地存储
    {
        PlayerPrefs.SetInt("curScene",-1);
        PlayerPrefs.SetString("天空之城", "false");
        PlayerPrefs.SetString("炎泽之都", "false");
        PlayerPrefs.SetString("深蓝之海", "false");
        PlayerPrefs.SetString("绿茵之林", "false");
    }

    #region 动画展示
    public void Run() { remy.GetComponent<Animator>().SetTrigger("Run"); }
    public void Fly() { remy.GetComponent<Animator>().SetTrigger("Fly"); }
    public void Downslide() { remy.GetComponent<Animator>().SetTrigger("Downslide"); }
    public void Jump() { remy.GetComponent<Animator>().SetTrigger("Jump"); }
    public void Die() { remy.GetComponent<Animator>().SetTrigger("Die"); }
    #endregion

    #region 旋转
    public void Reset() { remy.transform.eulerAngles = new Vector3(0, 180f, 0); }
    public void Lrotate() { remy.transform.Rotate(0, 30f, 0); }
    public void Rrotate() { remy.transform.Rotate(0, -30f, 0); }
    #endregion

    #region 音乐
    public void mouseVoice()
    {
        audio.Play();
    }
    #endregion
}
