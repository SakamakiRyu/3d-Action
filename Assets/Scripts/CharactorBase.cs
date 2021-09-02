using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorBase : MonoBehaviour
{
    /// <summary>現在のHP</summary>
    public int CurrentHp { get; protected set; }

    /// <summary>移動速度</summary>
    [SerializeField]
    protected int m_moveSpeed = default;

    ///<summary>動く</summary>
    public abstract void Move();

    /// <summary>ダメージを受ける</summary>
    public virtual void Damaged(int damage)
    {
        CurrentHp -= damage;
        if (IsArive()) { };
    }

    /// <summary>自身の攻撃力を送る</summary>
    public virtual int SendAtkPower()
    {
        int totalAtkPower = 0;
        return totalAtkPower;
    }

    /// <summary>自身の防御力を送る</summary>
    public virtual int SendDefPower()
    {
        int totalDefPower = 0;
        return totalDefPower;
    }

    /// <summary>現在のHP(CurrentHp)で生死判定をする</summary>
    /// <returns>判定結果 true == 生、false == 死 </returns>
    bool IsArive()
    {
        if (CurrentHp <= 0)
        {
            return false;
        }
        return true;
    }
}
