using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Attack : MonoBehaviour
{
    [SerializeField] ParticleSystem m_hitEffect = default;
    [SerializeField] AudioClip m_hitSE = default;
    AudioSource m_source;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_hitEffect.Play();
            m_source.PlayOneShot(m_hitSE);
            other.GetComponent<EnemyController>().GetDamage();
        }
        if (other.CompareTag("Player"))
        {
            m_source.PlayOneShot(m_hitSE);
            other.GetComponent<PlayerController>().GetDamage();
        }
    }
}