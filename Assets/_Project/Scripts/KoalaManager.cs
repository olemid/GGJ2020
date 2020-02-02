using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoalaManager : MonoBehaviour
{
    public GameObject bottle;
    public Transform arms;
    bool isFed = false;
    public float handsMoveSpeed = 0.5f;

    void Update()
    {
        if(isFed)
            arms.localEulerAngles = new Vector3(Mathf.Lerp(-25f, 15f, Mathf.PingPong(Time.time * handsMoveSpeed * 2, 1)), 0, 0);
    }

    public void StartFeeding()
    {
        isFed = true;
        bottle.SetActive(true);
    }

    public void ResetKoala()
    {
        print("Reset koala");
        isFed = false;
        bottle.SetActive(false);
    }
}
