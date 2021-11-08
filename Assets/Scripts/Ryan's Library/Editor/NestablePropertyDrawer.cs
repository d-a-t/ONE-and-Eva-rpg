using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions;
using Object = System.Object;

public class NestablePropertyDrawer : PropertyDrawer {
	protected Object propertyObject = null;

	private static readonly Regex matchArrayElement = new Regex(@"^data\[(\d+)\]$");
	protected virtual void Initialize(SerializedProperty prop) {
		if (propertyObject == null) {
			string[] path = prop.propertyPath.Split('.');
			propertyObject = prop.serializedObject.targetObject;
			FieldInfo field = null;

			foreach (string pathNode in path) {
				if (field != null && field.FieldType.IsArray) {
					if (pathNode.Equals("Array"))
						continue;

					Match elementMatch = matchArrayElement.Match(pathNode);
					int index;
					if (elementMatch.Success && int.TryParse(elementMatch.Groups[1].Value, out index)) {
						field = null;
						object[] objectArray = (object[])propertyObject;
						if (objectArray != null && index < objectArray.Length) {
							propertyObject = ((object[])propertyObject)[index];
						} else {
							propertyObject = null;
							break;
						}
					}
				} else {
					field = propertyObject.GetType().GetField(pathNode, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					propertyObject = field.GetValue(propertyObject);
				}
			}
		}
	}

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
		Initialize(prop);
	}
}