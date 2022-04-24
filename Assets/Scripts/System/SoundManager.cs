using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public enum SEType
    {
        Sword1 = 0,
        Sword2 = 1,
    }

    [SerializeField]
    AudioSource m_bgmSource = default;

    [SerializeField]
    AudioSource m_seSource = default;

    [SerializeField]
    AudioClip[] m_seClips = default;

    /// <summary>
    /// SE���Ȃ炷
    /// </summary>
    /// <param name="type">�炷��</param>
    public void PlaySE(SEType type)
    {

    }
}
