/// <summary>
/// �U��(�_���[�W)���󂯎�鎖���ł���悤�ɂȂ�B
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// �_���[�W��^����
    /// </summary>
    public void AddDamage();
}

/// <summary>
/// �Q�[���N���A���ɏ�����
/// </summary>
interface IGame
{
    /// <summary>�C�x���g�ɓo�^����֐�</summary>
    void Subscribe();
    /// <summary>�Q�[���N���A���ɌĂ΂��֐�</summary>
    void GameClear();
}