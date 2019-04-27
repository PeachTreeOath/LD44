using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // If moving diagonally, make sure to account for reduced speed
        if (hAxis != 0 && vAxis != 0)
            moveDistance *= 0.7071f;

        body.MovePosition(transform.position + new Vector3(hAxis * moveDistance, vAxis * moveDistance, 0));
        //transform.position = body.position;
    }
}
