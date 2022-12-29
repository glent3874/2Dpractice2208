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
    public int maxHp = 100;
    [Header("�ثe��q")]
    public int currentHp;
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
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator ani;
    [SerializeField] Player player;
    [SerializeField] CapsuleCollider2D CapsuleCollider2D;
    Collider2D hit;
    #endregion

    #region �ƥ�
    private void Start()
    {
        currentHp = maxHp;              //��l�Ʀ�q
        healthBar.SetMaxHealth(maxHp);  //�񺡦��
    }

    private void Update()
    {
        CheckPlayerInAttackArea();      //�ˬd���a�O�_�i�J�����ϰ�
    }

    /// <summary>
    /// ø�s����gizmos
    /// </summary>
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
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage">�ˮ`�q</param>
    public void Hurt(int damage)
    {
        currentHp -= damage;                    //��e��q��ˮ`�q
        healthBar.SetHealth(currentHp);         //�]�m���
        if (timerAttack < (cdAttack - 0.5f))    //��������i�H�i�J���˰ʵe
            ani.SetTrigger("hurt");
        if (currentHp <= 0) Dead();             //��q�֩�s���`
    }

    /// <summary>
    /// ���`
    /// </summary>
    private void Dead()
    {
        currentHp = 0;                          //��q�k�s
        ani.SetBool("dead", true);              //�}�Ҧ��`�ʵe
        CapsuleCollider2D.enabled = false;      //�����I����
        onDead.Invoke();                        //�Ұʦ��`�ƥ�
        enabled = false;                        //�������}��
    }

    /// <summary>
    /// �ˬd���a�O�_�i�J�����d��
    /// </summary>
    private void CheckPlayerInAttackArea()
    {
        //���ͧP�w�ϰ�
        hit = Physics2D.OverlapBox(
            transform.position + 
            transform.right * checkAttackOffset.x + 
            transform.up * checkAttackOffset.y, 
            checkAttackSize, 0, 1 << 7);
        if (hit) Attack();                      //�p�G�i�J�d��N�i�J�������A
        else 
            timerAttack = cdAttack - 0.5f;      //���}�d��N���]�����N�o�p�ɾ�
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        if (timerAttack < cdAttack)                             //�����N�o�p�ɾ�
            timerAttack += Time.deltaTime;
        else                                                    //�ɶ���N����
        {
            timerAttack = 0;                                    //�k�s�p�ɾ�
            ani.SetTrigger("attack");                           //�Ұʧ����ʵe
            StartCoroutine(DelaySendDamageToPlayer());          //�Ұʶˮ`�����{
        }
    }
    
    /// <summary>
    /// ����ˮ`��{
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelaySendDamageToPlayer()
    {
        //����ˮ`���p��Ϩ�P�ʵe�P�B
        for (int i = 0; i < attackDelay.Length; i++)
        {
            yield return new WaitForSeconds(attackDelay[i]);    //�b�����ݳ]�w���ɶ�
            if (hit) player.Hurt(attack);                       //�p�G���a�٦b�����d��N�p��ˮ`
        }
    }
    #endregion
}

