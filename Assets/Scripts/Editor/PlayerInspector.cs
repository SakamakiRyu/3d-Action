using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
    GUIContent m_maxHpLabel;
    SerializedProperty m_maxHpProp;

    GUIContent m_moveSpeedLabel;
    SerializedProperty m_moveSpeedProp;

    GUIContent m_jumpPower;
    SerializedProperty m_jumpPowerProp;

    GUIContent m_atkLabel;
    SerializedProperty m_atkProp;

    private void OnEnable()
    {
        m_maxHpLabel = new GUIContent("最大HP");
        m_maxHpProp = serializedObject.FindProperty("m_maxHp");

        m_moveSpeedLabel = new GUIContent("移動速度");
        m_moveSpeedProp = serializedObject.FindProperty("m_moveSpeed");

        m_jumpPower = new GUIContent("ジャンプ力");

        m_atkLabel = new GUIContent("攻撃力");
        m_atkProp = serializedObject.FindProperty("m_atk");
    }

    public override void OnInspectorGUI()
    {
        var player = target as Player;

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_maxHpProp, m_maxHpLabel);
        EditorGUILayout.PropertyField(m_moveSpeedProp, m_moveSpeedLabel);
        EditorGUILayout.PropertyField(m_atkProp, m_atkLabel);
    }
}
