using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO projectile bottle

public class FireHose : MonoBehaviour
{
    public float moveSpeed = 1;

    public ParticleSystem water;
    public float waterSpawnRate = 64;

    public ParticleSystem seeds;
    public float seedSpawnRate = 16;

    public Transform hoseRootJoint;

    public Transform bottleTransform;

    public Transform seedPacketTransform;

    bool isFireCrosshairGood = false;
    bool isBottleCrosshairGood = false;
    bool isSeedCrosshairGood = false;

    public Image crosshairUI;


    private void Start()
    {
        ChallengeManager.instance.challengeComplete.AddListener(OnEndChallenge);
    }

    void Update()
    {
        if (!ChallengeManager.instance.challengeActive)
            return;

        // move the hose head
        float t = Mathf.PingPong(Time.time * moveSpeed * 2, 1f);

        crosshairUI.rectTransform.localPosition = new Vector3(Mathf.Lerp(-870, 870, t), 0, 0);

        transform.localPosition = new Vector3(Mathf.Lerp(-0.95f, 0.95f, t), transform.localPosition.y, transform.localPosition.z);
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.Lerp(-30, 30, t), 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.FireHose)
            {
                if (isFireCrosshairGood)
                {
                    ChallengeManager.instance.PutOutRandomFire();
                }
                else
                {
                    ChallengeManager.instance.MisClickFirehose();
                }
            }
            else if(ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.FeedingBottle)
            {
                if (isBottleCrosshairGood)
                    ChallengeManager.instance.FeedKoala();
                else
                    ChallengeManager.instance.MisClickFirehose();
            }

            if(ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.SeedBag)
            {
                if (isSeedCrosshairGood)
                    ChallengeManager.instance.PlantSeed();
                else
                    ChallengeManager.instance.MisclickSeed();
            }


        }

        if (ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.FireHose)
        {
            RotateHose(t);

            // adjust the particle system
            var waterEmission = water.emission;

            if (Input.GetKey(KeyCode.Space))
            {
                waterEmission.rateOverTime = waterSpawnRate;

                // TODO after a short delay, start depleting water?
            }
            else
            {
                waterEmission.rateOverTime = 0f;
            }
        }
        else if(ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.FeedingBottle)
        {
            bottleTransform.localPosition = new Vector3(Mathf.Lerp(-0.95f, 0.95f, t), bottleTransform.localPosition.y, bottleTransform.localPosition.z);
            bottleTransform.localEulerAngles = new Vector3(bottleTransform.localEulerAngles.x, Mathf.Lerp(-30, 30, t), 0);


        }
        else if(ChallengeManager.instance.currentActiveTool == ChallengeManager.Tool.SeedBag)
        {
            seedPacketTransform.localPosition = new Vector3(Mathf.Lerp(-0.95f, 0.95f, t), seedPacketTransform.localPosition.y, seedPacketTransform.localPosition.z);
            seedPacketTransform.localEulerAngles = new Vector3(seedPacketTransform.localEulerAngles.x, Mathf.Lerp(-35, 35, t), 0);

            var seedEmission = seeds.emission;
            if (Input.GetKey(KeyCode.Space))
            {
                seedEmission.rateOverTime = seedSpawnRate;
            }
            else
            {
                seedEmission.rateOverTime = 0f;
            }
        }
    }

    /// <summary>
    /// Rotates the fire hosed based on t lerp alpha
    /// </summary>
    /// <param name="t">T.</param>
    void RotateHose(float t)
    {
        float rotateValue = 0;
        float rotateIncrement = 1.5f;

        Transform node = hoseRootJoint;

        while(node.childCount != 0)
        {
            node.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(rotateValue, -rotateValue, t));

            rotateValue += rotateIncrement;
            node = node.GetChild(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.FireHose)
            isFireCrosshairGood = true;
        else if (other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.FeedingBottle)
            isBottleCrosshairGood = true;
        else if (other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.SeedBag)
            isSeedCrosshairGood = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.FireHose)
            isFireCrosshairGood = false;
        else if (other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.FeedingBottle)
            isBottleCrosshairGood = false;
        else if (other.GetComponent<ToolTrigger>().toolType == ChallengeManager.Tool.SeedBag)
            isSeedCrosshairGood = false;
    }

    // TODO this does not stop the water
    void OnEndChallenge()
    {
        var waterEmission = water.emission; 
        waterEmission.rateOverTime = 0f;
    }
}
