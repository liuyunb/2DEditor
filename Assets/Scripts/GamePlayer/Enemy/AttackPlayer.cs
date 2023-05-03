using System;
using Architecture;
using Command;
using FrameWorkDesign;
using UnityEngine;

namespace Player.Enemy
{
    public class AttackPlayer : ShootingEditor2DController
    {
        public int hurt = 1;

        public float impulsePower = 10f;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                this.SendCommand(new HurtPlayerCommand(hurt));
                var dir = col.transform.position - transform.position;
                dir.y += 0.5f;
                dir.Normalize();
                col.gameObject.GetComponent<Rigidbody2D>()?.AddForce(dir * impulsePower, ForceMode2D.Impulse);
            }
        }
    }
}