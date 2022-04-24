using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public enum SEType
    {
        Sword1 = 0,
    }

    [SerializeField]
    AudioSource m_bgmSource = default;

    [SerializeField]
    AudioSource m_seSource = default;

    [SerializeField]
    AudioClip[] m_bgmClips = default;

    [SerializeField]
    AudioClip[] m_seClips = default;

    /// <summary>
    /// SEをならす
    /// </summary>
    /// <param name="type">鳴らす音</param>
    public void PlaySE(SEType type)
    {

    }
}
