using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayKnight : MonoBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("���D����"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("�̤j��q"), Range(0, 500)]
    public int hp = 100;
    [Header("�ثe��q")]
    public int currentHealth;
    public HealthBar healthBar;
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
    [Header("�����ϰ쪺�첾�j�p")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;
    [Header("�����N�o"), Range(0.5f, 5)]
    public float cdAttack = 3;
    private float timerAttack;

    // �N�p�H�����ܦb�ݩʭ��O�W
    [SerializeField]
    private StateEnemy state;

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    /// <summary>
    /// ������J��
    /// </summary>
    private float moveValue;
    #endregion

    #region �ƥ�
    private void Start()
    {
        currentHealth = hp;
        healthBar.SetMaxHealth(hp);
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CheckState();
        CheckPlayerInAttackArea();
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
    private void GetHorizontal()
    {

    }
    private void Move(float horizontal)
    {
        rig.velocity = new Vector2(horizontal * moveSpeed, rig.velocity.y);

        ani.SetBool("walk", horizontal != 0);
    }
    private void TurnDirection()
    {

    }
    private void Jump()
    {
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
    private void CheckState()
    {
        switch (state)
        {
            case StateEnemy.idle:
                break;
            case StateEnemy.walk:
                break;
            case StateEnemy.track:
                break;
            case StateEnemy.attack:
                Attack();
                break;
            case StateEnemy.dead:
                break;
            case StateEnemy.hurt:
                break;
            default:
                break;
        }
    }
    public void Hurt(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        ani.SetTrigger("hurt");
        if (currentHealth <= 0) Dead();
    }
    private void Dead()
    {
        currentHealth = 0;
        ani.SetBool("dead", true);
        state = StateEnemy.dead;
        GetComponent<CapsuleCollider2D>().enabled = false;
        rig.velocity = Vector3.zero;
        rig.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
    }
    private void CheckPlayerInAttackArea()
    {
        Collider2D hit = Physics2D.OverlapBox(
            transform.position +
            transform.right * checkAttackOffset.x +
            transform.up * checkAttackOffset.y,
            checkAttackSize, 0, 1 << 7);
        if (hit) state = StateEnemy.attack;
    }
    private void Attack()
    {
        if(timerAttack < cdAttack)
        {
            timerAttack += Time.deltaTime;
        }
        else
        {
            AttackMethod();
        }
    }
    private void AttackMethod()
    {
        timerAttack = 0;
        ani.SetTrigger("attack");
        print("����");
    }
    #endregion
}
//�w�q�C�|
// 1. �ϥ�����r enum �w�q�C�|�H�Υ]�t���ﶵ, �i�H�b���O�~�w�q
// 2. �ݭn���@�����w�q�����C�|����
// �y�k: �׹��� enum �C�|�W��{�ﶵ1, �ﶵ2, ....., �ﶵN}
enum StateEnemy
{
    idle, walk, track, attack, dead, hurt
}
