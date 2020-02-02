using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO
// rectangular traces, rotate based on velocity

public class BusController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10;

    [SerializeField]
    float spriteRotatefrequency = 0.1f;

    Vector2 moveAxis;
    bool freezeInput = false;

    // references
    new Rigidbody2D rigidbody { get { return GetComponent<Rigidbody2D>(); } }
    AudioSource audioSource;
    SpriteRenderer sprite;

    // footprint
    public GameObject footprintPrefab;
    public float footprintDistanceThreshold = 0.05f;
    public Vector3 lastFootprintPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();

        InvokeRepeating(nameof(RotateSprite), 0, spriteRotatefrequency);
    }

    void Update()
    {
        if (freezeInput)
            return;

        // get movement input
        moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //move rigidbody
        rigidbody.AddForce(moveAxis * moveSpeed, ForceMode2D.Force);

        // pitch engine to fit speed
        audioSource.pitch = Mathf.Lerp(0.65f, 1f, Mathf.Max(Mathf.Abs(rigidbody.velocity.x), Mathf.Abs(rigidbody.velocity.y)));

        // flip sprite according to direction
        if (moveAxis.x > 0)
            sprite.flipX = false;
        else if(moveAxis.x < 0)
            sprite.flipX = true;

        // spawn footprint
        if(Vector3.Distance(transform.position, lastFootprintPosition) >= footprintDistanceThreshold)
        {
            //print(Vector3.Angle(transform.position, lastFootprintPosition));
            LeaveFootprint();
        }
    }

    /// <summary>
    /// Rotates the sprite by a random amount within given range
    /// </summary>
    void RotateSprite()
    {
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(-8f, 8f));
    }

    /// <summary>
    /// Instantiates a footprint prefab
    /// </summary>
    void LeaveFootprint()
    {
        Instantiate(footprintPrefab, transform.position + new Vector3(0, 0, 0.5f), Quaternion.identity);//Quaternion.Euler(0, 0, Vector3.Angle(transform.position, lastFootprintPosition)));
        lastFootprintPosition = transform.position + new Vector3(0, 0, 0.5f);
    }

    /// <summary>
    /// Stops velocity and blocks input
    /// </summary>
    public void Freeze()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        freezeInput = true;
    }

    /// <summary>
    /// Resumes input
    /// </summary>
    public void Resume()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        freezeInput = false;
    }
}
