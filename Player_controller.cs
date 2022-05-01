using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 玩家状态
/// </summary>
public enum PlayerState
{
    Normal,
    Attack
}


public class Player_controller : MonoBehaviour
{
    /// <summary>
    /// 组件
    /// </summary>
    public CharacterController characterController;
    public Animator animator;
    public AudioSource audiosource;

    /// <summary>
    /// 武器列表
    /// </summary>
    //public Weapon_list[] weapon;

    private PlayerState playerState;//实例化枚举

    /// <summary>
    /// 参数
    /// </summary>
    public float MoveSpeed = 5f;//移动速度
    public float JumpForce = 30f;//跳跃力度
    public float RotateSpeed = 10f;//旋转速度

    private bool jump_start = false;
    private float jump_value = 0;
    private float 减速模拟值 = 1;

    private int flystate = 0;
    private float flyspaceValue = 0f;
    private bool flyspaceStart = false;
    private int flyspaceflag = 1;

    private bool downslideStart = false;
    private float downslideTime = 0f;

    private bool dieStart = false;

    public PlayerState PlayerState
    {
        get => playerState;
        private set
        {
            playerState = value;
            //当状态切换时,自动处理一些逻辑
            //逻辑层面的状态切换
            switch (playerState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Attack:
                    break;
            }
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        //playerName.text = PlayerPrefs.GetString("player_name", "default");//提取并赋予玩家名
        //Weapon_list[0].Init(audiosource, animator);
    }

    void Update()
    {
        StateOnUpdate();
    }

    private void StateOnUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                Move();
                Jump();
                Downslide();
                Fly();
                Die();
                break;
            case PlayerState.Attack:
                Attack();
                break;
        }
    }

    private void Move()
    {
        //获取玩家输入
        float h = Input.GetAxis("Horizontal");//Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");//Input.GetAxis("Vertical");

        //比较h,v绝对值大小,采取其中最大的考虑移动
        float move = Mathf.Abs(h) > Mathf.Abs(v) ? Mathf.Abs(h) : Mathf.Abs(v);

        //玩家向相机方向移动
        Vector3 targetDirection = new Vector3(h, 0, v);
        float y = CameraController.my_camera.transform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        //

        //移动触发
        animator.SetFloat("Move", move * 2);
        if (flystate == 0)
        {
            characterController.SimpleMove(targetDirection.normalized * MoveSpeed);//调用SimpMove时才受重力影响,且其赋予y轴值无效
            if (downslideStart == true)//下滑
            {
                downslideTime += Time.deltaTime;
                if (downslideTime > 0 && downslideTime <= 0.2)
                    characterController.Move((Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * new Vector3(0, 0, 1)).normalized * MoveSpeed/2);
                else if (downslideTime > 0.2)
                {
                    downslideStart = false;
                    downslideTime = 0f;
                }
            }
        }
        else characterController.Move(targetDirection.normalized * MoveSpeed / 7);//飞行调用Move不受重力影响

        //旋转
        if (h!=0 || v!= 0)
        {
            //目标旋转角
            Quaternion targetDirQuaternion = Quaternion.LookRotation(targetDirection);
            //平滑过渡当前角度到目标旋转角度
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * RotateSpeed);
        }

        //跑步声
        if (h != 0 || v != 0)
        {
            if (!audiosource.isPlaying)
                audiosource.Play();
            if (GetComponents<AudioSource>()[1].isPlaying&& GetComponents<AudioSource>()[2].isPlaying)
            {
                audiosource.Stop();//奔跑音效失效
            }
        }
        else audiosource.Stop();
    }

    #region 动画状态
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&flystate==0)//陆地状态空格
        {
            jump_start = true;
            animator.SetBool("Jump", true);
            jump_value = 0;
            GetComponents<AudioSource>()[1].Play();//跳跃音效
        }
        else if(Input.GetKey(KeyCode.Space)&&flystate==1)//飞行状态空格
        {
            characterController.Move(new Vector3(0, JumpForce/25, 0));
        }

        if(Input.GetKeyDown(KeyCode.Space)&&flystate==1)
        {
            flyspaceStart = true;
            flyspaceflag *= -1;
        }
        if(flyspaceStart==true)//累加飞行时按空格的间隔时间，若小于0.5秒再按空格则下降
        {
            flyspaceValue += Time.deltaTime;
            //Debug.Log("flyspaceValue:" + flyspaceValue);
            if(flyspaceValue<=0.5f&&flyspaceflag==1)//小于0.5秒且按空格则下降
            {
                //Debug.Log("下降");
                characterController.Move(new Vector3(0, -JumpForce/3, 0));
                flyspaceValue = 0f;
                flyspaceStart = false;
            }
            if(flyspaceValue>0.5f)//大于0.5秒自动退出飞行间隔记时状态
            {
                //Debug.Log("退出飞行间隔记时状态");
                flyspaceValue = 0f;
                flyspaceStart = false;
                flyspaceflag = 1;
            }
        }

        if (jump_start && flystate == 0)
        {
            //Debug.Log("跳跃中");
            jump_value += Time.deltaTime;
            if (jump_value >= 0.24f)
            {
                减速模拟值 -= Time.deltaTime * 5;
                characterController.Move(new Vector3(0, 2f * JumpForce * Time.deltaTime * 减速模拟值, 0));
            }
            if (jump_value >= 0.4f)
            {
                //Debug.Log("跳跃结束");
                animator.SetBool("Jump", false);
                jump_start = false;
                jump_value = 0;
                减速模拟值 = 1f;
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                //{
                //    animator.SetTrigger("JumpOver");
                //}
            }
        }
    }
    public void Downslide()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Downslide");
            downslideStart = true;
            GetComponents<AudioSource>()[2].PlayOneShot(GetComponents<AudioSource>()[2].clip);//下滑音效
        }
    }
    public void Fly()
    {
        if (Input.GetKeyDown(KeyCode.Q)&&flystate==0)//不是飞行状态时
        {
            animator.SetBool("Fly",true);
            flystate = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Q)&&flystate==1)//飞行状态时
        {
            animator.SetBool("Fly", false);
            flystate = 0;
        }
    }
    public void Die()
    {
        animator.SetBool("Die", true);
        if (transform.position.y <=-100&&dieStart==false)
        {
            animator.SetTrigger("die");
            animator.SetBool("Die", true);
            dieStart = true;
        }
        else if(transform.position.y>-100)
        {
            animator.SetBool("Die", false);
            dieStart = false;
        }
        if (transform.position.y <= -300)
        {
            transform.position = new Vector3(0, 1, 0);//复活
        }
    }
    #endregion
    private void Attack()
    {

    }

    #region 攻击相关

    #endregion

    #region 动画事件
    private void JumpOver()
    {
        animator.SetTrigger("JumpOver");
    }
    #endregion
}
