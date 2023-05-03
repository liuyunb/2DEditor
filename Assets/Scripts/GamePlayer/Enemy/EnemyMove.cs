using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Trigger2DCheck _walkCheck;//地面
    private Trigger2DCheck _fallCheck;//掉落
    private Trigger2DCheck _groundCheck;//障碍物检测

    private Rigidbody2D _rb;

    private void Awake()
    {
        _walkCheck = transform.Find("WalkCheck").GetComponent<Trigger2DCheck>();
        _fallCheck = transform.Find("FallCheck").GetComponent<Trigger2DCheck>();
        _groundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        AIMove();
    }

    public void  AIMove()
    {
        var dir = Mathf.Sign(transform.localScale.x);

        if (_walkCheck.Triggered && _fallCheck.Triggered && !_groundCheck.Triggered)
        {
            _rb.velocity = new Vector2(moveSpeed * dir, _rb.velocity.y);
        }
        else
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
