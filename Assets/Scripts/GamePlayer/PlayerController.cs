using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using UnityEngine;

public class PlayerController : ShootingEditor2DController
{
    public float moveSpeed = 5f;

    public float jumpHeight = 3f;

    private Trigger2DCheck _checkGround;
    
    private Rigidbody2D _rb;

    //跳跃相关输入
    private bool _jumpPressed;
    private bool _isJumping;
    private bool _isOnGround;
    private bool _isReload;
    //发射子弹相关
    private Gun _gun;
    
    private bool _isShoot;
    private bool _isShiftGun;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _checkGround = transform.Find("CheckGround").GetComponent<Trigger2DCheck>();
        _gun = transform.Find("Gun").GetComponent<Gun>();
    }

    private void Start()
    {
        _isOnGround = _checkGround.Triggered;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            _isShoot = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _isReload = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isShiftGun = true;
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (_jumpPressed)
        {
            Jump();
        }

        if (_isShoot)
        {
            Shoot();
            _isShoot = false;
        }

        if (_isReload)
        {
            Reload();
            _isReload = false;
        }

        if (_isShiftGun)
        {
            ShiftGun();
            _isShiftGun = false;
        }
    }

    private void Shoot()
    {
        _gun.Shoot();
    }

    private void Reload()
    {
        _gun.Reload();
    }

    private void ShiftGun()
    {
        this.SendCommand<ShiftGunCommand>();
    }

    private void Jump()
    {
        if (_checkGround.Triggered && !_isJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
            _isJumping = true;
        }

        if (!_checkGround.Triggered && _isJumping)
            _isOnGround = false;

        if (!_isOnGround && _checkGround.Triggered)
            JumpEnd();
    }

    private void JumpEnd()
    {
        _isJumping = false;
        _jumpPressed = false;
        _isOnGround = true;
    }

    private void Move()
    {
        var dir = Input.GetAxis("Horizontal");

        if (dir * transform.localScale.x < 0)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

        }
        
        dir = Mathf.Clamp(dir + _rb.velocity.x / 5, -1, 1);
        _rb.velocity = new Vector2(dir * moveSpeed, _rb.velocity.y);

    }
}
