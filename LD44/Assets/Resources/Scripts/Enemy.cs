using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject target;
    private string targetTag;
    private float Range = 10f;
    public float walkSpeed = 10f;
    public float timeToGrabGold = 3f;
    public float timeToRecalcTarget = 3f;

    public float throwSpeed;
    public float moveTowardsSpeed;

    private bool isGrabbed;
    private bool isThrown;
    private Rigidbody2D body;
    private Queue<Vector3> previousPositions = new Queue<Vector3>();
    private Vector3 previousPosition;
    
    public bool hasGold = false;
    private float secondsGrabbingGold = 0f;
    private float secondsMovingToTarget = 0f;
    private GameObject heldTreasure = null;

    public enum EnemyState
    {
        SEEKING_GOLD,
        GATHERING_GOLD,
        LEAVING_WITH_GOLD,
        STRUGGLING,
        BUMPED,
        DAMAGED,
        DYING,
        BEING_FLUNG
    }

    private EnemyState enemyState;
    private int timeInStatus, goldCarried;

    //enemy type attributes
    private int mass, speed, maxGoldCapacity, stunTime, goldGatherRange, goldGatherTime;

    private EnemyState selfDetermineState()
    {
        if (goldCarried >= maxGoldCapacity)
        {
            return EnemyState.LEAVING_WITH_GOLD;
        }

        /*else if () { // enemy is within range of a gold pile
          return EnemyState.GATHERING_GOLD;
        }*/
        else return EnemyState.SEEKING_GOLD;
    }

    private void doStateAction()
    {
        if (enemyState == null)
        {
            enemyState = selfDetermineState();
        }

        switch (enemyState)
        {
            case EnemyState.SEEKING_GOLD:
                seekingGoldBehavior();
                break;
            case EnemyState.GATHERING_GOLD:
                gatheringGoldBehavior();
                break;
            case EnemyState.LEAVING_WITH_GOLD:
                leavingWithGoldBehavior();
                break;
            case EnemyState.STRUGGLING:
                strugglingBehavior();
                break;
            case EnemyState.BUMPED:
                bumpedBehavior();
                break;
            case EnemyState.DAMAGED:
                damagedBehavior();
                break;
            case EnemyState.DYING:
                dyingBehavior();
                break;
            case EnemyState.BEING_FLUNG:
                beingFlungBehavior();
                break;
            default:
                throw new System.Exception();
        }
    }


    // Use this for initialization
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
        previousPosition = body.position;
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

    private void MoveTowardNearestObjectWithTag(string tag)
    {
        if (target == null || !targetTag.Equals(tag) || secondsMovingToTarget >= timeToRecalcTarget)
        {
            target = FindClosestObjectWithTag(tag);
            targetTag = tag;
            secondsMovingToTarget = 0f;
        }


        if (target != null)
        {
            
            Vector2 direction = new Vector2((transform.position.x - target.transform.position.x), (transform.position.y - target.transform.position.y));
            Vector2 directionNormalized = direction.normalized;
            Vector2 velocity = new Vector2(directionNormalized.x * walkSpeed, directionNormalized.y * walkSpeed);
            GetComponent<Rigidbody2D>().velocity = -velocity;
            secondsMovingToTarget += Time.deltaTime;
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
        isThrown = true;

        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousPositions)
        {
            totalVelocity += velocity;
        }

        if (previousPositions.Count != 0)
            body.velocity = (totalVelocity / previousPositions.Count) * throwSpeed;
    }

    public Vector2 GetEnemyPosition()
    {
        return body.position;
    }


    public void OnTriggerStay2D(Collider2D otherCollider)
    {
        //Start picking up gold
        if (!hasGold && otherCollider.gameObject.tag.Equals("gold_pile")){
            if (secondsGrabbingGold >= timeToGrabGold)
            {
                secondsGrabbingGold = 0f;
                heldTreasure = TreasureController.instance.TakeTreasure(otherCollider.gameObject);
                hasGold = true;
            }
            else
            {
                //not enough time picking up the gold
                secondsGrabbingGold += Time.deltaTime;
            }
        }

       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Exit arena and dispose of gold
        if (hasGold && collision.gameObject.tag.Equals("door"))
        {
            TreasureController.instance.DestoryTreasure(heldTreasure);
            SpawnManager.instance.NotifyEnemyDead();
            Destroy(this.gameObject);
        }
    }



    // Only works against the IntangibleWallTrigger layer
    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void seekingGoldBehavior()
    {

    }

    void gatheringGoldBehavior()
    {

    }

    void leavingWithGoldBehavior()
    {

    }

    void strugglingBehavior()
    {

    }

    void bumpedBehavior()
    {

    }

    void damagedBehavior()
    {

    }

    void dyingBehavior()
    {

    }

    void beingFlungBehavior()
    {

    }
}
