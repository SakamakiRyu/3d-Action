using UnityEngine;

/// <summary>敵クラス</summary>
public class Enemy : CharactorBase
{
    /// <summary>系統</summary>   
    [SerializeField, Header("　　　　能力値パラメータ")]
    Breed m_breed = null;
    public int MaxHP => m_breed.MaxHp;

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override int SendAtkPower()
    {
        throw new System.NotImplementedException();
    }
}