using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 0.5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
