using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��⪱�a
/// </summary>
public class OrangePlayer : MonoBehaviour
{
    #region ���
    [Header("���z�t��")]
    public Rigidbody2D rb = null;
    [Header("���ʳt��")]
    public float moveSpeed = 5f;
    [Header("���D����"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("�O�_�b�a�O�W")]
    public bool onGround = false;
    [Header("�ˬd�a�O�ϰ�:�첾�P�b�|")]
    public Vector2 groundOffset;
    public Vector2 groundRadius;
    [Header("�O�_�b����Ʀ檬�A")]
    public bool wallSliding = false;
    [Header("�ˬd����ϰ�:�첾�P�b�|")]
    public Vector2 wallOffset;
    public Vector2 wallRadius;
    [Header("����Ʀ�t��")]
    public float wallSlidingSpeed;
    [Header("�O�_�b������D���A")]
    public bool wallJumping = false;
    [Header("������D���O�D�P�ɶ�")]
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    private float ad;
    #endregion

    #region �ƥ�
    private void Update()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// ø�s�����ϰ�
    /// </summary>
    private void OnDrawGizmos()
    {
        //�a�O����ø�s�ϰ�
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position + 
            transform.right * groundOffset.x + 
            transform.up * groundOffset.y,
            groundRadius);

        //�������ø�s�ϰ�
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position +
            transform.right * wallOffset.x +
            transform.up * wallOffset.y,
            wallRadius);
    }

    /// <summary>
    /// �I��������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collectable"))                     //�p�G�I����������ҬOcollectable
        {
            Destroy(collision.gameObject);                          //�����I��������
            GameoverController2.countAllCoins--;                    //�����ƶq-1
        }
    }
    #endregion

    #region ��k
    /// <summary>
    /// ����
    /// </summary>
    private void Move()
    {
        ad = Input.GetAxis("Horizontal");                           //���o������J��

        Vector2 move;
        move.x = ad * moveSpeed;                                    //�����b���ʬ�������J��*���ʳt��
        move.y = rb.velocity.y;                                     //�����b����
        rb.velocity = move;
    }

    /// <summary>
    /// ���D
    /// </summary>
    private void Jump()
    {
        //�a�O�����I���ϰ�
        Collider2D groundHit = Physics2D.OverlapBox(
            transform.position +
            transform.right * groundOffset.x +
            transform.up * groundOffset.y,
            groundRadius, 0, 1 << 6);

        if (groundHit) onGround = true;                                     //������a�O�N�}��onground
        else onGround = false;                                              //�S���N����onground

        //��������I���ϰ�
        Collider2D wallHit = Physics2D.OverlapBox(
            transform.position +
            transform.right * wallOffset.x +
            transform.up * wallOffset.y,
            wallRadius, 0, 1 << 6);

        if (wallHit && !groundHit && Input.GetAxis("Horizontal") != 0)      //�p�G���I������B�S���I��a�O�B�����b����J�ȴN�}��wallsliding
            wallSliding = true;
        else wallSliding = false;                                           //�S���N����wallsliding

        if (wallSliding)                                                    //�}�l��������Ʀ檫�z
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        if(Input.GetKeyDown(KeyCode.UpArrow) &&�@wallSliding)               //������D
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping) rb.velocity = new Vector2(xWallForce * -ad, yWallForce);

        //���U���D����D
        if(Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        //��}���D��}�l����
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
