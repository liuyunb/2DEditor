using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class AddGunBulletItem : ShootingEditor2DController
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.SendCommand<AddBulletCommand>();
        }
    }
}
