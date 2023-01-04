using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���a
/// </summary>
public class Player : MonoBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("���D����"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("��q"),Range(0,200)]
    public int hp = 200;
    public HealthBar healthBar;
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
    [Header("���`�ƥ�")]
    public UnityEvent onDead;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator ani;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask attackMask;
    private float moveValue;                //������J��
    private bool isAttack;                  //������
    private float attackcooldowntimer;      //�����N�o�p�ɾ�
    #endregion

    #region �ƥ�
    private void Start()
    {
        healthBar.SetMaxHealth(hp);         //�]�w������̤j��
    }
    private void Update()
    {
        TurnDirection();                    //���ܪ��a��V
        Jump();                             //���D
        Attack();                           //����
    }
    /// <summary>
    /// ���z�t��update
    /// </summary>
    private void FixedUpdate()
    {
        Move();                             //����
    }
    /// <summary>
    /// ø�s�ϰ�
    /// </summary>
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
    /// ����
    /// </summary>
    /// <param name="horizontal">�����ƭ�</param>
    private void Move()
    {
        moveValue = Input.GetAxisRaw("Horizontal");                             //���o������J��
        rig.velocity = new Vector2(moveValue * moveSpeed, rig.velocity.y);      //����
        ani.SetBool("walk", moveValue != 0);                                    //������J�Ȥ���0�N����
    }
    /// <summary>
    /// ���a��V
    /// </summary>
    private void TurnDirection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))                               //���U�k�N���V�k
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))                            //���U���N���V��
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    /// <summary>
    /// ���D
    /// </summary>
    private void Jump()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + groundOffset, groundRadius, groundMask);      //���ߧP�w
        if (hit)                    //���b�a�O�W
        {
            onGround = true;
            jumpReload = false;
            landing = false;
        }
        else
        {
            onGround = false;
        }
        
        Collider2D landingHit = Physics2D.OverlapCircle(transform.position + landingOffset, landingRadius, groundMask);     //���a�P�w

        //���Ū��A
        if (!landingHit) 
            falling = true;         //���b����

        //���b�����ǳƸ��a
        if (landingHit && falling)  
        {
            falling = false;
            jump = false;
        }

        //���D���A�i�J�̰��I��
        if (jump && rig.velocity.y < 5f) 
            jumpReload = true;

        //���D���z
        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround) 
        {
            jump = true;
            rig.velocity = new Vector2(rig.velocity.x, jumpHeight);
        }

        //�������z
        if (Input.GetKeyUp(KeyCode.UpArrow) && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.5f);
        } 

        //���D�ʵe
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
        //�D���� �D���� �D���D ���U������ �~�i����
        if (!isAttack && Input.GetKeyDown("x") && moveValue == 0 && !jump)
        {
            isAttack = true;
            ani.SetTrigger("attack");

            Collider2D attackHit = Physics2D.OverlapBox(
                transform.position +
                transform.right * checkAttackOffset.x +
                transform.up * checkAttackOffset.y,
                checkAttackSize, 0, attackMask);

            if(attackHit)
                attackHit.GetComponent<GrayKnight>().Hurt((int)attack);     //�ǰe�ˮ`
        }

        //������
        if(isAttack)
        {
            //�����N�o�p�ɾ�
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
    public void Hurt(int damage)
    {
        hp -= damage;
        healthBar.SetHealth(hp);        //�]�w���
        if (hp <= 0) Dead();            //��q�k�s�ɦ��`
    }
    /// <summary>
    /// ���`
    /// </summary>
    private void Dead()
    {
        hp = 0;
        ani.SetBool("dead",true);
        GetComponent<CapsuleCollider2D>().enabled = false;      //�����I����
        rig.velocity = Vector3.zero;                            //�k�s����
        rig.constraints = RigidbodyConstraints2D.FreezeAll;     //��w���z�t��
        onDead.Invoke();                                        //���`�ƥ�
        enabled = false;                                        //�����}��
    }
    #endregion
}
