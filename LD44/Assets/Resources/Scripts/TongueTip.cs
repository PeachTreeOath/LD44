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

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
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
                BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, targetPosition, moveSpeed * 0.5f * Time.deltaTime);
                BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, targetPosition, moveSpeed * 0.25f * Time.deltaTime);
            }
            if (isTongueReturning)
            {
                transform.position = Vector3.MoveTowards(transform.position, returnPosition, moveSpeed * Time.deltaTime);
                BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, returnPosition, moveSpeed * 1.5f * Time.deltaTime);
                BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, returnPosition, moveSpeed * 1.5f * Time.deltaTime);
            }

            if (isEnemyTongued && tonguedEnemy)
            {
                transform.position = tonguedEnemy.GetEnemyPosition();
                BezierLineRenderer.instance.point1.position = Vector3.MoveTowards(BezierLineRenderer.instance.point1.position, transform.position, moveSpeed * 0.6f * Time.deltaTime);
                BezierLineRenderer.instance.point2.position = Vector3.MoveTowards(BezierLineRenderer.instance.point2.position, transform.position, moveSpeed * 0.3f * Time.deltaTime);
            }

            if (targetPosition == transform.position)
            {
                isTongueExtending = false;
                isTongueReturning = true;
            }

            if (isTongueReturning && transform.position == returnPosition)
            {
                isTongueReturning = false;
                isPlayerTongueing = false;
                //player.GetComponentInChildren<LineRenderer>().positionCount = 0;
                player.spriteRenderer.sprite = ResourceLoader.instance.mimicClosedSprite;
            }
        }
        else
        {
            transform.position = returnPosition;
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
        Debug.Log("ReleaseEnemy");
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
            tonguedEnemy.StartGrab();
            isTongueExtending = false;
            isTongueReturning = false;
            isEnemyTongued = true;
        }
    }
}
