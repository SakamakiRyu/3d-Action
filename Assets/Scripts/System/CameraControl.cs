﻿using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
public class CameraControl : MonoBehaviour
{
    /// <summary>カメラタイプ</summary>
    enum CameraType
    {
        /// <summary>フリールック</summary>
        FreeLookCamera,
        /// <summary>ロックオン</summary>
        LockonCamera
    }

    [SerializeField]
    private PlayerInput _pInput;

    [SerializeField]
    private CinemachineVirtualCamera _lockonCam;

    [SerializeField]
    private CinemachineFreeLook _freeCam;

    [SerializeField]
    private Transform _followTransform;

    [SerializeField]
    private Image _crosshairImage = default;

    [SerializeField]
    private float _radius = default;

    private SphereCollider _coll;
    private InputAction _chenge, _chengeUP, _chengeDown;
    private List<EnemyController> TargetList = new List<EnemyController>();

    /// <summary>現在のカメラタイプ</summary>
    private CameraType _currentCamType = CameraType.FreeLookCamera;
    /// <summary>ロックオンしているm_targetListの要素番号</summary>
    private int _targetIndex = 0;
    /// <summary>ロックオンしている敵のID</summary>
    private int _targetID;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //　カメラコントローラーの座標をFollowと同期する
        this.transform.position = _followTransform.position;
        ControlCamera();
    }

    private void Init()
    {
        if (!_pInput)
        {
            _pInput = FindObjectOfType<PlayerInput>();
        }
        _chenge = _pInput.currentActionMap["ChengeCamera"];
        _chengeUP = _pInput.currentActionMap["R1"];
        _chengeDown = _pInput.currentActionMap["L1"];
    }

    /// <summary>カメラを切り替える</summary>
    private void ChengeCamera()
    {
        switch (_currentCamType)
        {
            case CameraType.FreeLookCamera:
                {
                    _crosshairImage.enabled = true;
                    _lockonCam.Priority = _freeCam.Priority + 1;
                    _freeCam.Priority = 0;
                    _lockonCam.Priority = 1;
                    _lockonCam.enabled = true;
                    _freeCam.enabled = false;
                    _currentCamType = CameraType.LockonCamera;
                }
                break;
            case CameraType.LockonCamera:
                {
                    _crosshairImage.enabled = false;
                    _targetIndex = 0;
                    _targetID = 0;
                    SortingTarget();
                    CinemachineVirtualCameraBase oldCamera = _lockonCam;
                    _freeCam.Priority = _lockonCam.Priority + 1;
                    _lockonCam.Priority = 0;
                    _freeCam.Priority = 1;
                    _lockonCam.enabled = false;
                    _freeCam.enabled = true;
                    _freeCam.ChangeToFreeLook(oldCamera);
                    _currentCamType = CameraType.FreeLookCamera;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>カメラの機能制御</summary>
    private void ControlCamera()
    {
        switch (_currentCamType)
        {
            case CameraType.FreeLookCamera:
                // ロックオン対象が存在する時にカメラ変更のボタンが押されたらカメラを切り替える
                if (TargetList.Count > 0 && _chenge.triggered)
                {
                    SetTarget();
                    ChengeCamera();
                }
                break;
            case CameraType.LockonCamera:
                // ロックオンできる敵が居なくなる、またはロックオン対象が死んだ場合、
                // またはカメラ変更ボタンが押された時にカメラを切り替える。
                if (TargetList.Count == 0 || TargetList[_targetIndex].IsDead || _chenge.triggered)
                {
                    ChengeCamera();
                }

                if (TargetList.Count != 0)
                {
                    // ロックオンカメラ(VirtualCamera)のFollowに設定しているオブジェクトの正面をターゲットに向ける
                    _followTransform.LookAt(TargetList[_targetIndex].transform);
                }

                // ロックオン可能な対象が二つ以上の時にロックオンの対象を切り替える
                if (TargetList.Count > 1)
                {
                    if (_chengeUP.triggered)
                    {
                        _targetIndex = _targetIndex == TargetList.Count - 1 ? 0 : ++_targetIndex;
                        SetTarget();
                    }
                    if (_chengeDown.triggered)
                    {
                        _targetIndex = _targetIndex == 0 ? TargetList.Count - 1 : --_targetIndex;
                        SetTarget();
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>TargetListの整理</summary>
    private void SortingTarget()
    {
        TargetList = TargetList.Where(x => !x.IsDead).ToList();
    }

    /// <summary>TargetCamera(VirtualCamera)のLookAtにロックオン対象を設定し、_targetIDを上書きする</summary>
    private void SetTarget()
    {
        _lockonCam.LookAt = TargetList[_targetIndex].transform;
        _targetID = TargetList[_targetIndex].GetInstanceID();
    }

    private void AddTarget(EnemyController enemy)
    {
        TargetList.Add(enemy);
    }

    private void RemoveTarget(EnemyController enemy)
    {
        TargetList.Remove(enemy);
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
            if (target.GetInstanceID() == _targetID)
            {
                ChengeCamera();
            }
            RemoveTarget(target.GetComponent<EnemyController>());
        }
    }



#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_coll)
        {
            _coll = GetComponent<SphereCollider>();
        }
        _coll.radius = _radius;
        if (_freeCam && _followTransform)
        {
            _freeCam.Follow = _followTransform;
            _freeCam.LookAt = _followTransform;
        }
        if (_lockonCam && _followTransform)
        {
            _freeCam.Follow = _followTransform;
        }
    }
#endif
}