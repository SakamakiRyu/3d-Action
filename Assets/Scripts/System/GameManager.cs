using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameState m_state { get; set; } = GameState.Title;

    public enum GameState
    {
        Title,
        Game,
    }

    private void Update()
    {
        if (m_state == GameState.Title)
        {
            SceneManager.LoadScene("Game");
        }
    }

   
}