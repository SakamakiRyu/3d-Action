using UnityEngine;

[CreateAssetMenu(fileName = "Date", menuName = "Quest")]
public class QuestDate : MonoBehaviour
{
    [SerializeField]
    private int _Count;

    public int GetCount => _Count;
}
