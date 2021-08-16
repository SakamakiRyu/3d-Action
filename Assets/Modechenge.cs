using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Modechenge : MonoBehaviour
{
    [SerializeField]
    InputActionMap[] m_actionMaps;
    PlayerInput m_pInput;
    InputAction m_option;

    void Start()
    {
        m_pInput = GetComponent<PlayerInput>();
        m_actionMaps = m_pInput.actions.actionMaps.ToArray();   // アクションマップを全て取得
        m_option = m_pInput.currentActionMap["Option"];
    }

    void Update()
    {
        if (m_option.triggered)
        {
            foreach (var item in m_actionMaps)
            {
                Debug.Log(item.name);
            }
        }
    }
}
