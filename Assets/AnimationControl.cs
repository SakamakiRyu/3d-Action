using UnityEngine;

/// <summary>
/// アニメーションの管理クラス
/// </summary>
public class AnimationControl : MonoBehaviour
{
    [SerializeField]
    private Animator _Animator;

    private void Start()
    {
        if (_Animator is null) Debug.LogError("Animator is null !");
    }

    /// <summary>
    /// アニメーションの変更をする
    /// </summary>
    /// <param name="name">ステート名</param>
    /// <param name="duration">何秒後にアニメーションを切り替えるか</param>
    public void ChengeAnimation(string name, float duration = 0.0f)
    {
        if (_Animator is null) return;

        _Animator.CrossFade(name, duration);
    }

    /// <summary>
    /// アニメーションの再生速度を変更する
    /// </summary>
    /// <param name="time">再生速度</param>
    public void ChengeAnimationSpeed(float time)
    {
        if (_Animator is null) return;

        _Animator.speed = time;
    }
}
