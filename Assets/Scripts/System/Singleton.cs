using UnityEngine;

/// <summary>継承先のクラスをシングルトン化をするクラス</summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = (T)FindObjectOfType(typeof(T));

                if (!_Instance)
                {
                    Debug.LogError(typeof(T) + "がシーンに存在しません。");
                }
            }
            return _Instance;
        }
    }


    public virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}