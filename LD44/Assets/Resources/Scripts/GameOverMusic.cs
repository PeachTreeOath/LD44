using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.instance.PlaySound("Game_Over");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
