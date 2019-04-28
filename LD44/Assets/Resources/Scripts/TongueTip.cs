using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour
{
    public float moveSpeed;
    public bool isTongueExtending;
    public bool isTongueReturning;
    public bool isPlayerTongueing = false;
    public bool isEnemyTongued = false;
    private Vector3 targetPosition;
    private Vector3 returnPosition;
    private PlayerController player;
    private Enemy tonguedEnemy;
    private float tongueLength = 5;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        returnPosition = player.transform.position;
        if (isPlayerTongueing)
        {
            if (isTongueExtending)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                //BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, GetPerpendicular(BezierLineRenderer.instance.point3.position, BezierLineRenderer.instance.point0.position, Random.Range(-5f, 5f))[Random.Range(0, 1)], moveSpeed * 10f * Time.deltaTime);
                //BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, GetPerpendicular(BezierLineRenderer.instance.point3.position, BezierLineRenderer.instance.point0.position, Random.Range(-5, 5f))[Random.Range(0, 1)], moveSpeed * 10f * Time.deltaTime);
            }
            if (isTongueReturning)
            {
                transform.position = Vector3.MoveTowards(transform.position, returnPosition, moveSpeed * Time.deltaTime);
                //BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, GetPerpendicular(BezierLineRenderer.instance.point3.position, BezierLineRenderer.instance.point0.position, Random.Range(-5f, 5f))[Random.Range(0, 1)], moveSpeed * 10f * Time.deltaTime);
                //BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, GetPerpendicular(BezierLineRenderer.instance.point3.position, BezierLineRenderer.instance.point0.position, Random.Range(-5f, 5f))[Random.Range(0, 1)], moveSpeed * 10f * Time.deltaTime);
            }

            if(BezierLineRenderer.instance.point1.position != targetPosition)
                BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, transform.position, moveSpeed * 0.8f * Time.deltaTime);

            if (BezierLineRenderer.instance.point2.position != targetPosition)
                BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, transform.position, moveSpeed * 0.4f * Time.deltaTime);


            if (isEnemyTongued && tonguedEnemy)
            {
                transform.position = tonguedEnemy.GetEnemyPosition();
            }

            if (targetPosition == transform.position || Vector2.Distance(transform.position, player.transform.position) > tongueLength)
            {
                isTongueExtending = false;
                isTongueReturning = true; //!Input.GetButton("Fire1");
            }

            if (isTongueReturning && transform.position == returnPosition)
            {
                isTongueReturning = false;
                isPlayerTongueing = false;
                //player.GetComponentInChildren<LineRenderer>().positionCount = 0;
                GetComponent<SpriteRenderer>().enabled = false;
                player.tongueLine.sortingLayerName = "Hidden";
                player.spriteRenderer.sprite = ResourceLoader.instance.mimicClosedSprite;
            }

            //transform.position = BezierLineRenderer.instance.tipAttachPoint;
        }
        else
        {
            BezierLineRenderer.instance.point1.position = BezierLineRenderer.instance.point2.position = transform.position = returnPosition;
        }

        /* if (isEnemyTongued)
        {
            Debug.Log("I've tongued something, make me do something");
            isEnemyTongued = false;
            tonguedEnemy = null;
        }*/
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }

    public void ReleaseEnemy()
    {
//        Debug.Log("ReleaseEnemy");
        if (tonguedEnemy != null)
        {
            tonguedEnemy.ReleaseGrab();
            tonguedEnemy = null;
            RetractTongue();
        }
    }

    public void RetractTongue()
    {
        isTongueExtending = false;
        isTongueReturning = true;
        isEnemyTongued = false;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (tonguedEnemy)
            return;

        tonguedEnemy = other.GetComponentInChildren<Enemy>();
        if (tonguedEnemy && Input.GetButton("Fire1"))
        {
            Instantiate(ResourceLoader.instance.lickParticles);
            tonguedEnemy.StartGrab();
            isTongueExtending = false;
            isTongueReturning = false;
            isEnemyTongued = true;
        }
    }

    private Vector3[] GetPerpendicular(Vector3 p1, Vector3 p2, float magnitude)
    {
        /*
            v = P2 - P1
            P3 = (-v.y, v.x) / Sqrt(v.x^2 + v.y^2) * h
            P4 = (-v.y, v.x) / Sqrt(v.x^2 + v.y^2) * -h

            Basically, you first find the vector from P1 to P2. Then to get a perpendicular vector 
            you take the negative inverse of the vector (switching the axes around and making one negative, 
            which one you make negative determines the direction). After that, you find the normal of the 
            perpendicular vector by dividing the vector by it's magnitude (length). Finally, you multiply 
            by h and -h to get the vectors of the right length in each direction.
         */

        Vector3 v = p2 - p1;
        if (v.x == 0 && v.y == 0)
            return new Vector3[] { p1, p2 };

        Vector3 p3 = new Vector3(-v.y, v.x) / (Mathf.Sqrt(v.x * v.x + v.y * v.y) * magnitude);
        Vector3 p4 = new Vector3(-v.y, v.x) / (Mathf.Sqrt(v.x * v.x + v.y * v.y) * -magnitude);

        return new Vector3[] { p3, p4 };
    }
}
