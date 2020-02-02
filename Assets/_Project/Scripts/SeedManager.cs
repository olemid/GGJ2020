using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public GameObject plant;
    [HideInInspector] public bool isPlanted = false;

    public void PlantSeed()
    {
        isPlanted = true;

        if (plant != null) 
            plant.SetActive(true);
    }

    public void ResetSeed()
    {
        print("Reset seed");
        isPlanted = false;

        if (plant != null)
            plant.SetActive(false);
    }
}
