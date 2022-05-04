using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDealManager : MonoBehaviour
{
    #region �б�
    public List<Transform> RotateSteps = new List<Transform>();//��ת̨��
    public List<Transform> ScaleSteps = new List<Transform>();//����̨��
    public List<Transform> TranslateSteps = new List<Transform>();//λ��̨��
    #endregion

    #region ����
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
    private void Rotatedeal()//��ת
    {
        foreach (Transform step in RotateSteps)
        {
            step.Rotate(new Vector3(0, 1, 0), 1f);
        }
    }
    private void Scaledeal()//����
    {
        if (scaleValue <= 3 && scaleAdd == true)//����
        {
            scaleValue += Time.deltaTime;
            if (scaleValue > 3) scaleAdd = false;
        }
        else if(scaleValue>=1&&scaleAdd==false)//��С
        {
            scaleValue -= Time.deltaTime;
            if (scaleValue < 1) scaleAdd = true;
        }

        foreach (Transform step in ScaleSteps)//����
        {
            step.localScale = new Vector3(scaleValue,1,1);
        }
    }
    private void Translatedeal()//�ƶ�
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

        foreach (Transform step in TranslateSteps)//�ƶ�
        {
            step.Translate(0, transspeed, 0);
        }
    }
    #endregion
}
