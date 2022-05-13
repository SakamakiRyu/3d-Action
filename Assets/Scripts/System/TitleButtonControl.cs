using UnityEngine;

public class TitleButtonControl : MonoBehaviour
{
    /// <summary>
    /// インゲームシーンをロードする
    /// </summary>
    public void LoadToInGame()
    {
        GameManager.Instance.LoadToInGameScene();
    }
}
