using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;

    private Rigidbody2D body;
    private LineRenderer tongueLine;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
        tongueLine = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        float moveDistance = moveSpeed * Time.deltaTime;
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // If moving diagonally, make sure to account for reduced speed
        if (hAxis != 0 && vAxis != 0)
            moveDistance *= 0.7071f;

        body.MovePosition(transform.position + new Vector3(hAxis * moveDistance, vAxis * moveDistance, 0));

        // Firing
        if (Input.GetButtonDown("Fire1"))
        {
            FireTongue();
        }
    }

    private void FireTongue()
    {
        //TODO: Get the tongue to shoot out (I suggest using tongueLine)
        // 1. tongue goes towards mouse position
        // 2. tongue moves out and back if it misses
        // 3. if the tongue tip touches an enemy, call something to start the fishing minigame (this can be a blank method for now)
    }
}
