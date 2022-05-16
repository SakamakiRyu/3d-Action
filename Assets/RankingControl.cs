using UnityEngine;
using UnityEngine.UI;

public class RankingControl : MonoBehaviour
{
    [SerializeField]
    private Text _rankingListText;

    private void Start()
    {
        ClearRankingList();
    }

    /// <summary>
    /// ランキングリストの更新をする
    /// </summary>
    public void UpdateRankingList()
    {
        ClearRankingList();

        var list = TimerControl.GetTimers;

        if (list.Count != 0)
        {
            for (int i = 1; i <= list.Count; i++)
            {
                _rankingListText.text += $"{i}位 : {list[i - 1]}\n";
            }
        }
        else
        {
            _rankingListText.text = "記録なし";
        }
    }

    private void ClearRankingList()
    {
        _rankingListText.text = "";
    }
}
