using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("参照するPlayerInput")]
    PlayerInput m_pInput;

    [SerializeField, Header("LockonCamera (Virtual)")]
    CinemachineVirtualCamera m_lockonCam;

    [SerializeField, Header("FreeLookCamera")]
    CinemachineFreeLook m_freeCam;

    [SerializeField, Header("CameraのFollow対象(PlayerのFollowする座標)")]
    Transform m_followTransform;

    [SerializeField, Header("現在のカメラタイプを表す画像")]
    Image m_crossHairImage = default;

    InputAction m_chenge, m_L1, m_R1;

    public Action AddEnemy;

    public List<Enemy> m_targetList = new List<Enemy>();

    CameraType m_currentCamType = CameraType.FreeLookCamera;

    int m_targetIndex = 0;
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
            m_L1 = m_pInput.currentActionMap["L1"];
            m_R1 = m_pInput.currentActionMap["R1"];
        }
    }

    private void Update()
    {
        //　カメラコントローラーの座標をPlayerと同期する
        this.transform.position = m_followTransform.position;
        OnPlayLockonCameta();
    }

    /// <summary>各カメラの機能制御</summary>
    void OnPlayLockonCameta()
    {
        switch (m_currentCamType)
        {
            case CameraType.FreeLookCamera:
                // ロックオンできる対象が居る場合ロックオンカメラを有効にする
                if (m_targetList.Count > 0 && m_chenge.triggered)
                {
                    SetLookAt();
                    OnChengeCamera();
                }
                break;

            case CameraType.LockonCamera:

                // ロックオンカメラのFollowに設定しているオブジェクトの向きをターゲットに向ける(↑のLookAtとは別物)
                m_followTransform.LookAt(m_targetList[m_targetIndex].transform);

                // ターゲットの切り替え
                if (m_L1.triggered)
                {
                    m_targetIndex = m_targetIndex == 0 ? m_targetList.Count - 1 : m_targetIndex - 1;
                    SetLookAt();
                }
                if (m_R1.triggered)
                {
                    m_targetIndex = m_targetIndex == m_targetList.Count - 1 ? 0 : m_targetIndex + 1;
                    SetLookAt();
                }

                if (m_chenge.triggered || m_targetList.Count == 0)
                {
                    OnChengeCamera();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>カメラを切り替える</summary>
    void OnChengeCamera()
    {
        switch (m_currentCamType)
        {
            case CameraType.FreeLookCamera:
                m_lockonCam.Priority = m_freeCam.Priority + 1;
                m_freeCam.Priority = 0;
                m_lockonCam.Priority = 1;
                m_lockonCam.enabled = true;
                m_freeCam.enabled = false;
                m_crossHairImage.enabled = true;
                m_currentCamType = CameraType.LockonCamera;
                break;
            case CameraType.LockonCamera:
                CinemachineVirtualCameraBase oldCamera = m_lockonCam;
                m_freeCam.Priority = m_lockonCam.Priority + 1;
                m_lockonCam.Priority = 0;
                m_freeCam.Priority = 1;
                m_lockonCam.enabled = false;
                m_freeCam.enabled = true;
                m_crossHairImage.enabled = false;
                m_freeCam.ChangeToFreeLook(oldCamera);
                m_currentCamType = CameraType.FreeLookCamera;
                break;
            default:
                break;
        }
    }

    /// <summary>TargetCamera(VirtualCamera)のLookAtにロックオン対象を設定する</summary>
    void SetLookAt()
    {
        // targetのIDを取得
        m_lockonCam.LookAt = m_targetList[m_targetIndex].transform;
        m_targetID = m_targetList[m_targetIndex].GetInstanceID();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Enemy"))
        {
            m_targetList.Add(target.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider target)
    {
        if (target.CompareTag("Enemy"))
        {
            var go = target.GetComponent<Enemy>();
            m_targetList.Remove(go);
            // ロックオン対象がロックオン範囲外に出た場合はロックオンカメラを強制的に終了する
            if (m_currentCamType.Equals(CameraType.LockonCamera) && m_targetID == go.GetInstanceID())
            {
                OnChengeCamera();
                m_targetIndex = 0;
                m_targetID = int.MaxValue;
            }
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