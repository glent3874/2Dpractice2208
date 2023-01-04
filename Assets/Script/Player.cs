using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 玩家
/// </summary>
public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("跳躍高度"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("血量"),Range(0,200)]
    public int hp = 200;
    public HealthBar healthBar;
    [Header("攻擊力"), Range(0, 1000)]
    public float attack = 20;
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
    [Header("攻擊冷卻"), Range(0, 5)]
    public float cd = 2;
    [Header("攻擊區域的位移與大小")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;
    [Header("死亡事件")]
    public UnityEvent onDead;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator ani;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask attackMask;
    private float moveValue;                //水平輸入值
    private bool isAttack;                  //攻擊中
    private float attackcooldowntimer;      //攻擊冷卻計時器
    #endregion

    #region 事件
    private void Start()
    {
        healthBar.SetMaxHealth(hp);         //設定血條為最大值
    }
    private void Update()
    {
        TurnDirection();                    //改變玩家方向
        Jump();                             //跳躍
        Attack();                           //攻擊
    }
    /// <summary>
    /// 物理系統update
    /// </summary>
    private void FixedUpdate()
    {
        Move();                             //移動
    }
    /// <summary>
    /// 繪製區域
    /// </summary>
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
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="horizontal">水平數值</param>
    private void Move()
    {
        moveValue = Input.GetAxisRaw("Horizontal");                             //取得水平輸入值
        rig.velocity = new Vector2(moveValue * moveSpeed, rig.velocity.y);      //移動
        ani.SetBool("walk", moveValue != 0);                                    //水平輸入值不為0就移動
    }
    /// <summary>
    /// 玩家方向
    /// </summary>
    private void TurnDirection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))                               //按下右就面向右
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))                            //按下左就面向左
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + groundOffset, groundRadius, groundMask);      //站立判定
        if (hit)                    //站在地板上
        {
            onGround = true;
            jumpReload = false;
            landing = false;
        }
        else
        {
            onGround = false;
        }
        
        Collider2D landingHit = Physics2D.OverlapCircle(transform.position + landingOffset, landingRadius, groundMask);     //落地判定

        //滯空狀態
        if (!landingHit) 
            falling = true;         //正在掉落

        //正在掉落準備落地
        if (landingHit && falling)  
        {
            falling = false;
            jump = false;
        }

        //跳躍狀態進入最高點時
        if (jump && rig.velocity.y < 5f) 
            jumpReload = true;

        //跳躍物理
        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround) 
        {
            jump = true;
            rig.velocity = new Vector2(rig.velocity.x, jumpHeight);
        }

        //掉落物理
        if (Input.GetKeyUp(KeyCode.UpArrow) && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.5f);
        } 

        //跳躍動畫
        ani.SetBool("jump", jump);
        ani.SetBool("jump reload", jumpReload);
        ani.SetBool("falling", falling);
        ani.SetBool("landing", !falling);
    }
    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        //非攻擊 非移動 非跳躍 按下攻擊鍵 才可攻擊
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
                attackHit.GetComponent<GrayKnight>().Hurt((int)attack);     //傳送傷害
        }

        //攻擊後
        if(isAttack)
        {
            //攻擊冷卻計時器
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
    /// 受傷
    /// </summary>
    /// <param name="damage">造成的傷害</param>
    public void Hurt(int damage)
    {
        hp -= damage;
        healthBar.SetHealth(hp);        //設定血條
        if (hp <= 0) Dead();            //血量歸零時死亡
    }
    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        hp = 0;
        ani.SetBool("dead",true);
        GetComponent<CapsuleCollider2D>().enabled = false;      //關閉碰撞器
        rig.velocity = Vector3.zero;                            //歸零移動
        rig.constraints = RigidbodyConstraints2D.FreezeAll;     //鎖定物理系統
        onDead.Invoke();                                        //死亡事件
        enabled = false;                                        //關閉腳本
    }
    #endregion
}
