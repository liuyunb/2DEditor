using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _player;

    private float _originZ;

    public float minX = 0;
    public float maxX = 2f;

    public float minY = 0;
    public float maxY = 3f;

    private void Awake()
    {
        _originZ = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_player)
        {
            var player = GameObject.FindWithTag("Player");

            if (player)
            {
                _player = player.transform;
            }
            else
            {
                return;
            }

        }

        var isRight = Mathf.Sign(_player.transform.localScale.x);

        var playerPos = _player.position;

        var offset = new Vector3(2 * isRight, 0, 0);
        
        var cameraPos = transform.position;

        cameraPos = Vector3.Lerp(cameraPos, playerPos + offset, Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(cameraPos.x, minX, maxX), Mathf.Clamp(cameraPos.y, minY, maxY), _originZ);

    }
}
