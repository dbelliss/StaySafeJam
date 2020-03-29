using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField]
    List<Transform> patrolPoints = new List<Transform>();
    int curPatrolPoint = 0;
    float desiredPatrolPointProximity = .5f;
    Rigidbody2D rb;
    SpriteRenderer sr;

    [SerializeField]
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x > .1f)
        {
            sr.flipX = true;
        }
        else if (rb.velocity.x < -.1f)
        {
            sr.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (patrolPoints.Count == 0)
        {
            return;
        }

        Vector3 goal = patrolPoints[curPatrolPoint].position;
        float curDistanceFromGoal = Vector2.Distance(goal, transform.position);
        if (curDistanceFromGoal < desiredPatrolPointProximity)
        {
            curPatrolPoint = (curPatrolPoint + 1) % patrolPoints.Count;
        }

        Vector2 directionToGoal = goal - transform.position;
        directionToGoal = new Vector2(directionToGoal.x, 0);
        rb.velocity = directionToGoal * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameManager.PLAYER_TAG)
        {
            RoomManager.instance.Die();
        }
    }
}
