using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGrabber : MonoBehaviour
{


    public ThrowableObject obj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /* 
                    Debug.Log("EJOFOIEWJF");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                ThrowableObject throwable = hit.collider.GetComponent<ThrowableObject>();
                if (throwable)
                {
                    throwable.StartGrab();
                }
            }
            */
            obj.StartGrab();
        }
        if (Input.GetMouseButtonUp(0))
        {
            obj.ReleaseGrab();

        }

    }
}
