using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyParentAlpha : MonoBehaviour
{
    void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().color = transform.parent.GetComponent<SpriteRenderer>().color;
    }
}
