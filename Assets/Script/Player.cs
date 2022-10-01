using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("���D����"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("��q"),Range(0,20)]
    public float hp = 5;
    [Header("�����O"), Range(0, 1000)]
    public float attack = 20;
    [Header("�O�_�b�a�O�W")]
    public bool onGround = false;
    [Header("Jump")]
    public bool jump = false;
    [Header("Jump Reload")]
    public bool jumpReload = false;
    [Header("Falling")]
    public bool falling = false;
    [Header("Landing")]
    public bool landing = false;
    [Header("�ˬd���a�ϰ�:�첾�P�b�|")]
    public Vector3 landingOffset;
    [Range(0, 2)]
    public float landingRadius = 0.5f;
    [Header("�ˬd�a�O�ϰ�:�첾�P�b�|")]
    public Vector3 groundOffset;
    [Range(0, 2)]
    public float groundRadius = 0.5f;
    [Header("�����N�o"), Range(0, 5)]
    public float cd = 2;
    [Header("�����ϰ쪺�첾�P�j�p")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    /// <summary>
    /// ���a������J��
    /// </summary>
    private float moveValue;
    private bool isAttack;
    private float attackcooldowntimer;
    #endregion

    #region �ƥ�
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        GetPlayerInputHorizontal();
        TurnDirection();
        Jump();
        JumpAnimation();
        Attack();
    }
    private void FixedUpdate()
    {
        Move(moveValue);
    }
    private void OnDrawGizmos()
    {
        //ø�s�a�O�P�w�ϰ�
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + groundOffset, groundRadius);
        //ø�s���a�P�w�ϰ�
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + landingOffset, landingRadius);

        //ø�s�����P�w�ϰ�
        Gizmos.color = new Color(0.5f, 0.3f, 0.1f, 0.3f);
        Gizmos.DrawCube(
            transform.position +
            transform.right * checkAttackOffset.x +
            transform.up * checkAttackOffset.y,
            checkAttackSize);
    }
    #endregion

    #region ��k
    /// <summary>
    /// ���o���a��J�����b�V��: A D �� �k
    /// </summary>
    private void GetPlayerInputHorizontal()
    {
        //�k = 1 �� = -1 ��} = 0
        moveValue = Input.GetAxisRaw("Horizontal");
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="horizontal">�����ƭ�</param>
    private void Move(float horizontal)
    {
        rig.velocity = new Vector2(horizontal * moveSpeed, rig.velocity.y);
        
        ani.SetBool("walk", horizontal != 0);
    }
    /// <summary>
    /// �����V
    /// </summary>
    private void TurnDirection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    /// <summary>
    /// ���D
    /// </summary>
    private void Jump()
    {
        //���ߧP�w
        Collider2D hit = Physics2D.OverlapCircle(transform.position + groundOffset, groundRadius, 1 << 6);
        if (hit)
        {
            onGround = true;
            jumpReload = false;
            landing = false;
        }
        else
        {
            onGround = false;
        }
        //���a�P�w
        Collider2D landingHit = Physics2D.OverlapCircle(transform.position + landingOffset, landingRadius, 1 << 6);
        if (!landingHit)
        {
            falling = true;
        }
        if (landingHit && falling)
        {
            falling = false;
            jump = false;
        }
        if (jump && rig.velocity.y < 5f) jumpReload = true;

        //���D���z
        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround) 
        {
            jump = true;
            rig.velocity = new Vector2(rig.velocity.x, jumpHeight);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.5f);
        }
    }
    private void JumpAnimation()
    {
        //�ʵe
        ani.SetBool("jump", jump);
        ani.SetBool("jump reload", jumpReload);
        ani.SetBool("falling", falling);
        ani.SetBool("landing", !falling);
    }
    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        //Idle ���A�~�i�H����
        if (!isAttack && Input.GetKeyDown("x") && moveValue == 0 && !jump)
        {
            isAttack = true;
            ani.SetTrigger("attack");

            //
            Collider2D attackhit = Physics2D.OverlapBox(
                transform.position +
                transform.right * checkAttackOffset.x +
                transform.up * checkAttackOffset.y,
                checkAttackSize, 0, 1 << 8);

            if(attackhit)
            {
                attackhit.GetComponent<GrayKnight>().Hurt((int)attack);
            }
        }

        if(isAttack)
        {
            if(attackcooldowntimer < cd)
            {
                attackcooldowntimer += Time.deltaTime;
            }
            else
            {
                attackcooldowntimer = 0;
                isAttack = false;
            }
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage">�y�����ˮ`</param>
    public void Hurt(float damage)
    {

    }
    /// <summary>
    /// ���`
    /// </summary>
    private void Dead()
    {

    }
    /// <summary>
    /// �Y�D��
    /// </summary>
    /// <param name="propName">��o���D��W��</param>
    private void EatProp(string propName)
    {

    }
    #endregion
}
