using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{

    public float throwSpeed;
    public float moveTowardsSpeed;

    private bool isGrabbed;
    private Rigidbody2D body;
    private Queue<Vector3> previousPositions = new Queue<Vector3>();
    private Vector3 previousPosition;

    // Start is called before the first frame update
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
