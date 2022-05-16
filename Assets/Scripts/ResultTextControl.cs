using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ResultScene��Text�𑀍삷��R���|�[�l���g
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

        // �w�肵�������_�ȉ��؂�̂�
        var text = System.Math.Floor(clearTime * System.Math.Pow(10, 2)) / System.Math.Pow(10, 2);

        _text.text = $"�N���A�^�C�� : {text}";
    }
}
