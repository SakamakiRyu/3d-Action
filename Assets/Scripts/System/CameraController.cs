using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;
using System;

/// <summary>
/// �^�[�Q�b�g�J�����̐���N���X
/// <para>VirCam���^�[�Q�b�g�J�����Ƃ��ė��p����</para>
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class CameraController : MonoBehaviour
{
    #region Define
    /// <summary>�J�����^�C�v</summary>
    private enum CameraType
    {
        Default,
        /// <summary>�t���[���b�N</summary>
        FreeLookCamera,
        /// <summary>���b�N�I��</summary>
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

    // �C���v�b�g�V�X�e���̓��͂��󂯎��ϐ�
    private InputAction _chenge, _chengeUP, _chengeDown;

    /// <summary>�^�[�Q�b�g�Ώۂ̃��X�g(EnemyController�N���X)</summary>
    private List<EnemyController> TargetList = new List<EnemyController>();

    /// <summary>���݂̃J�����^�C�v</summary>
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
    /// ������
    /// </summary>
    private void Init()
    {
        _currentCamType = CameraType.FreeLookCamera;
        // �����͎��R���_
        _freeCam.Priority = 1;
        _virCam.Priority = 0;
        // ���͂̕R�Â�
        _chenge = _pInput.currentActionMap["ChengeCamera"];
        _chengeUP = _pInput.currentActionMap["R1"];
        _chengeDown = _pInput.currentActionMap["L1"];
    }

    /// <summary>
    /// �J�����̍��W���t�H���[�ΏۂƓ�������
    /// </summary>
    private void SyncTransform()
    {
        this.transform.position = _followTarget.position;
    }

    /// <summary>
    /// �J�����̐��䏈��
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
    /// �J�����̐؂�ւ�����
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
    /// �Ώۂ��^�[�Q�b�g���X�g�ɒǉ�����
    /// </summary>
    /// <param name="enemy">�Ώ�</param>
    private void AddTargetList(EnemyController enemy)
    {
        TargetList.Add(enemy);
    }

    /// <summary>
    /// �Ώۂ��^�[�Q�b�g���X�g����O��
    /// </summary>
    /// <param name="enemy">�Ώ�</param>
    private void RemoveTargetList(EnemyController enemy)
    {
        TargetList.Remove(enemy);
    }
    #endregion
}
