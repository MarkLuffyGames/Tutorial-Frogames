using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    void Update()
    {
        transform.position = new Vector3(PlayerController.instance.transform.position.x + 6,transform.position.y,transform.position.z);
    }
}
