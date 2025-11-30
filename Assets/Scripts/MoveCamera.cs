using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playerCamPos;
    void Update()
    {
        transform.position = playerCamPos.position;
    }
}
