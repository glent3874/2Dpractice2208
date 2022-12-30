using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 橘色玩家
/// </summary>
public class OrangePlayer : MonoBehaviour
{
    #region 欄位
    [Header("物理系統")]
    public Rigidbody2D rb = null;
    [Header("移動速度")]
    public float moveSpeed = 5f;
    [Header("跳躍高度"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("是否在地板上")]
    public bool onGround = false;
    [Header("檢查地板區域:位移與半徑")]
    public Vector2 groundOffset;
    public Vector2 groundRadius;
    [Header("是否在牆壁滑行狀態")]
    public bool wallSliding = false;
    [Header("檢查牆壁區域:位移與半徑")]
    public Vector2 wallOffset;
    public Vector2 wallRadius;
    [Header("牆壁滑行速度")]
    public float wallSlidingSpeed;
    [Header("是否在牆壁跳躍狀態")]
    public bool wallJumping = false;
    [Header("牆壁跳躍的力道與時間")]
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    private float ad;
    #endregion

    #region 事件
    private void Update()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// 繪製偵測區域
    /// </summary>
    private void OnDrawGizmos()
    {
        //地板偵測繪製區域
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position + 
            transform.right * groundOffset.x + 
            transform.up * groundOffset.y,
            groundRadius);

        //牆壁偵測繪製區域
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position +
            transform.right * wallOffset.x +
            transform.up * wallOffset.y,
            wallRadius);
    }

    /// <summary>
    /// 碰撞器偵測
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collectable"))                     //如果碰撞的物件標籤是collectable
        {
            Destroy(collision.gameObject);                          //移除碰撞的物件
            GameoverController2.countAllCoins--;                    //金幣數量-1
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        ad = Input.GetAxis("Horizontal");                           //取得水平輸入值

        Vector2 move;
        move.x = ad * moveSpeed;                                    //水平軸移動為水平輸入值*移動速度
        move.y = rb.velocity.y;                                     //垂直軸不動
        rb.velocity = move;
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        //地板偵測碰撞區域
        Collider2D groundHit = Physics2D.OverlapBox(
            transform.position +
            transform.right * groundOffset.x +
            transform.up * groundOffset.y,
            groundRadius, 0, 1 << 6);

        if (groundHit) onGround = true;                                     //有撞到地板就開啟onground
        else onGround = false;                                              //沒有就關閉onground

        //牆壁偵測碰撞區域
        Collider2D wallHit = Physics2D.OverlapBox(
            transform.position +
            transform.right * wallOffset.x +
            transform.up * wallOffset.y,
            wallRadius, 0, 1 << 6);

        if (wallHit && !groundHit && Input.GetAxis("Horizontal") != 0)      //如果有碰到牆壁且沒有碰到地板且水平軸有輸入值就開啟wallsliding
            wallSliding = true;
        else wallSliding = false;                                           //沒有就關閉wallsliding

        if (wallSliding)                                                    //開始執行牆壁滑行物理
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        if(Input.GetKeyDown(KeyCode.UpArrow) &&　wallSliding)               //牆壁跳躍
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping) rb.velocity = new Vector2(xWallForce * -ad, yWallForce);

        //按下跳躍鍵跳躍
        if(Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        //放開跳躍鍵開始掉落
        if(Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
    #endregion
}
