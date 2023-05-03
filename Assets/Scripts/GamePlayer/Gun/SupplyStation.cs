using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using UnityEngine;

public class SupplyStation : ShootingEditor2DController
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.SendCommand<FullBulletCommand>();
        }
    }
}
