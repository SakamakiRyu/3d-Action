using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyParameter : ScriptableObject
{
    [SerializeField]
    private string _Name;

    [SerializeField]
    private int _MaxHP;

    [SerializeField]
    private int _AttackPower;

    [SerializeField]
    private int _BlockPower;

    public string Name => _Name;
    public int MaxHP => _MaxHP;
    public int AttackPower => _AttackPower;
    public int BlockPower => _BlockPower;
}
