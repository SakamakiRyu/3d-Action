using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorBase : MonoBehaviour
{
    /// <summary>
    /// 最大HP
    /// </summary>
    [SerializeField] protected int m_maxHp;

    /// <summary>
    /// 現在のHP
    /// </summary>
    public int CurrentHp { get; protected set; }

    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] protected int m_moveSpeed = default;

    /// <summary>
    /// 攻撃力
    /// </summary>
    [SerializeField] protected int m_atk = default;

    ///<summary>
    ///動く
    /// </summary>
    public abstract void OnMove();

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public virtual void Damaged(int damage)
    {
       CurrentHp -= damage;
    }

    /// <summary>
    /// 自身の攻撃力を送る
    /// </summary>
    public virtual int SendAtkPow()
    {
        int totalAtkPower = 0;
        return totalAtkPower;
    }
}
