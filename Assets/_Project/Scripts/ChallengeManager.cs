using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager instance;

    public enum Tool
    {
        FireHose = 0,
        FeedingBottle = 1,
        SeedBag = 2
    }

    public Tool currentActiveTool = Tool.FireHose;

    [Header("Tools")][SerializeField]
    List<Image> toolIcons;

    [Header("Fires")][SerializeField]
    List<GameObject> fires;
    public int amountOfWaterSquirts = 10;
    public GameObject[] fireObjectsToToggle;
    [SerializeField] List<Collider> hitBoxes;

    [Header("Koala")]
    public GameObject[] koalaObjectsToToggle;
    Transform activeKoala;
    public List<KoalaManager> koalas;
    public List<Collider> koalaHitBoxes;

    bool isKoalaFed = false;
   

    [Header("Shared")]
    public float challengeAvaliableTime = 10f;
    float challengeTimer;
    [HideInInspector] public bool challengeActive = false;
    public SpriteRenderer fadeSprite;
    public Image timerImage;
    public Slider waterSlider;

    [SerializeField] AudioClip finishSound, winSound, loseSound;

    // Fire variables
    Transform activeTree;
    int[] defaultChildren = { 0, 1, 2, 3, 4 };
    int[] currentTreeFires;

    int firePoints = 0;
    static int firePointsToWin = 5;

    public UnityEvent challengeComplete;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            DestroyImmediate(this);
    }

    void Start()
    {
        toolIcons[(int)currentActiveTool].rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// Activates a new challenge session
    /// </summary>
    public void StartNewChallenge()
    {
        // choose a tree to burn
        int index = Random.Range(0, hitBoxes.Count);

        for (int i = 0; i < hitBoxes.Count; i++)
        {
            fires[i].SetActive(i == index);
            hitBoxes[i].gameObject.SetActive(i == index);

            if (i == index)
            {
                foreach (Transform child in fires[i].transform)
                {
                    child.gameObject.SetActive(true);
                }
                activeTree = fires[i].transform;
            }
        }

        // activate a koala
        if(Levels.instance.currentLevel >= 0)
        {
            index = Random.Range(0, koalas.Count);

            for(int i = 0; i < koalas.Count; i++)
            {
                koalas[i].gameObject.SetActive(i == index);
                koalaHitBoxes[i].gameObject.SetActive(i == index);

                koalas[i].ResetKoala();

                if (i == index)
                    activeKoala = koalas[i].transform;
            }
            isKoalaFed = false;
        }
        else
        {
            activeKoala = null;
        }

        challengeTimer = 0;
        firePoints = 0;

        waterSlider.value = 1;

        SelectTool(1);

        challengeActive = true;

        currentTreeFires = defaultChildren;
    }

    private void FixedUpdate()
    {
        if (challengeActive)
        {
            challengeTimer += Time.fixedDeltaTime;

            timerImage.fillAmount = 1 - challengeTimer / challengeAvaliableTime;

            if(challengeTimer >= challengeAvaliableTime)
            {
                TimeUp();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentActiveTool != Tool.FireHose)
            {
                SelectTool(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentActiveTool != Tool.FeedingBottle)
            {
                SelectTool(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentActiveTool != Tool.SeedBag)
            {
                SelectTool(3);
            }
        }
    }

    void SelectTool(int index)
    {
        toolIcons[(int)currentActiveTool].rectTransform.DOScale(1, 0.35f).SetEase(Ease.OutBounce);

        if (index == 1)
            currentActiveTool = Tool.FireHose;
        else if (index == 2)
            currentActiveTool = Tool.FeedingBottle;
        else if (index == 3)
            currentActiveTool = Tool.SeedBag;

        toolIcons[(int)currentActiveTool].rectTransform.DOScale(1.5f, 0.35f).SetEase(Ease.OutBounce);

        foreach (GameObject go in fireObjectsToToggle)
            go.SetActive(index == 1);

        foreach (GameObject go in koalaObjectsToToggle)
            go.SetActive(index == 2);
    }


    void TimeUp()
    {
        challengeActive = false;
        GameModeSwitcher.instance.LowerMusic(finishSound.length);
        GetComponent<AudioSource>().PlayOneShot(finishSound);

        Invoke(nameof(ExitChallenge), 1f);
    }


    //
    //
    // Fire
    //
    //

    void NoMoreWater()
    {
        challengeActive = false;
        GameModeSwitcher.instance.LowerMusic(loseSound.length);
        GetComponent<AudioSource>().PlayOneShot(loseSound);

        Invoke(nameof(ExitChallenge), 1f);
    }

    //TODO make it actually random
    public void PutOutRandomFire()
    {
        activeTree.GetChild(firePoints).gameObject.SetActive(false);

        waterSlider.value -= 1f / amountOfWaterSquirts;

        firePoints++;

        bool win = CheckHasWon();

        if(!win)
            checkHasLost();
    }

    public void MisClickFirehose()
    {
        waterSlider.value -= 1f / amountOfWaterSquirts;
        checkHasLost();
    }

    //
    //
    // KOALA
    //
    //

    public void FeedKoala()
    {
        print("Feed Koala");
        activeKoala.GetComponent<KoalaManager>().StartFeeding();
        isKoalaFed = true;

        CheckHasWon();
    }

    public void MisclickKoala()
    {
        checkHasLost();
    }

    //
    //
    // WIN & LOSE
    //
    //

    void WinChallenge()
    {
        challengeActive = false;
        GameModeSwitcher.instance.LowerMusic(winSound.length);
        GetComponent<AudioSource>().PlayOneShot(winSound);

        challengeComplete.Invoke();

        Invoke(nameof(ExitChallenge), 3f);
    }

    void ExitChallenge()
    {
        GameModeSwitcher.instance.SetChallengeCanvasActive(false);
        fadeSprite.color = new Color(0, 0, 0, 0);
        fadeSprite.gameObject.SetActive(true);
        fadeSprite.DOFade(1, 1f).OnComplete(OnFadeComplete);
    }

    void OnFadeComplete()
    {
        fadeSprite.gameObject.SetActive(false);
        GameModeSwitcher.instance.EnableMapMode();
    }

    bool CheckHasWon()
    {
        if (firePoints == firePointsToWin)
        {
            if(activeKoala == null || (activeKoala != null && isKoalaFed))
            {
                WinChallenge();
                return true;
            }
        }
        return false;
    }

    void checkHasLost()
    {
        if(waterSlider.value <= 0 && firePoints < firePointsToWin)
        {
            NoMoreWater();
        }
    }
}
