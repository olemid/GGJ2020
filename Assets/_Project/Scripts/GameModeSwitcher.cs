using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameModeSwitcher : MonoBehaviour
{
    public static GameModeSwitcher instance;

    [SerializeField] Camera mapCamera, challengeCamera;

    [SerializeField] Canvas challengeCanvas;

    [Header("Music")]
    public AudioSource backgroundMusic;
    Sequence musicSequence;
    public float musicFadedVolume = 0.2f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            DestroyImmediate(this);
    }

    private void Start()
    {
        EnableMapMode();
    }

    public void EnableMapMode()
    {
        challengeCamera.gameObject.SetActive(false);
        mapCamera.gameObject.SetActive(true);

        challengeCanvas.gameObject.SetActive(false);

        MapManager.instance.OnCompleteChallenge();
    }

    public void EnableChallengeMode()
    {
        mapCamera.gameObject.SetActive(false);
        challengeCamera.gameObject.SetActive(true);

        challengeCanvas.gameObject.SetActive(true);

        ChallengeManager.instance.StartNewChallenge();
    }

    public void SetChallengeCanvasActive(bool active)
    {
        challengeCanvas.gameObject.SetActive(active);
    }

    public void LowerMusic(float duration)
    {
        var previousVolume = backgroundMusic.volume;

        musicSequence = DOTween.Sequence();
        musicSequence.Append(backgroundMusic.DOFade(musicFadedVolume, 0.1f));
        musicSequence.AppendInterval(duration);
        musicSequence.Append(backgroundMusic.DOFade(previousVolume, 0.5f));
    }
}
