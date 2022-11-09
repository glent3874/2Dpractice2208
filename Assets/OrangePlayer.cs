using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D 瞶╰参 = null;
    [SerializeField] float 禲硉 = 5f;

    private void Update()
    {
        float ad = Input.GetAxis("Horizontal");

        Vector2 move;
        move.x = ad * 禲硉;
        move.y = 瞶╰参.velocity.y;

        Vector2 锣传畒夹 = this.transform.TransformVector(move);

        瞶╰参.velocity = 锣传畒夹;
    }
}
