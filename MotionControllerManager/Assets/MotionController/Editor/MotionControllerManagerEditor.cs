using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.MotionController.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.MotionController.Editor
{

    [CustomEditor(typeof(MotionControllerManager))]
    public  class MotionControllerManagerEditor : UnityEditor.Editor
    {

        private SerializedProperty _rightSelectDown;
        private SerializedProperty _rightGrasped;
        private SerializedProperty _rightMenuDown;
        private SerializedProperty _rightThumbstickDown;
        private SerializedProperty _rightTouchpadTouched;
        private SerializedProperty _rightTouchpadDown;

        private SerializedProperty _leftSelectDown;
        private SerializedProperty _leftGraspedDown;
        private SerializedProperty _leftMenuDown;
        private SerializedProperty _leftThumbstickDown;
        private SerializedProperty _leftTouchpadTouched;
        private SerializedProperty _leftTouchpadDown;

        private SerializedProperty _leftLaserPointer;
        private SerializedProperty _rightLaserPointer;

        public MotionControllerManagerEditor()
        {

        }


        private void OnEnable()
        {
            _rightSelectDown = serializedObject.FindProperty("RightSelectDown");
            _rightGrasped = serializedObject.FindProperty("RightGrasped");
            _rightMenuDown = serializedObject.FindProperty("RightMenuDown");
            _rightThumbstickDown = serializedObject.FindProperty("RightThumbstickDown");
            _rightTouchpadTouched = serializedObject.FindProperty("RightTouchpadTouched");
            _rightTouchpadDown = serializedObject.FindProperty("RightTouchpadDown");
            _leftSelectDown = serializedObject.FindProperty("LeftSelectDown");
            _leftGraspedDown = serializedObject.FindProperty("LeftGrasped");
            _leftMenuDown = serializedObject.FindProperty("LeftMenuDown");
            _leftThumbstickDown = serializedObject.FindProperty("LeftThumbstickDown");
            _leftTouchpadTouched = serializedObject.FindProperty("LeftTouchpadTouched");
            _leftTouchpadDown = serializedObject.FindProperty("LeftTouchpadDown");

            _leftLaserPointer = serializedObject.FindProperty("LeftLaserPointer");
            _rightLaserPointer = serializedObject.FindProperty("RightLaserPointer");
        }
        public override void OnInspectorGUI()
        {
            
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Left Pointer Settings");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_leftLaserPointer,new GUIContent("Override Renderer"));
            EditorGUILayout.PropertyField(_leftSelectDown, new GUIContent("Select Down"));
            EditorGUILayout.PropertyField(_leftGraspedDown, new GUIContent("Grasped Down"));
            EditorGUILayout.PropertyField(_leftMenuDown, new GUIContent("Menue Down"));
            EditorGUILayout.PropertyField(_leftThumbstickDown, new GUIContent("Thumbstick Down"));
            EditorGUILayout.PropertyField(_leftTouchpadTouched, new GUIContent("Touchpad Touched"));
            EditorGUILayout.PropertyField(_leftTouchpadDown, new GUIContent("Touchpad Down"));
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Right Pointer Settings");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_rightLaserPointer, new GUIContent("Override Renderer"));
            EditorGUILayout.PropertyField(_rightSelectDown, new GUIContent("Select Down"));
            EditorGUILayout.PropertyField(_rightGrasped, new GUIContent("Grasped Down"));
            EditorGUILayout.PropertyField(_rightMenuDown, new GUIContent("Menue Down"));
            EditorGUILayout.PropertyField(_rightThumbstickDown, new GUIContent("Thumbstick Down"));
            EditorGUILayout.PropertyField(_rightTouchpadTouched, new GUIContent("Touchpad Touched"));
            EditorGUILayout.PropertyField(_rightTouchpadDown, new GUIContent("Touchpad Down"));
            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
