using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;
using System;

/// <summary>
/// ターゲットカメラの制御クラス
/// <para>VirCamをターゲットカメラとして利用する</para>
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class CameraController : MonoBehaviour
{
    #region Define
    /// <summary>カメラタイプ</summary>
    private enum CameraType
    {
        Default,
        /// <summary>フリールック</summary>
        FreeLookCamera,
        /// <summary>ロックオン</summary>
        VirCamera
    }
    #endregion

    #region Field
    [SerializeField]
    private PlayerInput _pInput;

    [SerializeField]
    private CinemachineFreeLook _freeCam;

    [SerializeField]
    private CinemachineVirtualCamera _virCam;

    [SerializeField]
    private Transform _followTarget;

    // インプットシステムの入力を受け取る変数
    private InputAction _chenge, _chengeUP, _chengeDown;

    /// <summary>ターゲット対象のリスト(EnemyControllerクラス)</summary>
    private List<EnemyController> TargetList = new List<EnemyController>();

    /// <summary>現在のカメラタイプ</summary>
    private CameraType _currentCamType = CameraType.Default;
    #endregion

    #region Unity Function
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        SyncTransform();
        CameraControl();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<EnemyController>(out var enemy);

        if (enemy)
        {
            AddTargetList(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<EnemyController>(out var enemy);

        if (enemy)
        {
            RemoveTargetList(enemy);
        }
    }
    #endregion

    #region Private Function
    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        _currentCamType = CameraType.FreeLookCamera;
        // 初期は自由視点
        _freeCam.Priority = 1;
        _virCam.Priority = 0;
        // 入力の紐づけ
        _chenge = _pInput.currentActionMap["ChengeCamera"];
        _chengeUP = _pInput.currentActionMap["R1"];
        _chengeDown = _pInput.currentActionMap["L1"];
    }

    /// <summary>
    /// カメラの座標をフォロー対象と同期する
    /// </summary>
    private void SyncTransform()
    {
        this.transform.position = _followTarget.position;
    }

    /// <summary>
    /// カメラの制御処理
    /// </summary>
    private void CameraControl()
    {
        switch (_currentCamType)
        {
            case CameraType.FreeLookCamera:
                {
                    // 
                    if (TargetList.Count > 0)
                    {
                        ChengeCameraType(CameraType.VirCamera);
                    }
                }
                break;
            case CameraType.VirCamera:
                {
                    ChengeCameraType(CameraType.FreeLookCamera);
                }
                break;
        }
    }

    /// <summary>
    /// カメラの切り替え処理
    /// </summary>
    private void ChengeCameraType(CameraType next)
    {
        switch (next)
        {
            case CameraType.FreeLookCamera:
                {
                    _freeCam.Priority = _virCam.Priority + 1;
                    _virCam.Priority = 0;
                }
                break;
            case CameraType.VirCamera:
                {
                    _virCam.Priority = _freeCam.Priority + 1;
                    _freeCam.Priority = 0;
                }
                break;
        }

        _currentCamType = next;
    }


    /// <summary>
    /// 対象をターゲットリストに追加する
    /// </summary>
    /// <param name="enemy">対象</param>
    private void AddTargetList(EnemyController enemy)
    {
        TargetList.Add(enemy);
    }

    /// <summary>
    /// 対象をターゲットリストから外す
    /// </summary>
    /// <param name="enemy">対象</param>
    private void RemoveTargetList(EnemyController enemy)
    {
        TargetList.Remove(enemy);
    }
    #endregion
}
