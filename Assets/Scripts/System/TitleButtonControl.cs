using UnityEngine;

public class TitleButtonControl : MonoBehaviour
{
    /// <summary>
    /// �C���Q�[���V�[�������[�h����
    /// </summary>
    public void LoadToInGame()
    {
        GameManager.Instance.LoadToInGameScene();
    }
}
