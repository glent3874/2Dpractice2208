using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 15)]
    public float moveSpeed = 10.5f;
    [Header("跳躍高度"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("血量"),Range(0,20)]
    public float hp = 5;
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

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    /// <summary>
    /// 玩家水平輸入值
    /// </summary>
    private float moveValue;
    private bool isAttack;
    private float attackcooldowntimer;
    #endregion

    #region 事件
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
    /// 取得玩家輸入水平軸向值: A D 左 右
    /// </summary>
    private void GetPlayerInputHorizontal()
    {
        //右 = 1 左 = -1 放開 = 0
        moveValue = Input.GetAxisRaw("Horizontal");
    }
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="horizontal">水平數值</param>
    private void Move(float horizontal)
    {
        rig.velocity = new Vector2(horizontal * moveSpeed, rig.velocity.y);
        
        ani.SetBool("walk", horizontal != 0);
    }
    /// <summary>
    /// 旋轉方向
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
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        //站立判定
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
    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        //Idle 狀態才可以攻擊
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
    /// 受傷
    /// </summary>
    /// <param name="damage">造成的傷害</param>
    public void Hurt(float damage)
    {

    }
    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {

    }
    /// <summary>
    /// 吃道具
    /// </summary>
    /// <param name="propName">獲得的道具名稱</param>
    private void EatProp(string propName)
    {

    }
    #endregion
}
