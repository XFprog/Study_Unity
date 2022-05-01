using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���״̬
/// </summary>
public enum PlayerState
{
    Normal,
    Attack
}


public class Player_controller : MonoBehaviour
{
    /// <summary>
    /// ���
    /// </summary>
    public CharacterController characterController;
    public Animator animator;
    public AudioSource audiosource;

    /// <summary>
    /// �����б�
    /// </summary>
    //public Weapon_list[] weapon;

    private PlayerState playerState;//ʵ����ö��

    /// <summary>
    /// ����
    /// </summary>
    public float MoveSpeed = 5f;//�ƶ��ٶ�
    public float JumpForce = 30f;//��Ծ����
    public float RotateSpeed = 10f;//��ת�ٶ�

    private bool jump_start = false;
    private float jump_value = 0;
    private float ����ģ��ֵ = 1;

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
            //��״̬�л�ʱ,�Զ�����һЩ�߼�
            //�߼������״̬�л�
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
        //playerName.text = PlayerPrefs.GetString("player_name", "default");//��ȡ�����������
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
        //��ȡ�������
        float h = Input.GetAxis("Horizontal");//Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");//Input.GetAxis("Vertical");

        //�Ƚ�h,v����ֵ��С,��ȡ�������Ŀ����ƶ�
        float move = Mathf.Abs(h) > Mathf.Abs(v) ? Mathf.Abs(h) : Mathf.Abs(v);

        //�������������ƶ�
        Vector3 targetDirection = new Vector3(h, 0, v);
        float y = CameraController.my_camera.transform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        //

        //�ƶ�����
        animator.SetFloat("Move", move * 2);
        if (flystate == 0)
        {
            characterController.SimpleMove(targetDirection.normalized * MoveSpeed);//����SimpMoveʱ��������Ӱ��,���丳��y��ֵ��Ч
            if (downslideStart == true)//�»�
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
        else characterController.Move(targetDirection.normalized * MoveSpeed / 7);//���е���Move��������Ӱ��

        //��ת
        if (h!=0 || v!= 0)
        {
            //Ŀ����ת��
            Quaternion targetDirQuaternion = Quaternion.LookRotation(targetDirection);
            //ƽ�����ɵ�ǰ�Ƕȵ�Ŀ����ת�Ƕ�
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * RotateSpeed);
        }

        //�ܲ���
        if (h != 0 || v != 0)
        {
            if (!audiosource.isPlaying)
                audiosource.Play();
            if (GetComponents<AudioSource>()[1].isPlaying&& GetComponents<AudioSource>()[2].isPlaying)
            {
                audiosource.Stop();//������ЧʧЧ
            }
        }
        else audiosource.Stop();
    }

    #region ����״̬
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&flystate==0)//½��״̬�ո�
        {
            jump_start = true;
            animator.SetBool("Jump", true);
            jump_value = 0;
            GetComponents<AudioSource>()[1].Play();//��Ծ��Ч
        }
        else if(Input.GetKey(KeyCode.Space)&&flystate==1)//����״̬�ո�
        {
            characterController.Move(new Vector3(0, JumpForce/25, 0));
        }

        if(Input.GetKeyDown(KeyCode.Space)&&flystate==1)
        {
            flyspaceStart = true;
            flyspaceflag *= -1;
        }
        if(flyspaceStart==true)//�ۼӷ���ʱ���ո�ļ��ʱ�䣬��С��0.5���ٰ��ո����½�
        {
            flyspaceValue += Time.deltaTime;
            //Debug.Log("flyspaceValue:" + flyspaceValue);
            if(flyspaceValue<=0.5f&&flyspaceflag==1)//С��0.5���Ұ��ո����½�
            {
                //Debug.Log("�½�");
                characterController.Move(new Vector3(0, -JumpForce/3, 0));
                flyspaceValue = 0f;
                flyspaceStart = false;
            }
            if(flyspaceValue>0.5f)//����0.5���Զ��˳����м����ʱ״̬
            {
                //Debug.Log("�˳����м����ʱ״̬");
                flyspaceValue = 0f;
                flyspaceStart = false;
                flyspaceflag = 1;
            }
        }

        if (jump_start && flystate == 0)
        {
            //Debug.Log("��Ծ��");
            jump_value += Time.deltaTime;
            if (jump_value >= 0.24f)
            {
                ����ģ��ֵ -= Time.deltaTime * 5;
                characterController.Move(new Vector3(0, 2f * JumpForce * Time.deltaTime * ����ģ��ֵ, 0));
            }
            if (jump_value >= 0.4f)
            {
                //Debug.Log("��Ծ����");
                animator.SetBool("Jump", false);
                jump_start = false;
                jump_value = 0;
                ����ģ��ֵ = 1f;
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
            GetComponents<AudioSource>()[2].PlayOneShot(GetComponents<AudioSource>()[2].clip);//�»���Ч
        }
    }
    public void Fly()
    {
        if (Input.GetKeyDown(KeyCode.Q)&&flystate==0)//���Ƿ���״̬ʱ
        {
            animator.SetBool("Fly",true);
            flystate = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Q)&&flystate==1)//����״̬ʱ
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
            transform.position = new Vector3(0, 1, 0);//����
        }
    }
    #endregion
    private void Attack()
    {

    }

    #region �������

    #endregion

    #region �����¼�
    private void JumpOver()
    {
        animator.SetTrigger("JumpOver");
    }
    #endregion
}
