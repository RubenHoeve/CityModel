using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementAbove : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 20, 0);
    }
}
