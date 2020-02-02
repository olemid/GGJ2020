using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAxis : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * rotateSpeed, 0);
    }
}
