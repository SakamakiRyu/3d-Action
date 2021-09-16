using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLookOnTargets : MonoBehaviour
{
    [SerializeField, Header("敵のリストを持っているクラス")]
    CameraController m_camController = default;

    [SerializeField]
    Image[] m_images;

    private void Start()
    {
        m_images = GetComponentsInChildren<Image>();
        foreach (var item in m_images)
        {
            item.gameObject.SetActive(false);
        }
        m_camController.AddEnemy += ImageSetting;
    }

    private void OnDestroy()
    {
        m_camController.AddEnemy -= ImageSetting;
    }

    /// <summary>敵のアイコンを設定</summary>
    void ImageSetting()
    {
        if (m_camController.m_targetList.Count == 0)
        {
            foreach (var item in m_images)
            {
                item.sprite = null;
            }
            ChengeState();
            return;
        }
        for (int i = 0; i < m_camController.m_targetList.Count; i++)
        {
            m_images[i].sprite = m_camController.m_targetList[i].Sprite;
        }
        ChengeState();
    }

    /// <summary>Imageの状態切り替え</summary>
    void ChengeState()
    {
        foreach (var item in m_images)
        {
            item.gameObject.SetActive(item.sprite != null);
        }
    }
}
