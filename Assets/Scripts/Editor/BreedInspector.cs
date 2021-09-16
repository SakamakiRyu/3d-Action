using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Breed))]
public class BreedInspector : Editor
{
    GUIContent parentLabel;
    SerializedProperty parentProp;

    GUIContent imageLabel;
    SerializedProperty imageProp;

    GUIContent maxHpLabel;
    SerializedProperty maxHpProp;

    GUIContent atkPowerLabel;
    SerializedProperty atkPowerProp;

    GUIContent overrideMaxHpLabel;
    SerializedProperty overrideMaxHpProp;

    GUIContent nameLabel;
    SerializedProperty nameProp;

    GUIContent overrideWeaknessesLabel;
    SerializedProperty overrideWeaknessesProp;

    GUIContent weaknessesLabel;
    SerializedProperty weaknessesProp;

    private void OnEnable()
    {
        parentLabel = new GUIContent("親系統");
        parentProp = serializedObject.FindProperty("m_parent");

        imageLabel = new GUIContent("アイコン画像");
        imageProp = serializedObject.FindProperty("m_sprite");

        maxHpLabel = new GUIContent("最大HP");
        maxHpProp = serializedObject.FindProperty("m_maxHp");

        atkPowerLabel = new GUIContent("攻撃力");
        atkPowerProp = serializedObject.FindProperty("m_atkPower");

        overrideMaxHpLabel = new GUIContent("最大HPを上書き");
        overrideMaxHpProp = serializedObject.FindProperty("m_IsOverrideMaxHp");

        nameLabel = new GUIContent("系統名");
        nameProp = serializedObject.FindProperty("m_name");

        overrideWeaknessesLabel = new GUIContent("弱点属性を上書き");
        overrideWeaknessesProp = serializedObject.FindProperty("m_IsOverrideWeaknesses");

        weaknessesLabel = new GUIContent("弱点武器");
        weaknessesProp = serializedObject.FindProperty("m_weaknesses");
    }

    public override void OnInspectorGUI()
    {
        var breed = target as Breed;

        // 最新データを取得
        serializedObject.Update();

        // 親系統
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(parentProp, parentLabel);
            if (check.changed)
            {
                // 一旦反映させて、無効な親系統でないかチェック
                serializedObject.ApplyModifiedProperties();
                breed.ValidateParent();
                serializedObject.Update();
            }
        }

        EditorGUILayout.PropertyField(imageProp, imageLabel);
        EditorGUILayout.PropertyField(atkPowerProp, atkPowerLabel);

        Breed parent = parentProp.objectReferenceValue as Breed;
        if (!parent)
        {
            // 親系統を持たない場合、各パラメータの編集のみ
            EditorGUILayout.PropertyField(nameProp, nameLabel);
            EditorGUILayout.PropertyField(maxHpProp, maxHpLabel);
            EditorGUILayout.PropertyField(weaknessesProp, weaknessesLabel);
        }
        else
        {
            // 親系統の系統名を表示する
            EditorGUILayout.LabelField($"{nameLabel.text} : {parent.Name}");
            EditorGUILayout.Space();

            // 弱点武器を上書きするか
            EditorGUILayout.PropertyField(overrideWeaknessesProp, overrideWeaknessesLabel);
            if (overrideWeaknessesProp.boolValue)
            {
                // 上書きをするなら書き換え
                EditorGUILayout.PropertyField(weaknessesProp, weaknessesLabel);
            }
            else
            {
                // 上書きしないなら親系統のものを表示する
                EditorGUILayout.LabelField($"{weaknessesLabel.text} : {parent.Weaknesses}");
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(overrideMaxHpProp, overrideMaxHpLabel);
            // 最大HPを上書きするか
            if (overrideMaxHpProp.boolValue)
            {
                EditorGUILayout.PropertyField(maxHpProp, maxHpLabel);
            }
            else
            {
                EditorGUILayout.LabelField($"{maxHpLabel.text} : {parent.MaxHp}");
            }
        }

        // 変更点を反映させる
        serializedObject.ApplyModifiedProperties();
    }
}