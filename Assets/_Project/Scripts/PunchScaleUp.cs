using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchScaleUp : MonoBehaviour
{
    public float initialPunchScale = 10;
    public float finalScale = 100;

    public float initialPunchTime = 1f;
    public float delayBetween = 0.5f;
    public float finalScaleTime = 5f;

    public Ease finalScaleEase = Ease.InBack;

    Sequence mySequence;

    void Start()
    {
        ScaleUp();
    }

    void ScaleUp()
    {
        mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(initialPunchScale, initialPunchScale, initialPunchScale), initialPunchTime).SetEase(Ease.OutElastic));
        mySequence.AppendInterval(delayBetween);
        mySequence.Append(transform.DOScale(finalScale, finalScaleTime).SetEase(finalScaleEase));
        mySequence.AppendCallback(MapManager.instance.OnInitiateChallenge);
    }

}
