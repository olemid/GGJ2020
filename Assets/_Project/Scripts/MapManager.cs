using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Header("Object references")]
    public SpriteRenderer map;
    public List<CircleCollider2D> colliders;
    public Transform min, max;

    public BusController bus;

    [Header("Challenges")]
    public GameObject challengePrefab;
    public GameObject photoPrefab;
    public float fireChallengeLikelihood = 0.2f;
    public float challengeFocusTweenTime = 2f;

    [Header("Spawning")]
    public Vector2 spawnRate = new Vector2(4, 8);

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            DestroyImmediate(this);
    }

    void Start()
    {
        // sort the colliders to biggest first for efficiency
        colliders.Sort((p1, p2) => p2.radius.CompareTo(p1.radius));
        
        Invoke(nameof(SpawnChallenge), Random.Range(spawnRate.x, spawnRate.y));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OnCompleteChallenge();
    }

    void SpawnChallenge()
    {
        float diceRoll = Random.Range(0f, 1f);

        if(diceRoll <= fireChallengeLikelihood)
            Instantiate(challengePrefab, GetValidPosition() + new Vector3(0, 0, -0.5f), Quaternion.identity, map.transform);
        else
            Instantiate(photoPrefab, GetValidPosition() + new Vector3(0, 0, -0.5f), Quaternion.identity, map.transform);

        // repeat
        Invoke(nameof(SpawnChallenge), Random.Range(spawnRate.x, spawnRate.y));
    }

    void PauseSpawning()
    {
        if (IsInvoking(nameof(SpawnChallenge)))
            CancelInvoke(nameof(SpawnChallenge));
    }

    public void OnInitiateChallenge()
    {
        PauseSpawning();

        bus.gameObject.SetActive(false);

        GameModeSwitcher.instance.EnableChallengeMode();
    }

    public void OnCompleteChallenge()
    {
        foreach (PunchScaleUp punch in FindObjectsOfType<PunchScaleUp>())
            Destroy(punch.gameObject);

        bus.gameObject.SetActive(true);
        bus.Resume();

        // resume spawning
        Invoke(nameof(SpawnChallenge), Random.Range(spawnRate.x, spawnRate.y));
    }

    /// <summary>
    /// Tries until it finds a valid position to return
    /// </summary>
    /// <returns>The valid position.</returns>
    Vector3 GetValidPosition()
    {
        Vector3 temp = GetRandomPosition();

        int count = 0;
        while (!IsInBounds(temp))
        {
            count++;
            if(count > 16)
            {
                Debug.LogError("Too many iterations in placement");
                return Vector3.zero;
            }
            temp = GetRandomPosition();
        }

        return new Vector3(temp.x, temp.y, temp.z);
    }

    /// <summary>
    /// Uses corner GameObjects to find a random position within bounds
    /// </summary>
    /// <returns>The random position.</returns>
    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y), transform.position.z);
    }

    /// <summary>
    /// Checks if a given point is inside any of the colliders
    /// </summary>
    /// <returns><c>true</c>, if in bounds was ised, <c>false</c> otherwise.</returns>
    /// <param name="point">Point.</param>
    bool IsInBounds(Vector3 point)
    {
        foreach(CircleCollider2D col in colliders)
        {
            if (col.bounds.Contains(point))
                return true;
        }
        return false;
    }
}
