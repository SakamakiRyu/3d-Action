using UnityEngine;

public class ResultButtonControl : MonoBehaviour
{
    /// <summary>
    /// タイトルシーンのロードをする
    /// </summary>
    public void LoadToTitle()
    {
        GameManager.Instance.LoadToTitleScene();
    }
}
