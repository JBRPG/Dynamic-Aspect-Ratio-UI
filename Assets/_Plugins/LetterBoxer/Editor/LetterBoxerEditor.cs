using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LetterBoxer))]
public class LetterBoxerEditor : Editor
{
    SerializedProperty referenceModeProp;
    SerializedProperty xProp;
    SerializedProperty yProp;
    SerializedProperty widthProp;
    SerializedProperty heightProp;
    SerializedProperty onAwakeProp;
    SerializedProperty onUpdateProp;
    SerializedProperty consistentOrthographicProp;
    SerializedProperty targetOrthographicProp;

    void OnEnable()
    {
        referenceModeProp = serializedObject.FindProperty("referenceMode");
        xProp = serializedObject.FindProperty("aspectWidth");
        yProp = serializedObject.FindProperty("aspectHeight");
        widthProp = serializedObject.FindProperty("resolutionWidth");
        heightProp = serializedObject.FindProperty("resolutionHeight");
        onAwakeProp = serializedObject.FindProperty("onAwake");
        onUpdateProp = serializedObject.FindProperty("onUpdate");
        consistentOrthographicProp = serializedObject.FindProperty("consistentOrthographicView");
        targetOrthographicProp = serializedObject.FindProperty("targetOrthographicSize");
    }

    override public void OnInspectorGUI()
    {
        serializedObject.Update();        
        EditorGUILayout.PropertyField(referenceModeProp, new GUIContent("Reference Mode", "Toggles whether you want to use an aspect ratio or a resolution to calculate the letterboxing"));

        LetterBoxer.ReferenceMode currentReferenceMode = (LetterBoxer.ReferenceMode)referenceModeProp.enumValueIndex;

        if (currentReferenceMode == LetterBoxer.ReferenceMode.DesignedAspectRatio)
        {
            EditorGUI.indentLevel++;            
            EditorGUILayout.PropertyField(xProp, new GUIContent("X", "The X portion of the aspect ratio"));
            EditorGUILayout.PropertyField(yProp, new GUIContent("Y", "The Y portion of the aspect ratio"));            
            EditorGUI.indentLevel--;
        }
        else
        { 
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(widthProp, new GUIContent("Width", "The width in pixels"));
            EditorGUILayout.PropertyField(heightProp, new GUIContent("Height", "The height in pixels"));
            EditorGUI.indentLevel--;            
        }
                
        EditorGUILayout.LabelField("Calculate");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(onAwakeProp, new GUIContent("On Awake", "Calculate the letterboxing during OnAwake()"));
        EditorGUILayout.PropertyField(onUpdateProp, new GUIContent("On Update", "Calculate the letterboxing during OnUpdate()"));
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Orthographic Consistency Settings");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(consistentOrthographicProp, new GUIContent("Consistent Orthographic View",
            "If enabled, will always have target orthographic size based on horizontal aspect ratio."));
        EditorGUILayout.PropertyField(targetOrthographicProp, new GUIContent("Target Orthographic Size",
            "If aspect ratio favors horizontal, then keep target ratio.\n" +
            "Otherwise when aspect ratio is vertical, adjust target orthographic size."));
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

