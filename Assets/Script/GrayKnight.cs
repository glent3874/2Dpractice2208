using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���M�h
/// </summary>
public class GrayKnight : MonoBehaviour
{
    #region ���
    [Header("�̤j��q"), Range(0, 500)]
    public int hp = 100;
    [Header("�ثe��q")]
    public int currentHealth;
    public HealthBar healthBar;
    [Header("�����O"), Range(0, 1000)]
    public int attack = 20;
    [Header("�����ϰ쪺�첾�j�p")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;
    [Header("�����N�o"), Range(0.5f, 5)]
    public float cdAttack = 3;
    private float timerAttack;
    [Header("��������"), Range(0.1f, 3)]
    public float[] attackDelay;
    [Header("���`�ƥ�")]
    public UnityEvent onDead;

    // �N�p�H�����ܦb�ݩʭ��O�W
    [SerializeField]
    private StateEnemy state;

    private Rigidbody2D rig;
    private Animator ani;
    private Player player;
    Collider2D hit;
    #endregion

    #region �ƥ�
    private void Start()
    {
        #region ��l�Ƽƭ�
        currentHealth = hp;
        healthBar.SetMaxHealth(hp);
        #endregion

        #region ���o����P���a���O
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
        //print("����");
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
//�w�q�C�|
// 1. �ϥ�����r enum �w�q�C�|�H�Υ]�t���ﶵ, �i�H�b���O�~�w�q
// 2. �ݭn���@�����w�q�����C�|����
// �y�k: �׹��� enum �C�|�W��{�ﶵ1, �ﶵ2, ....., �ﶵN}
enum StateEnemy
{
    idle, attack, dead, hurt
}
