using UnityEngine;

/// <summary>
/// 指定したオブジェクトとPositionを同期する
/// </summary>
public class SyncTrasnform : MonoBehaviour
{
    [SerializeField]
    [Header("同期する対象の座標")]
    private Transform _target = default;

    private void Update()
    {
        if (_target)
        {
            var v3 = new Vector3(_target.transform.position.x, this.transform.position.y, _target.transform.position.z);

            this.transform.position = v3;
        }
    }
}
