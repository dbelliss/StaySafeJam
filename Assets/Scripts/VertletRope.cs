using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertletRope : MonoBehaviour
{
    private LineRenderer lr;
    private float ropeSegmentLength = 0.25f;    
    private float ropeWidth = 0.06f;
    private int numSegments = 20;
    private List<VertletSegment> vertletSegments = new List<VertletSegment>();

    Vector3 ropePlayerOffset = Vector2.down * .3f;
    public class VertletSegment
    {
        public Vector2 curPosition;
        public Vector2 posOld;

        public VertletSegment(Vector2 pos)
        {
            this.curPosition = pos;
            this.posOld = pos;
        }
    }

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = PlayerController.player1.transform.position + ropePlayerOffset;

        for (int i = 0; i < numSegments; i++)
        {
            vertletSegments.Add(new VertletSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegmentLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineRenderer();

        float distance = Vector2.Distance(PlayerController.player1.transform.position, PlayerController.player2.transform.position);
        Color lineColor = Color.red;
        if (distance < 3)
        {
            lineColor = Color.green;
        }
        else if (distance < 6)
        {
            lineColor = Color.yellow;
        }

        if (PlayerController.IsRopeHeld())
        {
            lineColor = Color.cyan;
        }

        lr.startColor = lineColor;
        lr.endColor = lineColor;
    }

    private void FixedUpdate()
    {
        SimulatePhysics();
    }



    private void StickToPlayers()
    {
        VertletSegment firstSegment = vertletSegments[0];
        firstSegment.curPosition = PlayerController.player1.transform.position + ropePlayerOffset;
        this.vertletSegments[0] = firstSegment;

        //Constrant to Second Point 
        VertletSegment endSegment = this.vertletSegments[vertletSegments.Count - 1];
        endSegment.curPosition = PlayerController.player2.transform.position + ropePlayerOffset;
        this.vertletSegments[vertletSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.numSegments - 1; i++)
        {
            VertletSegment firstSeg = this.vertletSegments[i];
            VertletSegment secondSeg = this.vertletSegments[i + 1];

            float dist = (firstSeg.curPosition - secondSeg.curPosition).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegmentLength)
            {
                changeDir = (firstSeg.curPosition - secondSeg.curPosition).normalized;
            }
            else if (dist < ropeSegmentLength)
            {
                changeDir = (secondSeg.curPosition - firstSeg.curPosition).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.curPosition -= changeAmount * 0.5f;
                vertletSegments[i] = firstSeg;
                secondSeg.curPosition += changeAmount * 0.5f;
                vertletSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.curPosition += changeAmount;
                vertletSegments[i + 1] = secondSeg;
            }
        }
    }

    private void UpdateLineRenderer()
    {
        float lineWidth = this.ropeWidth;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.numSegments];
        for (int i = 0; i < this.numSegments; i++)
        {
            ropePositions[i] = this.vertletSegments[i].curPosition;
        }

        lr.positionCount = ropePositions.Length;
        lr.SetPositions(ropePositions);
    }

    private void SimulatePhysics()
    {
        Vector2 gravity = Vector2.down;

        for (int i = 1; i < this.numSegments; i++)
        {
            VertletSegment firstSegment = this.vertletSegments[i];
            Vector2 velocity = firstSegment.curPosition - firstSegment.posOld;
            firstSegment.posOld = firstSegment.curPosition;
            firstSegment.curPosition += velocity;
            firstSegment.curPosition += gravity * Time.fixedDeltaTime;
            this.vertletSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.StickToPlayers();
        }
    }
}