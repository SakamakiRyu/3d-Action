using System.Collections;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public enum EffectType
    {
        /// <summary>ヒットストップ</summary>
        HitStop,
        /// <summary>攻撃のエフェクト</summary>
        HitEffect
    }

    /// <summary>
    /// エフェクトの再生をする
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
    /// ヒットストップ処理
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
