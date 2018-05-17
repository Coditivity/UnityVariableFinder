using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class VariableSearchWindow : EditorWindow {


    static MonoBehaviour _monoBehaviour = null;
    static SerializedObject _serializedObject = null;

    [MenuItem("CONTEXT/MonoBehaviour/Find Variable")]    
    private static void SearchWidow(MenuCommand menuCommand)
    {

        _monoBehaviour = menuCommand.context as MonoBehaviour;
        _serializedObject = new UnityEditor.SerializedObject(_monoBehaviour);
        GetWindow(typeof(VariableSearchWindow));
    }


    string variableName = null;
    string variableCount = "";
    private void OnGUI()
    {
        
        string newVariableName = EditorGUILayout.TextField("Variable name:", variableName);
        FieldInfo[] fields = null;
        if(string.Compare(newVariableName, variableName) != 0)
        {
            Type type = _monoBehaviour.GetType();
            fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            variableName = newVariableName;
            variableCount = fields[0].Name;// .Length.ToString();
            
        }
        
       // if(fields!=null)
        {
            //if (fields.Length > 0)
            {
                GUIContent content = new GUIContent("Name: ");
                EditorGUILayout.PropertyField(_serializedObject.FindProperty("potato"), content);
                _serializedObject.ApplyModifiedProperties();
            }
        }
       



        
    }

    private void OnDestroy()
    {
        _monoBehaviour = null;
        _serializedObject = null;
    }
}
