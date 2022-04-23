using UnityEngine;

static class CinemachineExtensions
{
    public static void ChangeToFreeLook(this Cinemachine.CinemachineFreeLook camera, Cinemachine.CinemachineVirtualCameraBase from)
    {
        var freelook = from as Cinemachine.CinemachineFreeLook;
        if (freelook)
        {
            CopyAxis(camera, freelook);
        }
        else
        {
            LookAtTarget(camera, from.LookAt.position);
        }
    }

    public static void CopyAxis(this Cinemachine.CinemachineFreeLook camera, Cinemachine.CinemachineFreeLook from)
    {
        camera.m_XAxis.Value = from.m_XAxis.Value;
        camera.m_YAxis.Value = from.m_YAxis.Value;
    }

    public static void LookAtTarget(this Cinemachine.CinemachineFreeLook camera, Vector3 target)
    {
        //元コード
        //https://qiita.com/asiram/items/8ce8cbea0511dbf7fd75

        // それぞれの座標をカメラの高さに合わせる.
        float cameraHeight = camera.transform.position.y;
        Vector3 followPosition = new Vector3(camera.Follow.position.x, cameraHeight, camera.Follow.position.z);
        Vector3 targetPosition = new Vector3(target.x, cameraHeight, target.z);

        // それぞれのベクトルを計算.
        Vector3 followToTarget = targetPosition - followPosition;
        Vector3 followToTargetReverse = Vector3.Scale(followToTarget, new Vector3(-1, 1, -1));
        Vector3 followToCamera = camera.transform.position - followPosition;

        // カメラ回転の角度と方向を計算.
        Vector3 axis = Vector3.Cross(followToCamera, followToTargetReverse);
        float direction = axis.y < 0 ? -1 : 1;
        float angle = Vector3.Angle(followToCamera, followToTargetReverse);

        camera.m_XAxis.Value = angle * direction;
    }
}
