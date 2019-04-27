using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStarted = false;
    public bool startScreen = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            //TODO: fill with game business logic (check enemy's for win/wave?, check gold for loss, spawn enemies, etc.)
        }
        else if (startScreen)
        {
            //TODO: show start screen where player has to eat start button
        }
        else
        {
            //TODO: any win/loss logic
        }
    }
}
