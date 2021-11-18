using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraPOV : MonoBehaviour
{
    public Transform Player;
    public Camera Behind, Above;
    public KeyCode TKey;
    public bool camSwitch = false;

    void Update()
    {
        if (Input.GetKeyDown(TKey))
        {
            camSwitch = !camSwitch;
            Above.gameObject.SetActive(!camSwitch);
            Behind.gameObject.SetActive(camSwitch);
        }
    }
}
