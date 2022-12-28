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
    public int hp = 100;
    [Header("目前血量")]
    public int currentHealth;
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

    // 將私人欄位顯示在屬性面板上
    [SerializeField]
    private StateEnemy state;

    private Rigidbody2D rig;
    private Animator ani;
    private Player player;
    Collider2D hit;
    #endregion

    #region 事件
    private void Start()
    {
        #region 初始化數值
        currentHealth = hp;
        healthBar.SetMaxHealth(hp);
        #endregion

        #region 取得元件與玩家類別
        rig = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player").GetComponent<Player>();
        #endregion
    }
    private void Update()
    {
        CheckState();
        CheckPlayerInAttackArea();
    }
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
    
    private void CheckState()
    {
        switch (state)
        {
            case StateEnemy.attack:
                Attack();
                break;
            case StateEnemy.dead:
                break;
            default:
                break;
        }
    }
    public void Hurt(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (timerAttack < (cdAttack - 0.5f))
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
        onDead.Invoke();
        enabled = false;
    }
    private void CheckPlayerInAttackArea()
    {
        hit = Physics2D.OverlapBox(
            transform.position +
            transform.right * checkAttackOffset.x +
            transform.up * checkAttackOffset.y,
            checkAttackSize, 0, 1 << 7);
        if (hit) state = StateEnemy.attack;
        else
        {
            state = StateEnemy.idle;
            timerAttack = cdAttack - 0.5f;
        }
    }
    private void Attack()
    {
        if (timerAttack < cdAttack)
        {
            timerAttack += Time.deltaTime;
            //print(timerAttack);
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
        //print("攻擊");
        StartCoroutine(DelaySendDamageToPlayer());
    }
    private IEnumerator DelaySendDamageToPlayer()
    {
        for (int i = 0; i < attackDelay.Length; i++)
        {
            yield return new WaitForSeconds(attackDelay[i]);

            if (hit) player.Hurt(attack);
        }
    }
    #endregion
}
//定義列舉
// 1. 使用關鍵字 enum 定義列舉以及包含的選項, 可以在類別外定義
// 2. 需要有一個欄位定義為此列舉類型
// 語法: 修飾詞 enum 列舉名稱{選項1, 選項2, ....., 選項N}
enum StateEnemy
{
    idle, attack, dead, hurt
}
