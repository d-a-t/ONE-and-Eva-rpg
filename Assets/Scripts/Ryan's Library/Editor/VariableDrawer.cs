using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(Variable), true)]
public class VariableDrawer<T> : NestablePropertyDrawer {
	protected new Variable<T> propertyObject { get { return (Variable<T>)base.propertyObject; } }

	private SerializedProperty ValueField = null;

	protected override void Initialize(SerializedProperty prop) {
		base.Initialize(prop);

		if (ValueField == null)
			ValueField = prop.FindPropertyRelative("_Value");
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		base.OnGUI(position, property, label);

		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var ValueRect = new Rect(position.x, position.y, 30, position.height);
		var LockedRect = new Rect(position.x + 30 + 5, position.y, 20, position.height);


		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(ValueRect, property.FindPropertyRelative("_Value"), GUIContent.none);
		EditorGUI.PropertyField(LockedRect, property.FindPropertyRelative("Locked"), GUIContent.none);
		if (EditorGUI.EndChangeCheck()) {
			ValueField.serializedObject.ApplyModifiedProperties();
			propertyObject.Call();
		}

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
