using UnityEngine;

/// <summary>
/// âπÇÃä«óùÉNÉâÉX
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

    /// <summary>BGMÇïœÇ¶ÇÈ</summary>
    /// <param name="type"></param>
    public void ChengeBGM(BGMType type)
    {
        _bgmSource.clip = _bgmClips[(int)type];
        _bgmSource.Play();
    }

    /// <summary>SEÇÇ»ÇÁÇ∑</summary>
    /// <param name="type">ñ¬ÇÁÇ∑âπ</param>
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
