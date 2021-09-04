﻿using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;

/// <summary>ロックオンカメラ機能</summary>
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

    [SerializeField, Header("ターゲット対象")]
    Transform m_lockonTarget = null;

    InputAction m_chenge;

    CameraType m_currentCamType = CameraType.FreeLookCamera;

    List<GameObject> m_enemieList = new List<GameObject>();

    private void Start()
    {
        // PlayerのPlayerInputを参照
        if (m_pInput)
        {
            m_chenge = m_pInput.currentActionMap["ChengeCamera"];
        }
        else
        {
            Debug.LogWarning("PlayerInputがアサインされていません");
        }
    }

    private void Update()
    {
        transform.position = m_followTransform.position;
        LookAtTarget();
    }

    void LookAtTarget()
    {
        if (!m_lockonTarget) return;
        // 近くにターゲットがいる場合は、ロックオンカメラを使用可能にする
        m_lockonCam.LookAt = m_lockonTarget;
        m_followTransform.LookAt(m_lockonTarget);
        if (m_chenge.triggered)
        {
            ChengeCamera();
        }
    }

    /// <summary>カメラを切り替える</summary>
    public void ChengeCamera()
    {
        switch (m_currentCamType)
        {
            case CameraType.FreeLookCamera:
                m_lockonCam.Priority = m_freeCam.Priority + 1;
                m_freeCam.Priority = 0;
                m_lockonCam.Priority = 1;
                m_lockonCam.enabled = true;
                m_freeCam.enabled = false;
                m_currentCamType = CameraType.LockonCamera;
                break;
            case CameraType.LockonCamera:
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            m_enemieList.Add(go);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            m_enemieList.Remove(go);
        }
        Debug.Log(m_enemieList.Count);
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