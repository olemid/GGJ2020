using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyAfterSeconds : MonoBehaviour
{
    public bool fade = true;

    public float lifetime = 1f;
    public float fadeTime = 0.5f;
    
    void Start()
    {
        if (fade)
            Invoke(nameof(StartFade), lifetime);
        else
            Destroy(gameObject, lifetime);
    }

    void StartFade()
    {
        GetComponent<SpriteRenderer>().DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject));
    }
}
