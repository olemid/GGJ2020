using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static Levels instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            DestroyImmediate(this);
    }

    public int currentLevel = 1;

    public static float crosshairSpeed1 = 0.4f;
    public static float crosshairSpeed2 = 0.525f;
    public static float crosshairSpeed3 = 0.65f;

    public static float availableTime1 = 16;
    public static float availableTime2 = 12;
    public static float availableTime3 = 9;

}
