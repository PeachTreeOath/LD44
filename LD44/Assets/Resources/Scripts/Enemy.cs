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
        target = FindClosestGoldPile();
    }

    // Update is called once per frame
    void Update()
    {

        MoveTowardClosestGoldPile();

    }

    private void MoveTowardClosestGoldPile()
    {
        if (target == null)
        {
            target = FindClosestGoldPile();
        }
        else
        {

            Vector2 direction = new Vector2((transform.position.x - target.transform.position.x), (transform.position.y - target.transform.position.y));
            Vector2 directionNormalized = direction.normalized;
            Vector2 velocity = new Vector2(directionNormalized.x * speed, directionNormalized.y * speed);
            GetComponent<Rigidbody2D>().velocity = -velocity;
        }
    }

    private GameObject FindClosestGoldPile()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("gold_pile");
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


}
