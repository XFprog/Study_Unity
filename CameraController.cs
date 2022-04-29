using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour//ThirdPersonView_Android
{
    public static CameraController my_camera;
    private float crot;

    public GameObject target;//目标物体
    float Xsensitivity = 0.5f;//视角X轴旋转灵敏度
    float Ysensitivity = 0.5f;//视角Y轴旋转灵敏度

    //将X轴旋转角度限制在-45度和80度之间，因计算机程序可以识别的角度为0~360，故将-45度设置为315度
    float Xrot_limit1 = 80;//旋转限制角度1
    float Xrot_limit2 = 315;//限制旋转角度2

    Vector3 rot;//手指位移向量

    Vector2 上帧位 = new Vector2(0, 0);
    Vector2 本帧位 = new Vector2(0, 0);

    float Xrot, Yrot;//视角两个轴的旋转值

    float distance = 8f;//Camera与target的距离，并且是旋转距离
    float dis1;//两个触点移动前的距离
    float dis2;//两个触点移动后的距离
    float dis3;//两个触点间距离的改变量
    float distance_sensitivity = 0.003f;//设置Camera距离的灵敏度
    float max_distance = 50f;//Camera最大距离
    float min_distance = 2f;//Camera最小距离

    // Use this for initialization
    void Start()
    {
        my_camera = this;//单例

        //如果Camera自身X轴的初始旋转角度大于80度并小于等于270度，则将其X轴旋转到80度位置
        if (transform.localEulerAngles.x > Xrot_limit1 && transform.localEulerAngles.x <= 270)
        {
            transform.Rotate(Xrot_limit1 - transform.localEulerAngles.x, 0, 0);
        }
        //如果Camera自身X轴的初始旋转角度大于270度并小于315度，则将其X轴旋转到315度位置，即-45度位置
        else if (transform.localEulerAngles.x < Xrot_limit2 && transform.localEulerAngles.x > 270)
        {
            transform.Rotate(Xrot_limit2 - transform.localEulerAngles.x, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //将Camera放在target的位置上等待旋转
        transform.position = target.transform.position + new Vector3(0, 3f, 0f);//其后用来调整相机初始位置,此处z务必要设为0,否则相机旋转出问题

        //视角旋转
        if (Input.GetMouseButton(0))//touch.phase == TouchPhase.Moved
        {
            本帧位 = Input.mousePosition;//获取本帧鼠标位置
            //Debug.Log("本帧位" + 本帧位);
            if (上帧位 == new Vector2(0, 0)) 上帧位 = 本帧位;//上帧无鼠标位置记录
                                                    //将触控点上一帧的位移赋给位移向量rot
            rot = 本帧位 - 上帧位;//touch.deltaPosition;

            //视角X轴将要旋转的值等于其现在X轴的旋转值加上负的触控点Y轴的位移量乘以灵敏度，因为我们需要手指上划时视角抬头，故此处是负的
            Xrot = transform.localEulerAngles.x - rot.y * Xsensitivity;
            //视角Y轴将要旋转的值等于其现在Y轴的旋转值加上触控点X轴的位移量乘以灵敏度
            Yrot = transform.localEulerAngles.y + rot.x * Ysensitivity;
            //当视角X轴将要旋转的值小于等于80度或者大于等于315度时（即为其处于-45度到80度的范围时）
            if (Xrot <= Xrot_limit1 || Xrot >= Xrot_limit2)
            {
                //把视角将要旋转的值化为四元数
                Quaternion aaaaa = Quaternion.Euler(Xrot, Yrot, 0);
                //将四元式赋值给Camera的rotation，完成旋转
                transform.rotation = aaaaa;
            }
            //当视角X轴将要旋转的值处于80度和315度之间时，保持视角现有的X轴旋转角度不变，只旋转其Y轴（此举是为了防止视角在X轴限制的极限位置时，斜向滑动手指视角Y轴不随着旋转）
            else if (Xrot > Xrot_limit1 && Xrot < Xrot_limit2)
            {
                //在视角处于X轴极限旋转位置时，视角X轴旋转值不变，仅Y轴旋转值改变，将其转化为四元数
                Quaternion aaaaa = Quaternion.Euler(transform.localEulerAngles.x, Yrot, 0);
                //将四元式赋值给Camera的rotation，完成旋转
                transform.rotation = aaaaa;
            }
            上帧位 = 本帧位;
            //Debug.Log("上帧位" + 上帧位);
        }
        else
        {
            上帧位 = new Vector2(0, 0);//鼠标不是移动状态
            本帧位 = new Vector2(0, 0);
        }
        //滚轮缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            dis3 = 1000f;
            //新的视角距离等于原有距离减去两个触点间距离的改变量与灵敏度乘积的差，此处为减去是因为我们希望两触点对向移动镜头拉远，两手指背向移动镜头拉近，若为加则效果相反
            distance -= dis3 * distance_sensitivity;
            //将视角距离限制在min_distance和max_distance的范围之内
            distance = Mathf.Clamp(distance, min_distance, max_distance);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            dis3 = -1000f;
            //新的视角距离等于原有距离减去两个触点间距离的改变量与灵敏度乘积的差，此处为减去是因为我们希望两触点对向移动镜头拉远，两手指背向移动镜头拉近，若为加则效果相反
            distance -= dis3 * distance_sensitivity;
            //将视角距离限制在min_distance和max_distance的范围之内
            distance = Mathf.Clamp(distance, min_distance, max_distance);
        }
        //camera平移，完成距离设置
        transform.Translate(Vector3.back * distance);
    }
}