using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D ���z�t�� = null;
    [SerializeField] float �]�t = 5f;

    private void Update()
    {
        float ad = Input.GetAxis("Horizontal");

        Vector2 move;
        move.x = ad * �]�t;
        move.y = ���z�t��.velocity.y;

        Vector2 �ഫ�y�� = this.transform.TransformVector(move);

        ���z�t��.velocity = �ഫ�y��;
    }
}
