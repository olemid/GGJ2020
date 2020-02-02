using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeTrigger : MonoBehaviour
{
    enum ChallengeType
    {
        Fire,
        Photo
    }

    [SerializeField] ChallengeType challengeType;

    public AudioClip hitSound;

    public GameObject initiateEffectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.transform.CompareTag("Player"))
        {
            transform.parent.GetComponent<AudioSource>().PlayOneShot(hitSound);

            switch (challengeType)
            {
                case ChallengeType.Fire:
                    MapManager.instance.bus.Freeze();
                    Instantiate(initiateEffectPrefab, collision.transform.position - new Vector3(0, 0, 1), Quaternion.identity);
                    break;
                case ChallengeType.Photo:
                    break;
            }

            Destroy(gameObject);
        }
    }
}
