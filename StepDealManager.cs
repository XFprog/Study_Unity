using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDealManager : MonoBehaviour
{
    #region 列表
    public List<Transform> RotateSteps = new List<Transform>();//旋转台阶
    public List<Transform> ScaleSteps = new List<Transform>();//缩放台阶
    public List<Transform> TranslateSteps = new List<Transform>();//位移台阶
    #endregion

    #region 变量
    private float scaleValue = 1;
    private bool scaleAdd = true;
    private float transValue = 0;
    private float transspeed = 0;
    private int transDec = -1;
    #endregion

    void Start()
    {
        
    }
    void Update()
    {
        Rotatedeal();
        Scaledeal();
        Translatedeal();
    }

    #region dealSteps
    private void Rotatedeal()//旋转
    {
        foreach (Transform step in RotateSteps)
        {
            step.Rotate(new Vector3(0, 1, 0), 1f);
        }
    }
    private void Scaledeal()//缩放
    {
        if (scaleValue <= 3 && scaleAdd == true)//扩大
        {
            scaleValue += Time.deltaTime;
            if (scaleValue > 3) scaleAdd = false;
        }
        else if(scaleValue>=1&&scaleAdd==false)//缩小
        {
            scaleValue -= Time.deltaTime;
            if (scaleValue < 1) scaleAdd = true;
        }

        foreach (Transform step in ScaleSteps)//缩放
        {
            step.localScale = new Vector3(scaleValue,1,1);
        }
    }
    private void Translatedeal()//移动
    {
        transspeed = transDec*Time.deltaTime*3;
        transValue += transspeed;
        if (transValue < -30)
        {
            transValue = -30;
            transDec = -transDec;
        }
        else if (transValue > 0)
        {
            transValue = 0;
            transDec = -transDec;
        }

        foreach (Transform step in TranslateSteps)//移动
        {
            step.Translate(0, transspeed, 0);
        }
    }
    #endregion
}
