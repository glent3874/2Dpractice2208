using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayKnight : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("跳躍高度"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("最大血量"), Range(0, 500)]
    public int hp = 100;
    [Header("目前血量")]
    public int currentHealth;
    public HealthBar healthBar;
    [Header("是否在地板上")]
    public bool onGround = false;
    [Header("Jump")]
    public bool jump = false;
    [Header("Jump Reload")]
    public bool jumpReload = false;
    [Header("Falling")]
    public bool falling = false;
    [Header("Landing")]
    public bool landing = false;
    [Header("檢查落地區域:位移與半徑")]
    public Vector3 landingOffset;
    [Range(0, 2)]
    public float landingRadius = 0.5f;
    [Header("檢查地板區域:位移與半徑")]
    public Vector3 groundOffset;
    [Range(0, 2)]
    public float groundRadius = 0.5f;
    [Header("攻擊區域的位移大小")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;
    [Header("攻擊冷卻"), Range(0.5f, 5)]
    public float cdAttack = 3;
    private float timerAttack;

    // 將私人欄位顯示在屬性面板上
    [SerializeField]
    private StateEnemy state;

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    /// <summary>
    /// 水平輸入值
    /// </summary>
    private float moveValue;
    #endregion

    #region 事件
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
        //繪製地板判定區域
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + groundOffset, groundRadius);
        //繪製落地判定區域
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + landingOffset, landingRadius);
        //繪製攻擊判定區域
        Gizmos.color = new Color(0.5f, 0.3f, 0.1f, 0.3f);
        Gizmos.DrawCube(
            transform.position +
            transform.right * checkAttackOffset.x +
            transform.up * checkAttackOffset.y,
            checkAttackSize);
    }
    #endregion

    #region 方法
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
        //落地判定
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

        //跳躍物理
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
        //動畫
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
        print("攻擊");
    }
    #endregion
}
//定義列舉
// 1. 使用關鍵字 enum 定義列舉以及包含的選項, 可以在類別外定義
// 2. 需要有一個欄位定義為此列舉類型
// 語法: 修飾詞 enum 列舉名稱{選項1, 選項2, ....., 選項N}
enum StateEnemy
{
    idle, walk, track, attack, dead, hurt
}
