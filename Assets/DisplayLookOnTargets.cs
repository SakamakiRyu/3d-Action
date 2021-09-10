using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLookOnTargets : MonoBehaviour
{
    [SerializeField, Header("敵のリストを持っているクラス")]
    CameraController m_camController = default;

    [SerializeField]
    // List<Image> m_images = new List<Image>();

    Image[] m_images;
    private void Start()
    {
        m_images = GetComponentsInChildren<Image>();
    }
}
