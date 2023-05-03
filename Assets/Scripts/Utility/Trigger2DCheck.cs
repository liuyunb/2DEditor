using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2DCheck : MonoBehaviour
{
    public LayerMask targetMask;

    public int enterCount = 0;

    public bool Triggered
    {
        get { return enterCount > 0; }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsInLayerMask(col.gameObject, targetMask))
        {
            enterCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, targetMask))
        {
            enterCount--;
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        var objLayerMask = 1 << obj.layer;
        return (mask.value & objLayerMask) > 0;
    }
}
