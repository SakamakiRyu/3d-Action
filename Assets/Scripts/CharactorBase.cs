﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorBase : MonoBehaviour
{
    /// <summary>最大HP</summary>
    [SerializeField]
    protected int m_maxHp;

    /// <summary>現在のHP</summary>
    public int CurrentHp { get; protected set; }

    /// <summary>移動速度</summary>
    [SerializeField]
    protected int m_moveSpeed = default;

    /// <summary>攻撃力</summary>
    [SerializeField]
    protected int m_atk = default;

    ///<summary>動く</summary>
    public abstract void OnMove();

    /// <summary>ダメージを受ける</summary>
    public virtual void Damaged(int damage)
    {
        CurrentHp -= damage;
        if(IsArive());
    }

    /// <summary>
    /// 自身の攻撃力を送る
    /// </summary>
    public virtual int SendAtkPower()
    {
        int totalAtkPower = 0;
        return totalAtkPower;
    }

    /// <summary>自身の防御力を送る</summary>
    /// <returns></returns>
    public virtual int SendDefPower()
    {
        int totalDefPower = 0;
        return totalDefPower;
    }

    /// <summary>生きているかの判定</summary>
    /// <returns>判定結果</returns>
    bool IsArive()
    {
        if (CurrentHp <= 0)
        {
            return false;
        }
        return true;
    }
}
