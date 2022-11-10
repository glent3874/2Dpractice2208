using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePlayer : MonoBehaviour
{
    #region 逆
    [SerializeField] Rigidbody2D 瞶╰参 = null;
    [SerializeField] float 禲硉 = 5f;
    [Header("铬臘蔼"), Range(0, 20)]
    public int jumpHeight = 16;
    [Header("琌狾")]
    public bool onGround = false;
    [Header("浪琩狾跋办:簿籔畖")]
    public Vector2 groundOffset;
    public Vector2 groundRadius;
    #endregion

    #region ㄆン
    private void Update()
    {
        float ad = Input.GetAxis("Horizontal");

        Vector2 move;
        move.x = ad * 禲硉;
        move.y = 瞶╰参.velocity.y;

        Vector2 锣传畒夹 = this.transform.TransformVector(move);

        瞶╰参.velocity = 锣传畒夹;

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

    #region よ猭
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
            瞶╰参.velocity = new Vector2(瞶╰参.velocity.x, jumpHeight);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow) && 瞶╰参.velocity.y > 0f)
        {
            瞶╰参.velocity = new Vector2(瞶╰参.velocity.x, 瞶╰参.velocity.y * 0.5f);
        }
    }
    #endregion
}
