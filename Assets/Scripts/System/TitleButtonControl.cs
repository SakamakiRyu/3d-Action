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

    public void ShowRanking()
    {
        var times = TimerControl.GetTimers;
        foreach (var item in times)
        {
            Debug.Log(item);
        }
    }
}
