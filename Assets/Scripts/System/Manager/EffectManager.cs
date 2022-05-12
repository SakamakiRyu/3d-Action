using System.Collections;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public enum EffectType
    {
        /// <summary>�q�b�g�X�g�b�v</summary>
        HitStop,
        /// <summary>�U���̃G�t�F�N�g</summary>
        HitEffect
    }

    /// <summary>
    /// �G�t�F�N�g�̍Đ�������
    /// </summary>
    public void PlayEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.HitStop:
                {
                    StartCoroutine(HitStopAcync());
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �q�b�g�X�g�b�v����
    /// </summary>
    private IEnumerator HitStopAcync()
    {
        var _Timer = 0f;
        Time.timeScale = 0.1f;
        while (_Timer < 0.4f)
        {
            _Timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;
        yield return null;
    }
}
