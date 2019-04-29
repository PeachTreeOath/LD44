using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainGame, // 0
    GameOver, // 1
    Other     // 2
}

public class GameManager : Singleton<GameManager>
{
    //public bool gameStarted = false;
    //public bool startScreen = true;

    // Dictates the state of the game
    public GameState CurrentState
    { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = GameState.Other;
    }

    // Update is called once per frame
    void Update()
    {
        // --DEPRECATED--

        //if (gameStarted)
        //{
        //    //TODO: fill with game business logic (check enemy's for win/wave?, check gold for loss, spawn enemies, etc.)
        //}
        //else if (startScreen)
        //{
        //    //TODO: show start screen where player has to eat start button
        //}
        //else
        //{
        //    //TODO: any win/loss logic
        //}


    }

    public void AssessTreasureCount(int inInt)
    {
        if (inInt < 1)
        {
            // If there's no money left on the field,
            // sets game state to Game Over and calls UI Text Manager
            // to turn on the GameOver UI panel.
            CurrentState = GameState.GameOver;
            //UITextManager.instance.OnGameOver();
            SceneManager.LoadScene("GameOverScreen");
        }
        else
        {
            CurrentState = GameState.MainGame;
        }
    }
}
