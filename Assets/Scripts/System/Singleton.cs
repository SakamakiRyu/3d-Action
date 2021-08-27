using UnityEngine;

/// <summary>継承先のクラスをシングルトン化をするクラス</summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; } = null;

    /// <summary>DestroyをGameObjectにするか</summary>
    protected virtual bool m_DestroyTargetGameObject => false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            Instance.Init();
            return;
        }

        if (m_DestroyTargetGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>継承先でAwakeのタイミングで処理したい場合に利用するメソッド</summary>
    public virtual void Init()
    {

    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>継承先でOnDestroyのタイミングで処理したい場合に利用するメソッド</summary>
    public virtual void OnRelease()
    {

    }
}
