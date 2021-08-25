using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>ロックオンカメラ機能</summary>
public class LockonCamera : MonoBehaviour
{
    /// <summary>VirtualCamera</summary>
    [SerializeField]
    CinemachineVirtualCamera m_virtualCam;

    /// <summary>FreeLookCam</summary>
    [SerializeField]
    CinemachineFreeLook m_freeCam;

    /// <summary>CameraのFollow対象</summary>
    [SerializeField]
    Transform m_followTrasform;

    /// <summary>ロックオン対象</summary>
    [SerializeField, Tooltip("ロックオン対象")]
    Transform m_lockonTarget = null;

    InputAction m_chenge;

    CameraType m_currentCam = CameraType.FreeLookCamera;

    private void Start()
    {
        m_chenge = GetComponent<PlayerInput>().currentActionMap["Chenge"];
    }

    private void Update()
    {
        LookAtTarget();
    }

    void LookAtTarget()
    {
        if (!m_lockonTarget) return;
        {
            m_followTrasform.LookAt(m_lockonTarget);
            if (m_chenge.triggered)
            {
                Debug.Log("A");
                ChengeCamera();
            }
        }
    }

    /// <summary>カメラを切り替える</summary>
    public void ChengeCamera()
    {
        switch (m_currentCam)
        {
            case CameraType.FreeLookCamera:
                m_virtualCam.Priority = m_freeCam.Priority + 1;
                m_freeCam.Priority = 0;
                m_virtualCam.Priority = 1;
                m_currentCam = CameraType.LockonCamera;
                break;
            case CameraType.LockonCamera:
                m_freeCam.Priority = m_virtualCam.Priority + 1;
                m_virtualCam.Priority = 0;
                m_freeCam.Priority = 1;
                m_currentCam = CameraType.FreeLookCamera;
                break;
            default:
                break;
        }
    }

    /// <summary>カメラタイプ</summary>
    enum CameraType
    {
        /// <summary>フリールック</summary>
        FreeLookCamera,
        /// <summary>ロックオン</summary>
        LockonCamera
    }
}
