using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour//ThirdPersonView_Android
{
    public static CameraController my_camera;
    private float crot;

    public GameObject target;//Ŀ������
    float Xsensitivity = 0.5f;//�ӽ�X����ת������
    float Ysensitivity = 0.5f;//�ӽ�Y����ת������

    //��X����ת�Ƕ�������-45�Ⱥ�80��֮�䣬�������������ʶ��ĽǶ�Ϊ0~360���ʽ�-45������Ϊ315��
    float Xrot_limit1 = 80;//��ת���ƽǶ�1
    float Xrot_limit2 = 315;//������ת�Ƕ�2

    Vector3 rot;//��ָλ������

    Vector2 ��֡λ = new Vector2(0, 0);
    Vector2 ��֡λ = new Vector2(0, 0);

    float Xrot, Yrot;//�ӽ����������תֵ

    float distance = 8f;//Camera��target�ľ��룬��������ת����
    float dis1;//���������ƶ�ǰ�ľ���
    float dis2;//���������ƶ���ľ���
    float dis3;//������������ĸı���
    float distance_sensitivity = 0.003f;//����Camera�����������
    float max_distance = 50f;//Camera������
    float min_distance = 2f;//Camera��С����

    // Use this for initialization
    void Start()
    {
        my_camera = this;//����

        //���Camera����X��ĳ�ʼ��ת�Ƕȴ���80�Ȳ�С�ڵ���270�ȣ�����X����ת��80��λ��
        if (transform.localEulerAngles.x > Xrot_limit1 && transform.localEulerAngles.x <= 270)
        {
            transform.Rotate(Xrot_limit1 - transform.localEulerAngles.x, 0, 0);
        }
        //���Camera����X��ĳ�ʼ��ת�Ƕȴ���270�Ȳ�С��315�ȣ�����X����ת��315��λ�ã���-45��λ��
        else if (transform.localEulerAngles.x < Xrot_limit2 && transform.localEulerAngles.x > 270)
        {
            transform.Rotate(Xrot_limit2 - transform.localEulerAngles.x, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //��Camera����target��λ���ϵȴ���ת
        transform.position = target.transform.position + new Vector3(0, 3f, 0f);//����������������ʼλ��,�˴�z���Ҫ��Ϊ0,���������ת������

        //�ӽ���ת
        if (Input.GetMouseButton(0))//touch.phase == TouchPhase.Moved
        {
            ��֡λ = Input.mousePosition;//��ȡ��֡���λ��
            //Debug.Log("��֡λ" + ��֡λ);
            if (��֡λ == new Vector2(0, 0)) ��֡λ = ��֡λ;//��֡�����λ�ü�¼
                                                    //�����ص���һ֡��λ�Ƹ���λ������rot
            rot = ��֡λ - ��֡λ;//touch.deltaPosition;

            //�ӽ�X�ὫҪ��ת��ֵ����������X�����תֵ���ϸ��Ĵ��ص�Y���λ�������������ȣ���Ϊ������Ҫ��ָ�ϻ�ʱ�ӽ�̧ͷ���ʴ˴��Ǹ���
            Xrot = transform.localEulerAngles.x - rot.y * Xsensitivity;
            //�ӽ�Y�ὫҪ��ת��ֵ����������Y�����תֵ���ϴ��ص�X���λ��������������
            Yrot = transform.localEulerAngles.y + rot.x * Ysensitivity;
            //���ӽ�X�ὫҪ��ת��ֵС�ڵ���80�Ȼ��ߴ��ڵ���315��ʱ����Ϊ�䴦��-45�ȵ�80�ȵķ�Χʱ��
            if (Xrot <= Xrot_limit1 || Xrot >= Xrot_limit2)
            {
                //���ӽǽ�Ҫ��ת��ֵ��Ϊ��Ԫ��
                Quaternion aaaaa = Quaternion.Euler(Xrot, Yrot, 0);
                //����Ԫʽ��ֵ��Camera��rotation�������ת
                transform.rotation = aaaaa;
            }
            //���ӽ�X�ὫҪ��ת��ֵ����80�Ⱥ�315��֮��ʱ�������ӽ����е�X����ת�ǶȲ��䣬ֻ��ת��Y�ᣨ�˾���Ϊ�˷�ֹ�ӽ���X�����Ƶļ���λ��ʱ��б�򻬶���ָ�ӽ�Y�᲻������ת��
            else if (Xrot > Xrot_limit1 && Xrot < Xrot_limit2)
            {
                //���ӽǴ���X�Ἣ����תλ��ʱ���ӽ�X����תֵ���䣬��Y����תֵ�ı䣬����ת��Ϊ��Ԫ��
                Quaternion aaaaa = Quaternion.Euler(transform.localEulerAngles.x, Yrot, 0);
                //����Ԫʽ��ֵ��Camera��rotation�������ת
                transform.rotation = aaaaa;
            }
            ��֡λ = ��֡λ;
            //Debug.Log("��֡λ" + ��֡λ);
        }
        else
        {
            ��֡λ = new Vector2(0, 0);//��겻���ƶ�״̬
            ��֡λ = new Vector2(0, 0);
        }
        //��������
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            dis3 = 1000f;
            //�µ��ӽǾ������ԭ�о����ȥ������������ĸı����������ȳ˻��Ĳ�˴�Ϊ��ȥ����Ϊ����ϣ������������ƶ���ͷ��Զ������ָ�����ƶ���ͷ��������Ϊ����Ч���෴
            distance -= dis3 * distance_sensitivity;
            //���ӽǾ���������min_distance��max_distance�ķ�Χ֮��
            distance = Mathf.Clamp(distance, min_distance, max_distance);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            dis3 = -1000f;
            //�µ��ӽǾ������ԭ�о����ȥ������������ĸı����������ȳ˻��Ĳ�˴�Ϊ��ȥ����Ϊ����ϣ������������ƶ���ͷ��Զ������ָ�����ƶ���ͷ��������Ϊ����Ч���෴
            distance -= dis3 * distance_sensitivity;
            //���ӽǾ���������min_distance��max_distance�ķ�Χ֮��
            distance = Mathf.Clamp(distance, min_distance, max_distance);
        }
        //cameraƽ�ƣ���ɾ�������
        transform.Translate(Vector3.back * distance);
    }
}