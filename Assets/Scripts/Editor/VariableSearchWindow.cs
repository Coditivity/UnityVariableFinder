using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Coditivity.GameFramework
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
            _serializedObject = new UnityEditor.SerializedObject(_monoBehaviour);
            Type type = _monoBehaviour.GetType();
            _fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            GetWindow(typeof(VariableSearchWindow));

        }


        string variableName = null;
        string variableCount = "";
        private void OnGUI()
        {

            string newVariableName = EditorGUILayout.TextField("Variable name:", variableName);

            if (string.Compare(newVariableName, variableName) != 0)
            {
                variableName = newVariableName;
            }

            foreach (FieldInfo field in _fields)
            {
                if (!string.IsNullOrEmpty(variableName))
                {
                    if (field.Name.ToLower().Contains(variableName.ToLower()))
                    {
                        SerializedProperty property = _serializedObject.FindProperty(field.Name);
                        if (property != null)
                        {
                            GUIContent content = new GUIContent(field.Name + " :");
                            EditorGUILayout.PropertyField(property, content);
                        }
                    }
                }
            }

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