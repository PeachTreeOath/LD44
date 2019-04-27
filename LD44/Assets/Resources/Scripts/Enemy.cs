using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private GameObject target;
    private float Range = 10f;
    public float speed = 10f;


    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("gold_pile");
    }

    // Update is called once per frame
    void Update()
    {
        //Range = Vector2.Distance(transform.position, target.transform.position);
        Vector2 direction = new Vector2((transform.position.x - target.transform.position.x), (transform.position.y - target.transform.position.y));
        Vector2 directionNormalized = direction.normalized;
        Vector2 velocity = new Vector2(directionNormalized.x * speed, directionNormalized.y * speed);
        GetComponent<Rigidbody2D>().velocity = -velocity;
    }


}
