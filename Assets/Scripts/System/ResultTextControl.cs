using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ResultSceneのTextを操作するコンポーネント
/// </summary>
public class ResultTextControl : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private void Start()
    {
        ToStringTime();
    }

    public void ToStringTime()
    {
        var clearTime = TimerControl.GetTimer;

        if (MissionControl.IsCleared)
        {
            // 指定した小数点以下切り捨て
            var text = System.Math.Floor(clearTime * System.Math.Pow(10, 2)) / System.Math.Pow(10, 2);

            _text.text = $"クリアタイム : {text}";
        }
        else
        {
            _text.text = "ミッション失敗...";
        }
    }
}
