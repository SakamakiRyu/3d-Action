/// <summary>Stateの抽象クラス</summary>
public abstract class PlayerStateBase
{
    /// <summary>ステートを開始した時に呼ばれる</summary>
    public virtual void OnEnter(PlayerStateMachine owner) { }

    /// <summary>毎フレーム呼ばれる</summary>
    public virtual void OnUpdate(PlayerStateMachine owner) { }

    /// <summary>ステートを抜ける際に呼ばれる</summary>
    public virtual void OnExit(PlayerStateMachine owner) { }
}
