using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵クラス
/// </summary>
public class Enemy : CharactorBase
{
    /// <summary>系統</summary>
    public Breed Breed => m_breed;
    [SerializeField] Breed m_breed = null;

    public override void OnMove()
    {
        throw new System.NotImplementedException();
    }
}