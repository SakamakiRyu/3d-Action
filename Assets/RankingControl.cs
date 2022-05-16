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
    /// �����L���O���X�g�̍X�V������
    /// </summary>
    public void UpdateRankingList()
    {
        ClearRankingList();

        var list = TimerControl.GetTimers;

        if (list.Count != 0)
        {
            for (int i = 1; i <= list.Count; i++)
            {
                _rankingListText.text += $"{i}�� : {list[i - 1]}\n";
            }
        }
        else
        {
            _rankingListText.text = "�L�^�Ȃ�";
        }
    }

    private void ClearRankingList()
    {
        _rankingListText.text = "";
    }
}
