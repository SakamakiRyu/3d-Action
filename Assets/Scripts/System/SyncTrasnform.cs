using UnityEngine;

public class SyncTrasnform : MonoBehaviour
{
    [SerializeField, Header("同期する対象の座標")]
    Transform m_target = default;

    void Update()
    {
        if (m_target)
        {
            Vector3 v3 = new Vector3(m_target.transform.position.x, this.transform.position.y, m_target.transform.position.z);
            this.transform.position = v3;
        }
    }
}
