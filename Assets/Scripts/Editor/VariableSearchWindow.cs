﻿using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Coditivity.GameFramework.Editor
{
    public class VariableSearchWindow : EditorWindow
    {
        static MonoBehaviour _monoBehaviour = null;
        static SerializedObject _serializedObject = null;
        static FieldInfo[] _fields = null;

        [MenuItem("CONTEXT/MonoBehaviour/Find Variable")]
        private static void SearchWidow(MenuCommand menuCommand)
        {
            _monoBehaviour = menuCommand.context as MonoBehaviour;
            _serializedObject = new SerializedObject(_monoBehaviour);
            Type type = _monoBehaviour.GetType();
            _fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            GetWindow(typeof(VariableSearchWindow), true, _monoBehaviour.GetType().ToString() + ": Variable Search");
            
        }


        string variableName = null;
        string variableCount = "";

        const string nameVariableNameField = "variableNameField";
        bool firstRun = true; //If window opened for the first time

        Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {


            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Variable Name:", style);
            EditorGUI.indentLevel -= 4;
            GUI.SetNextControlName(nameVariableNameField);
            string newVariableName = EditorGUILayout.TextField(variableName);
            EditorGUILayout.EndHorizontal();

            if(firstRun)
            {
                firstRun = false;
                EditorGUI.FocusTextInControl(nameVariableNameField);
            }

            EditorGUI.indentLevel += 4;
            EditorGUILayout.Separator();
            
            if (string.Compare(newVariableName, variableName) != 0)
            {
                variableName = newVariableName;
            }

            _serializedObject.Update();
            foreach (FieldInfo field in _fields)
            {
                if (!string.IsNullOrEmpty(variableName))
                {
                    if (field.Name.ToLower().Contains(variableName.ToLower()))
                    {
                        SerializedProperty property = _serializedObject.FindProperty(field.Name);
                        if (property != null)
                        {   
                            GUIContent content = new GUIContent(field.Name + " :", property.tooltip);
                            EditorGUILayout.PropertyField(property, content, true);
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            _serializedObject.ApplyModifiedProperties();
        }

        private void OnDestroy()
        {
            _monoBehaviour = null;
            _serializedObject = null;
            _fields = null;
        }
    }

}