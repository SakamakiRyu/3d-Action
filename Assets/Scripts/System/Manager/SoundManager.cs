using UnityEngine;

/// <summary>
/// ���̊Ǘ��N���X
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    public enum BGMType
    {
        Title,
        InGame,
        Result
    }

    public enum SEType
    {
        Sword,
        SlimeFootStep,
        PlayerFootStep,
        Damaged,
        Dive
    }

    [SerializeField]
    AudioSource _bgmSource = default;

    [SerializeField]
    AudioSource _seSource = default;

    [SerializeField]
    AudioClip[] _bgmClips = default;

    [SerializeField]
    AudioClip[] _seClips = default;

    /// <summary>BGM��ς���</summary>
    /// <param name="type"></param>
    public void ChengeBGM(BGMType type)
    {
        _bgmSource.clip = _bgmClips[(int)type];
        _bgmSource.Play();
    }

    /// <summary>SE���Ȃ炷</summary>
    /// <param name="type">�炷��</param>
    public void PlaySE(SEType type)
    {
        _seSource.PlayOneShot(_seClips[(int)type]);
    }

    public void ChengeBGMVolume(float value)
    {
        _bgmSource.volume = value;
    }

    public void ChengeSEVolume(float value)
    {
        _seSource.volume = value;
    }
}
