using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject target;
    private string targetTag;
    private float goldGatherRange;
    public float walkSpeed;
    public float timeToRecalcTarget = 3f;
    public float timeInStun;
    public float timeInCorpse = 1f;

    public float throwSpeed;
    public float moveTowardsSpeed;

    public float speedToRecoverFromThrow = 1f;

    public GameObject hasMoneyIcon;
    public GameObject isStunnedIcon;

    private bool isGrabbed;
    private bool isThrown;
    private bool isStunned;
    private bool isCorpse;
    [HideInInspector] public Rigidbody2D body;
    private Queue<Vector3> previousMovementVectors = new Queue<Vector3>();
    private Queue<float> previousVelocities = new Queue<float>();
    private Vector3 previousPosition;

    private bool hasGold = false;
    private float secondsGrabbingGold = 0f;
    private float secondsMovingToTarget = 0f;
    private float secondsStunned = 0f;
    private float secondsAsCorpse = 0f;
    private GameObject heldTreasure = null;
    private Animator anim;

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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private EnemyState enemyState;
    private int timeInStatus, goldCarried;

    //enemy type attributes
    private float mass, maxGoldCapacity, stunTime, goldGatherTime;

    //temporary constructor; remove when enemy is made abstract
    public Enemy()
    {
        mass = 2;
        walkSpeed = 10f;
        maxGoldCapacity = 2;
        timeInStun = 2;
        goldGatherRange = 1;
        goldGatherTime = 3;
    }

    public Enemy(float mass, float speed, int maxGoldCapacity, float timeInStun, float goldGatherRange, float goldGatherTime)
    {
        this.mass = mass;
        this.walkSpeed = speed;
        this.maxGoldCapacity = maxGoldCapacity;
        this.timeInStun = timeInStun;
        this.goldGatherRange = goldGatherRange;
        this.goldGatherTime = goldGatherTime;
    }

    public float minimumCollisionVelocityForDeath;

    // Use this for initialization
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
        previousPosition = body.position;

        // The Money icon should be off by default
        hasMoneyIcon.SetActive(false);
        isStunnedIcon.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isCorpse)
        {
            secondsAsCorpse += Time.deltaTime;
            if (secondsAsCorpse >= timeInCorpse)
            {
                Die();
            }
        }
        else if (isGrabbed)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), moveTowardsSpeed * Time.deltaTime);
            body.position = newPos;
        }
        else if (isThrown)
        {
            if (body.velocity.magnitude <= speedToRecoverFromThrow)
            {
                isStunned = true;
                isThrown = false;
                secondsStunned = 0f;
                AudioManager.instance.PlaySound("Oof_Light");
            }
        }
        else if (isStunned)
        {
            anim.speed = 0;
            secondsStunned += Time.deltaTime;
            if (secondsStunned >= timeInStun)
            {
                anim.speed = 1;
                isStunned = false;
                isStunnedIcon.SetActive(false);
            }
            else
            {
                isStunnedIcon.SetActive(true);
            }
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

            // If this unit has gold, turn on the hasMoneyIcon
            hasMoneyIcon.SetActive(true);
        }
    }

    //Bundles all of the states where AI behavior needs to be disabled.
    private bool isInThrowSequence()
    {
        return isThrown || isGrabbed || isStunned || isCorpse;
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
        previousMovementVectors.Enqueue(body.position - (Vector2)previousPosition);
        previousVelocities.Enqueue(body.velocity.magnitude);

        if (previousMovementVectors.Count > 5)
            previousMovementVectors.Dequeue();

        if (previousVelocities.Count > 5)
            previousVelocities.Dequeue();

        previousPosition = body.position;
    }

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void ReleaseGrab()
    {
        //        Debug.Log("releaseGrab");

        isGrabbed = false;
        isThrown = true;

        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousMovementVectors)
        {
            totalVelocity += velocity;
        }

        if (previousMovementVectors.Count != 0)
            body.velocity = GetPastAverageVelocity() * throwSpeed;

        AudioManager.instance.PlaySound("Whoosh");
    }

    public Vector3 GetPastAverageVelocity()
    {
        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousMovementVectors)
        {
            totalVelocity += velocity;
        }

        if (previousMovementVectors.Count != 0)
            totalVelocity = totalVelocity / previousMovementVectors.Count;

        return totalVelocity;
    }

    public float GetPastHighestVelocity()
    {
        float highestVelocity = 0;

        foreach (float velocity in previousVelocities)
        {
            highestVelocity = Mathf.Max(highestVelocity, velocity);
        }

        return highestVelocity;
    }

    /*
        public void ReleaseGrab()
    {
        //        Debug.Log("releaseGrab");

        isGrabbed = false;
        isThrown = true;

        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousMovementVectors)
        {
            totalVelocity += velocity;
        }

        if (previousMovementVectors.Count != 0)
            body.velocity = GetPastAverageVelocity() * throwSpeed;

        AudioManager.instance.PlaySound("Whoosh");
    }

    public Vector3 GetPastAverageVelocity()
    {
        Vector3 totalVelocity = Vector3.zero;

        foreach (Vector3 velocity in previousMovementVectors)
        {
            totalVelocity += velocity;
        }

        if (previousMovementVectors.Count != 0)
            totalVelocity = totalVelocity / previousMovementVectors.Count;

        return totalVelocity;
    }

    public float GetPastHighestVelocity()
    {
        float highestVelocity = 0;

        foreach (Vector3 velocity in previousMovementVectors)
        {
            highestVelocity = Mathf.Max(highestVelocity, velocity.magnitude);
        }

        return highestVelocity;
    }
    */


    public Vector2 GetEnemyPosition()
    {
        return body.position;
    }

    public void OnTriggerStay2D(Collider2D otherCollider)
    {
        //Start picking up gold
        if (otherCollider.gameObject.tag.Equals("gold_pile") && !isInThrowSequence() && !hasGold)
        {
            if (secondsGrabbingGold >= goldGatherTime)
            {
                secondsGrabbingGold = 0f;
                heldTreasure = TreasureController.instance.TakeTreasure(otherCollider.gameObject);
                hasGold = true;
                AudioManager.instance.PlaySound("coins");

            }
            else
            {
                //not enough time picking up the gold
                secondsGrabbingGold += Time.deltaTime;
            }
        }

        //Exit arena and dispose of gold
        if (otherCollider.gameObject.tag.Equals("door") && !isInThrowSequence() && hasGold)
        {
            TreasureController.instance.DestoryTreasure(heldTreasure);
            hasGold = false;
            Die();
            AudioManager.instance.PlaySound("Gold_Escape");
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Enemy otherEnemy = other.gameObject.GetComponent<Enemy>();
        if (otherEnemy)
        {
            //Vector3 newVelocity = body.velocity - otherEnemy.body.velocity;
            //Vector3 newVelocity = GetPastAverageVelocity() - otherEnemy.GetPastAverageVelocity();
            float newVelocity = GetPastHighestVelocity() + otherEnemy.GetPastHighestVelocity();
            //Debug.LogWarning("ENEMY TO ENEMY COLLISION AT " + newVelocity.magnitude);
            //Debug.LogWarning("ENEMY TO ENEMY COLLISION AT " + newVelocity);
            //if (newVelocity.magnitude > minimumCollisionVelocityForDeath)
            if (newVelocity > minimumCollisionVelocityForDeath)
            {
                isCorpse = true;
                GameObject blood = Instantiate(ResourceLoader.instance.bloodParticles);
                blood.transform.position = transform.position;
                GameObject bloodSprite = Instantiate(ResourceLoader.instance.bloodSprite);
                bloodSprite.transform.position = transform.position;
                CameraShake.instance.trauma += 0.35f;
                AudioManager.instance.PlaySound("Thud");
            }
        }
        else
        {
            //Debug.LogWarning("OTHER COLLISION AT " + body.velocity.magnitude);
            //if (GetPastAverageVelocity().magnitude > minimumCollisionVelocityForDeath)
           // Debug.LogWarning("OTHER COLLISION AT " + GetPastHighestVelocity());
            if (GetPastHighestVelocity() > minimumCollisionVelocityForDeath)
            {
                Die();
                GameObject blood = Instantiate(ResourceLoader.instance.bloodParticles);
                blood.transform.position = transform.position;
                GameObject bloodSprite = Instantiate(ResourceLoader.instance.bloodSprite);
                bloodSprite.transform.position = transform.position;
                CameraShake.instance.trauma += 0.7f;
                AudioManager.instance.PlaySound("thud2");
            }
        }
    }

    private void Die()
    {
        PlayerController.instance.tip.ReleaseEnemy();
        SpawnManager.instance.NotifyEnemyDead();

        if (hasGold)
        {
            TreasureController.instance.DropTreasure(heldTreasure, transform.position);
          //  Debug.Log("Dropping treasure.");
            AudioManager.instance.PlaySound("coins");
        }
        Destroy(gameObject);
    }

    // Only works against the IntangibleWallTrigger layer
    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
