using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonScaler : MonoBehaviour
{

    public float maxScale = 1.5f;
    public float speed = 0.25f;

    void Update()
    {
        var t = Mathf.PingPong(Time.time * 2 * speed, 1);

        var size = Mathf.Lerp(1, maxScale, t);

        GetComponent<RectTransform>().localScale = new Vector3(size, size, size);
    }
}
