using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
    GUIContent m_moveSpeedLabel;
    SerializedProperty m_moveSpeedProp;

    GUIContent m_jumpPowerLabel;
    SerializedProperty m_jumpPowerProp;

    GUIContent m_turnSpeedLabel;
    SerializedProperty m_turnSpeedProp;

    GUIContent m_lineLengthLabel;
    SerializedProperty m_lineLengthProp;

    GUIContent m_maxHpLabel;
    SerializedProperty m_maxHpProp;

    GUIContent m_atkLabel;
    SerializedProperty m_atkProp;

    private void OnEnable()
    {
        m_moveSpeedLabel = new GUIContent("移動速度");
        m_moveSpeedProp = serializedObject.FindProperty("m_moveSpeed");

        m_jumpPowerLabel = new GUIContent("ジャンプ力");
        m_jumpPowerProp = serializedObject.FindProperty("m_jumpPower");

        m_turnSpeedLabel = new GUIContent("振り向く速さ");
        m_turnSpeedProp = serializedObject.FindProperty("m_turnSpeed");

        m_lineLengthLabel = new GUIContent("接地判定に使う線の長さ");
        m_lineLengthProp = serializedObject.FindProperty("m_lineLength");

        m_maxHpLabel = new GUIContent("最大HP");
        m_maxHpProp = serializedObject.FindProperty("m_maxHp");

        m_atkLabel = new GUIContent("攻撃力");
        m_atkProp = serializedObject.FindProperty("m_atk");
    }

    public override void OnInspectorGUI()
    {
        var player = target as Player;

        serializedObject.Update();

        EditorGUILayout.LabelField("       内部データ");
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_moveSpeedProp, m_moveSpeedLabel);
        EditorGUILayout.PropertyField(m_jumpPowerProp, m_jumpPowerLabel);
        EditorGUILayout.PropertyField(m_turnSpeedProp, m_turnSpeedLabel);
        EditorGUILayout.PropertyField(m_lineLengthProp, m_lineLengthLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("       能力値");
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_maxHpProp, m_maxHpLabel);
        EditorGUILayout.PropertyField(m_atkProp, m_atkLabel);

        serializedObject.ApplyModifiedProperties();
    }
}
