using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [SerializeField] float wiggleFrequency = 0.1f;
    [SerializeField] Vector2 wiggleRange = new Vector2(-16, 16);

    void Start()
    {
        InvokeRepeating(nameof(WiggleMe), 0, wiggleFrequency);
    }

    void WiggleMe()
    {
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(wiggleRange.x, wiggleRange.y));
    }
}
