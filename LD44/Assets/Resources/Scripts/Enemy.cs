using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject target;
    private float Range = 10f;
    public float walkSpeed = 10f;

    public float throwSpeed;
    public float moveTowardsSpeed;

    private bool isGrabbed;
    private bool isThrown;
    private Rigidbody2D body;
    private Queue<Vector3> previousPositions = new Queue<Vector3>();
    private Vector3 previousPosition;

    //AI states
    private bool hasGold = false;

    // Use this for initialization
    void Start()
    {
        
        body = GetComponentInChildren<Rigidbody2D>();
        previousPosition = body.position;
    }

    void Awake()
    {
        //target = GameObject.FindGameObjectWithTag("gold_pile");
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), moveTowardsSpeed * Time.deltaTime);
            body.position = newPos;
        }
        else if (isThrown)
        {

        }
        else if (!hasGold)
        {
            //Walk if not grabbed
            MoveTowardNearestObjectWithTag("gold_pile");

        }
        else if (hasGold)
        {
            //Walk if not grabbed
            MoveTowardNearestObjectWithTag("door");

        }
    }
    private void MoveTowardNearestGoldPile()
    {
        if (target == null)
        {
            target = FindClosestGoldPile();
        }
        

        if (target != null)
        {

            Vector2 direction = new Vector2((transform.position.x - target.transform.position.x), (transform.position.y - target.transform.position.y));
            Vector2 directionNormalized = direction.normalized;
            Vector2 velocity = new Vector2(directionNormalized.x * walkSpeed, directionNormalized.y * walkSpeed);
            GetComponent<Rigidbody2D>().velocity = -velocity;
        }

    }

    private void MoveTowardNearestObjectWithTag(string tag)
    {
        if (target == null)
        {
            target = FindClosestObjectWithTag(tag);
        }


        if (target != null)
        {

            Vector2 direction = new Vector2((transform.position.x - target.transform.position.x), (transform.position.y - target.transform.position.y));
            Vector2 directionNormalized = direction.normalized;
            Vector2 velocity = new Vector2(directionNormalized.x * walkSpeed, directionNormalized.y * walkSpeed);
            GetComponent<Rigidbody2D>().velocity = -velocity;
        }

    }

    private GameObject FindClosestObjectWithTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void FixedUpdate()
    {
        previousPositions.Enqueue(body.position - (Vector2)previousPosition);
        if (previousPositions.Count > 5)
            previousPositions.Dequeue();

        previousPosition = body.position;
    }

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void ReleaseGrab()
    {
        Debug.Log("releaseGrab");

        isGrabbed = false;

        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousPositions)
        {
            totalVelocity += velocity;
        }

        if (previousPositions.Count != 0)
            body.velocity = (totalVelocity / previousPositions.Count) * throwSpeed;
    }

}
