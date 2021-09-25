using UnityEngine;

/// <summary>
/// AnimationEventでサウンドを鳴らす為のクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField]
    AudioClip m_clip;

    AudioSource m_source;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        if (m_clip)
        {
            m_source.PlayOneShot(m_clip);
        }
    }

    public void OnPlaySound(AudioClip clip)
    {
        m_source.PlayOneShot(clip);
    }
}
