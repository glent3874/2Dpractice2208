using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePlayer : MonoBehaviour
{
    #region ���
    [SerializeField] Rigidbody2D ���z�t�� = null;
    [SerializeField] float �]�t = 5f;
    [Header("���D����"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("�O�_�b�a�O�W")]
    public bool onGround = false;
    [Header("�ˬd�a�O�ϰ�:�첾�P�b�|")]
    public Vector2 groundOffset;
    public Vector2 groundRadius;
    #endregion

    #region �ƥ�
    private void Update()
    {
        float ad = Input.GetAxis("Horizontal");

        Vector2 move;
        move.x = ad * �]�t;
        move.y = ���z�t��.velocity.y;

        Vector2 �ഫ�y�� = this.transform.TransformVector(move);

        ���z�t��.velocity = �ഫ�y��;

        Jump();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(
            transform.position + 
            transform.right * groundOffset.x + 
            transform.up * groundOffset.y,
            groundRadius);
    }
    #endregion

    #region ��k
    private void Jump()
    {
        Collider2D groundHit = Physics2D.OverlapBox(
            transform.position +
            transform.right * groundOffset.x +
            transform.up * groundOffset.y,
            groundRadius, 0, 1 << 6);

        Debug.Log(groundHit);

        if (groundHit) onGround = true;
        else onGround = false;

        if(Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            ���z�t��.velocity = new Vector2(���z�t��.velocity.x, jumpHeight);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow) && ���z�t��.velocity.y > 0f)
        {
            ���z�t��.velocity = new Vector2(���z�t��.velocity.x, ���z�t��.velocity.y * 0.5f);
        }
    }
    #endregion
}
