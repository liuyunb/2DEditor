using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using Command;
using FrameWorkDesign;
using UnityEngine;

public class Bullet : ShootingEditor2DController
{
    private Rigidbody2D _rb;

    public float shootSpeed = 10f;

    public float destroyTime = 10f;
    // Start is called before the first frame update

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        var isRight = Mathf.Sign(transform.lossyScale.x);
        _rb.velocity = new Vector2(shootSpeed * isRight, _rb.velocity.y);
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
            this.SendCommand<KillEnemyCommand>();
        }
    }
}
