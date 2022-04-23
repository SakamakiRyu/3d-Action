using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] 
    PlayerInput m_pInput;

    [SerializeField] 
    CinemachineVirtualCamera m_lockonCam;

    [SerializeField] 
    CinemachineFreeLook m_freeCam;

    [SerializeField] 
    Transform m_followTransform;

    [SerializeField] 
    Image m_crosshairImage = default;

    [SerializeField] 
    float m_radius = default;

    SphereCollider m_coll;
    InputAction m_chenge, m_chengeUP, m_chengeDown;
    List<EnemyController> m_targetList = new List<EnemyController>();

    /// <summary>現在のカメラタイプ</summary>
    CameraType m_currentCamType = CameraType.FreeLookCamera;
    /// <summary>ロックオンしているm_targetListの要素番号</summary>
    int m_targetIndex = 0;
    /// <summary>ロックオンしている敵のID</summary>
    int m_targetID;

    private void Start()
    {
        if (!m_pInput)
        {
            Debug.LogError($"{gameObject.name}にPlayerInputがアサインされていません");
        }
        else
        {
            m_chenge = m_pInput.currentActionMap["ChengeCamera"];
            m_chengeUP = m_pInput.currentActionMap["R1"];
            m_chengeDown = m_pInput.currentActionMap["L1"];
        }
    }

    private void Update()
    {
        //　カメラコントローラーの座標をFollowと同期する
        this.transform.position = m_followTransform.position;
        ControlCamera();
    }

    /// <summary>カメラを切り替える</summary>
    void ChengeCamera()
    {
        switch (m_currentCamType)
        {
            case CameraType.FreeLookCamera:
                m_crosshairImage.enabled = true;
                m_lockonCam.Priority = m_freeCam.Priority + 1;
                m_freeCam.Priority = 0;
                m_lockonCam.Priority = 1;
                m_lockonCam.enabled = true;
                m_freeCam.enabled = false;
                m_currentCamType = CameraType.LockonCamera;
                break;
            case CameraType.LockonCamera:
                m_crosshairImage.enabled = false;
                m_targetIndex = 0;
                m_targetID = 0;
                SortingTarget();
                CinemachineVirtualCameraBase oldCamera = m_lockonCam;
                m_freeCam.Priority = m_lockonCam.Priority + 1;
                m_lockonCam.Priority = 0;
                m_freeCam.Priority = 1;
                m_lockonCam.enabled = false;
                m_freeCam.enabled = true;
                m_freeCam.ChangeToFreeLook(oldCamera);
                m_currentCamType = CameraType.FreeLookCamera;
                break;
            default:
                break;
        }
    }

    /// <summary>カメラの機能制御</summary>
    void ControlCamera()
    {
        switch (m_currentCamType)
        {
            case CameraType.FreeLookCamera:
                // ロックオン対象が存在する時にカメラ変更のボタンが押されたらカメラを切り替える
                if (m_targetList.Count > 0 && m_chenge.triggered)
                {
                    SetTarget();
                    ChengeCamera();
                }
                break;
            case CameraType.LockonCamera:
                // ロックオンできる敵が居なくなる、またはロックオン対象が死んだ場合、
                // またはカメラ変更ボタンが押された時にカメラを切り替える。
                if (m_targetList.Count == 0 || m_targetList[m_targetIndex].IsDead || m_chenge.triggered)
                {
                    ChengeCamera();
                }

                if (m_targetList.Count != 0)
                {
                    // ロックオンカメラ(VirtualCamera)のFollowに設定しているオブジェクトの正面をターゲットに向ける
                    m_followTransform.LookAt(m_targetList[m_targetIndex].transform);
                }

                // ロックオン可能な対象が二つ以上の時にロックオンの対象を切り替える
                if (m_targetList.Count > 1)
                {
                    if (m_chengeUP.triggered)
                    {
                        m_targetIndex = m_targetIndex == m_targetList.Count - 1 ? 0 : ++m_targetIndex;
                        SetTarget();
                    }
                    if (m_chengeDown.triggered)
                    {
                        m_targetIndex = m_targetIndex == 0 ? m_targetList.Count - 1 : --m_targetIndex;
                        SetTarget();
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>m_targetListの整理</summary>
    void SortingTarget()
    {
        m_targetList = m_targetList.Where(x => !x.IsDead).ToList();
    }

    /// <summary>TargetCamera(VirtualCamera)のLookAtにロックオン対象を設定し、m_targetIDを上書きする</summary>
    void SetTarget()
    {
        m_lockonCam.LookAt = m_targetList[m_targetIndex].transform;
        m_targetID = m_targetList[m_targetIndex].GetInstanceID();
    }

    public void AddTarget(EnemyController enemy)
    {
        m_targetList.Add(enemy);
    }

    public void RemoveTarget(EnemyController enemy)
    {
        m_targetList.Remove(enemy);
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Enemy"))
        {
            AddTarget(target.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit(Collider target)
    {
        if (target.CompareTag("Enemy"))
        {
            // ロックオン範囲外に出た敵がロックオン対象だった場合、自動的にカメラを切り替える
            if (target.GetInstanceID() == m_targetID)
            {
                ChengeCamera();
            }
            RemoveTarget(target.GetComponent<EnemyController>());
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!m_coll)
        {
            m_coll = GetComponent<SphereCollider>();
        }
        m_coll.radius = m_radius;
        if (m_freeCam && m_followTransform)
        {
            m_freeCam.Follow = m_followTransform;
            m_freeCam.LookAt = m_followTransform;
        }
        if (m_lockonCam && m_followTransform)
        {
            m_freeCam.Follow = m_followTransform;
        }
    }
#endif
}