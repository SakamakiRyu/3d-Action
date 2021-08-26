using UnityEngine;

public class Test : MonoBehaviour
{
    /// <summary>剣の向きを設定する</summary>
    [SerializeField, Header("剣の向きを設定する")]
    Quaternion m_swordRotation = default;

    private void OnValidate()
    {
        transform.rotation = m_swordRotation;
    }
}
