using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingControl : MonoBehaviour
{
    [SerializeField]
    private Text _rankingText;

    public void UpdateRankingList()
    {
        var list = TimerControl.GetTimers;
        foreach (var item in list)
        {
            _rankingText.text += $"{item}\n";
        }
    }
}
