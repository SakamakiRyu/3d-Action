/// <summary>
/// �U��(�_���[�W)���󂯎�鎖���ł���悤�ɂȂ�B
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// �_���[�W��^����
    /// </summary>
    /// <param name="damage">�_���[�W</param>
    public void AddDamage(int damage);
}

/// <summary>
/// �Q�[���N���A���ɏ�����
/// </summary>
interface IGame
{
    /// <summary>�C�x���g�ɓo�^����֐�</summary>
    void Register();
    /// <summary>�Q�[���N���A���ɌĂ΂��֐�</summary>
    void GameClear();
}