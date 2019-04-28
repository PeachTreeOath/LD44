﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    public float moveSpeed;

    private Rigidbody2D body;
    [HideInInspector] public LineRenderer tongueLine;
    private Vector3 mousePosition;
    [HideInInspector] public TongueTip tip;
    public SpriteRenderer spriteRenderer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
        tongueLine = GetComponentInChildren<LineRenderer>();
        tip = GetComponentInChildren<TongueTip>();
        anim = GetComponent<Animator>();
        tongueLine.startWidth = 0.5f;
        tongueLine.endWidth = tongueLine.startWidth;
        tongueLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!tip.isPlayerTongueing)
        // {
        // Movement
        float moveDistance = moveSpeed * Time.deltaTime;
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // If moving diagonally, make sure to account for reduced speed
        if (hAxis != 0 && vAxis != 0)
            moveDistance *= 0.7071f;
        switch (hAxis)
        {
            case 1:
            spriteRenderer.flipX = false;
            anim.SetBool("isLeft",false);
            break;
            case -1:
            spriteRenderer.flipX = true;
            anim.SetBool("isLeft",true);
            break;
        }
        if (hAxis != 0 || vAxis != 0)
        {
            anim.SetBool("isMoving",true);
        }
        else
        {
            anim.SetBool("isMoving",false);
        }

        body.MovePosition(transform.position + new Vector3(hAxis * moveDistance, vAxis * moveDistance, 0));

        // Firing
        if (!tip.isPlayerTongueing)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireTongue();
            }
        }
        else
        {
            //tongueLine.positionCount = 2;
            //tongueLine.SetPosition(0, transform.position);
            //tongueLine.SetPosition(1, tip.transform.position);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            ReleaseTonguedEnemy();
        }
    }

    private Vector3? GetCurrentMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);

        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);

        }

        return null;
    }

    private void FireTongue()
    {
        spriteRenderer.sprite = ResourceLoader.instance.mimicOpenedSprite;

        //TODO: Get the tongue to shoot out (I suggest using tongueLine)
        // 1. tongue goes towards mouse position
        tip.GetComponent<SpriteRenderer>().enabled = true;
        tongueLine.enabled = true;
        mousePosition = (Vector3)GetCurrentMousePosition();
        tip.SetTargetPosition(mousePosition);
        // 2. tongue moves out and back if it misses
        tip.isPlayerTongueing = true;
        tip.isTongueExtending = true;
        // 3. if the tongue tip touches an enemy, call something to start the fishing minigame (this can be a blank method for now)
    }

    private void ReleaseTonguedEnemy()
    {
        Debug.Log("releasetonguedenemy");

        tip.ReleaseEnemy();
    }
}
