using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーにパラメーターを付与するクラス
/// </summary>
public class PlayerParameter : MonoBehaviour
{
    #region Define
    public enum State
    {
        None,
        Arive,
        Death
    }
    #endregion

    #region Field
    [SerializeField]
    private int _maxHP;

    [SerializeField]
    private PlayerUIController _uiCtrl;

    public int CurrentHP { get; private set; }

    public State CurrentState { get; private set; }
    #endregion

    #region Unity Function
    private void Awake()
    {
        CurrentHP = _maxHP;
    }

    private void Start()
    {
        ChengeState(State.Arive);
    }

    private void Update()
    {
        StateUpdate();
    }
    #endregion

    #region Private Fucntion
    /// <summary>
    /// ステートの変更をする
    /// </summary>
    private void ChengeState(State next)
    {
        switch (next)
        {
            case State.None:
                { }
                break;
            case State.Arive:
                { }
                break;
            case State.Death:
                {
                    GameManager.Instance.GameEndRequest();
                }
                break;
        }

        CurrentState = next;
    }

    /// <summary>
    /// ステート毎に毎フレーム呼ばれる処理
    /// </summary>
    private void StateUpdate()
    {
        switch (CurrentState)
        {
            case State.None:
                { }
                break;
            case State.Arive:
                { }
                break;
            case State.Death:
                { }
                break;
        }
    }
    #endregion

    #region Public Fucntion
    /// <summary>
    /// 体力を減らす
    /// </summary>
    public void ReduceHP()
    {
        CurrentHP--;

        if (_uiCtrl)
        {
            // HPゲージの更新
            _uiCtrl.UpdateHPGauge(CurrentHP, _maxHP);
        }

        if (CurrentHP <= 0)
        {
            ChengeState(State.Death);
            return;
        }
    }
    #endregion
}
