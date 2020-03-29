using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncepad : MonoBehaviour
{
    const string animBounceId = "bounce";
    Animator animator;

    [SerializeField]
    float bounceForce = 400f;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameManager.PLAYER_TAG)
        {
            audioSource.PlayOneShot(audioSource.clip);
            animator.SetTrigger(animBounceId);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * bounceForce);
        }
    }
}
