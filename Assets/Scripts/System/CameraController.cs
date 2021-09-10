using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
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

    [SerializeField, Header("ロックオン可能な敵")]
    public List<Enemy> m_enemieList = new List<Enemy>();

    public Action AddEnemy;

    private void Start()
    {
        if (!m_pInput)
        {
            Debug.LogError($"{gameObject.name}にPlayerInputがアサインされていません");
        }
        else
        {
            m_chenge = m_pInput.currentActionMap["ChengeCamera"];
        }
    }

    private void Update()
    {
        transform.position = m_followTransform.position;
        LookAtTarget();
    }

    void LookAtTarget()
    {
        // ロックオンカメラ有効中に、ロックオン対象が居なくなった場合には、強制敵にFreeLookにする
        if (m_currentCamType.Equals(CameraType.LockonCamera) && m_enemieList.Count == 0)
        {
            ChengeCamera();
        }
        if (m_enemieList.Count == 0) return;

        // 近くにターゲットがいる場合は、ロックオンカメラを使用可能にする
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
            var go = other.GetComponent<Enemy>();
            AddEnemyList(go);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var go = other.GetComponent<Enemy>();
            RemoveEnemyList(go);
        }
    }

    /// <summary>敵のリストの追加処理</summary>
    void AddEnemyList(Enemy enemy)
    {
        m_enemieList.Add(enemy);
        AddEnemy?.Invoke();
    }

    /// <summary>敵のリストの削除処理</summary>
    void RemoveEnemyList(Enemy enemy)
    {
        m_enemieList.Remove(enemy);
        AddEnemy?.Invoke();
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