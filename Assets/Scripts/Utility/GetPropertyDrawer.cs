using UnityEditor;
using UnityEngine;

namespace Utility
{
    [CustomPropertyDrawer(typeof(GetAttribute))]
    public class GetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 1. Check if the field is currently empty
            if (property.objectReferenceValue == null)
            {
                // 2. Get the GameObject this script is attached to
                MonoBehaviour target = property.serializedObject.targetObject as MonoBehaviour;
            
                if (target != null)
                {
                    // 3. Try to find the component on the same GameObject
                    // property.type gives us the class name (e.g., "PPtr<$Health>")
                    var component = target.GetComponent(fieldInfo.FieldType);
                
                    // 4. Assign the component to the script if it does exist
                    if (component != null)
                    {
                        property.objectReferenceValue = component;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            // 5. Draw the normal property field so we can still see it in the Inspector
            EditorGUI.PropertyField(position, property, label);
        } 
    }
}