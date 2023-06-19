using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyButtonImage))]
public class MyButtonImageEditor : Editor
{

    private GUIStyle centerLabelStyle;
    void OnEnable()
    {
        //centerLabelStyle = new GUIStyle(EditorStyles.label);
     //   centerLabelStyle.alignment = TextAnchor.MiddleCenter;
    }
    public override void OnInspectorGUI()
    {
        MyButtonImage script = (MyButtonImage)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("NormalColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HoverColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ClickedColor"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("tweenInterval"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size_on"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size_off"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("show"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("oneshot"));

        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeNotice"));

        if (script.shakeNotice)
        {
            EditorGUILayout.LabelField("Shake Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeBeforeShake"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeTimeUnit"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeInterval"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeAngle"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
