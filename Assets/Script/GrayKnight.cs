using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 灰騎士
/// </summary>
public class GrayKnight : MonoBehaviour
{
    #region 欄位
    [Header("最大血量"), Range(0, 500)]
    public int maxHp = 100;
    [Header("目前血量")]
    public int currentHp;
    public HealthBar healthBar;
    [Header("攻擊力"), Range(0, 1000)]
    public int attack = 20;
    [Header("攻擊區域的位移大小")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;
    [Header("攻擊冷卻"), Range(0.5f, 5)]
    public float cdAttack = 3;
    private float timerAttack;
    [Header("攻擊延遲"), Range(0.1f, 3)]
    public float[] attackDelay;
    [Header("死亡事件")]
    public UnityEvent onDead;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator ani;
    [SerializeField] Player player;
    [SerializeField] CapsuleCollider2D CapsuleCollider2D;
    Collider2D hit;
    #endregion

    #region 事件
    private void Start()
    {
        currentHp = maxHp;              //初始化血量
        healthBar.SetMaxHealth(maxHp);  //填滿血條
    }

    private void Update()
    {
        CheckPlayerInAttackArea();      //檢查玩家是否進入攻擊區域
    }

    /// <summary>
    /// 繪製攻擊gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
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
    /// 受傷
    /// </summary>
    /// <param name="damage">傷害量</param>
    public void Hurt(int damage)
    {
        currentHp -= damage;                    //當前血量減掉傷害量
        healthBar.SetHealth(currentHp);         //設置血條
        if (timerAttack < (cdAttack - 0.5f))    //剛攻擊完可以進入受傷動畫
            ani.SetTrigger("hurt");
        if (currentHp <= 0) Dead();             //血量少於零死亡
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        currentHp = 0;                          //血量歸零
        ani.SetBool("dead", true);              //開啟死亡動畫
        CapsuleCollider2D.enabled = false;      //取消碰撞器
        onDead.Invoke();                        //啟動死亡事件
        enabled = false;                        //關閉此腳本
    }

    /// <summary>
    /// 檢查玩家是否進入攻擊範圍
    /// </summary>
    private void CheckPlayerInAttackArea()
    {
        //產生判定區域
        hit = Physics2D.OverlapBox(
            transform.position + 
            transform.right * checkAttackOffset.x + 
            transform.up * checkAttackOffset.y, 
            checkAttackSize, 0, 1 << 7);
        if (hit) Attack();                      //如果進入範圍就進入攻擊狀態
        else 
            timerAttack = cdAttack - 0.5f;      //離開範圍就重設攻擊冷卻計時器
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        if (timerAttack < cdAttack)                             //攻擊冷卻計時器
            timerAttack += Time.deltaTime;
        else                                                    //時間到就攻擊
        {
            timerAttack = 0;                                    //歸零計時器
            ani.SetTrigger("attack");                           //啟動攻擊動畫
            StartCoroutine(DelaySendDamageToPlayer());          //啟動傷害延遲協程
        }
    }
    
    /// <summary>
    /// 延遲傷害協程
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelaySendDamageToPlayer()
    {
        //延遲傷害的計算使其與動畫同步
        for (int i = 0; i < attackDelay.Length; i++)
        {
            yield return new WaitForSeconds(attackDelay[i]);    //在此等待設定的時間
            if (hit) player.Hurt(attack);                       //如果玩家還在攻擊範圍就計算傷害
        }
    }
    #endregion
}

