using UnityEngine;

/// <summary>
/// ‰¹‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    public enum BGMType
    {
        Title,
        InGame
    }

    public enum SEType
    {
        Sword = 0,
        EnemyFootStep,
        PlayerFootStep
    }

    [SerializeField]
    AudioSource _bgmSource = default;

    [SerializeField]
    AudioSource _seSource = default;

    [SerializeField]
    AudioClip[] _bgmClips = default;

    [SerializeField]
    AudioClip[] _seClips = default;

    /// <summary>BGM‚ğ•Ï‚¦‚é</summary>
    /// <param name="type"></param>
    public void ChengeBGM(BGMType type)
    {
        _bgmSource.clip = _bgmClips[(int)type];
    }

    /// <summary>SE‚ğ‚È‚ç‚·</summary>
    /// <param name="type">–Â‚ç‚·‰¹</param>
    public void PlaySE(SEType type)
    {
        _seSource.PlayOneShot(_seClips[(int)type]);
    }
}
