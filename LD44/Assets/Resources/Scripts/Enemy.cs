using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private GameObject target;
    private float Range = 10f;
    public float Speed = 10f;


    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("gold_pile");
    }

    // Update is called once per frame
    void Update()
    {
        //Range = Vector2.Distance(transform.position, target.transform.position);
        Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x) * Speed, (transform.position.y - target.transform.position.y) * Speed);
        GetComponent<Rigidbody2D>().velocity = -velocity;
    }


}
