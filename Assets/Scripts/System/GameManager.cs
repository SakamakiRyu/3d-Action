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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}