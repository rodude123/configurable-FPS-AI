using utilities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
	public class ConditionalHidePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			var condHAtt = (ConditionalHideAttribute)attribute;
			var enabled = GetConditionalHideAttributeResult(condHAtt, property);

			var wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!condHAtt.HideInInspector || enabled)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			GUI.enabled = wasEnabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var condHAtt = (ConditionalHideAttribute)attribute;
			var enabled = GetConditionalHideAttributeResult(condHAtt, property);

			if (!condHAtt.HideInInspector || enabled)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
			//The property is not being drawn
			//We want to undo the spacing added before and after the property
			return -EditorGUIUtility.standardVerticalSpacing;
			//return 0.0f;


			/*
		//Get the base height when not expanded
		var height = base.GetPropertyHeight(property, label);

		// if the property is expanded go through all its children and get their height
		if (property.isExpanded)
		{
			var propEnum = property.GetEnumerator();
			while (propEnum.MoveNext())
			    height += EditorGUI.GetPropertyHeight((SerializedProperty)propEnum.Current, GUIContent.none, true);
		}
		return height;*/
		}

		private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
		{
			var enabled = condHAtt.UseOrLogic ? false : true;

			//Handle primary property
			SerializedProperty sourcePropertyValue = null;
			//Get the full relative property path of the sourcefield so we can have nested hiding.Use old method when dealing with arrays
			if (!property.isArray)
			{
				var propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
				var conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
				sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

				//if the find failed->fall back to the old system
				if (sourcePropertyValue == null)
				{
					//original implementation (doens't work with nested serializedObjects)
					sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
				}
			}
			else
			{
				//original implementation (doens't work with nested serializedObjects)
				sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
			}


			if (sourcePropertyValue != null)
			{
				enabled = CheckPropertyType(sourcePropertyValue, condHAtt);
				if (condHAtt.InverseCondition1)
				{
					enabled = !enabled;
				}
			}


			//handle secondary property
			SerializedProperty sourcePropertyValue2 = null;
			if (!property.isArray)
			{
				var propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
				var conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField2); //changes the path to the conditionalsource property path
				sourcePropertyValue2 = property.serializedObject.FindProperty(conditionPath);

				//if the find failed->fall back to the old system
				if (sourcePropertyValue2 == null)
				{
					//original implementation (doens't work with nested serializedObjects)
					sourcePropertyValue2 = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField2);
				}
			}
			else
			{
				// original implementation(doens't work with nested serializedObjects) 
				sourcePropertyValue2 = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField2);
			}

			//Combine the results
			if (sourcePropertyValue2 != null)
			{
				var prop2Enabled = CheckPropertyType(sourcePropertyValue2, condHAtt);
				if (condHAtt.InverseCondition2)
				{
					prop2Enabled = !prop2Enabled;
				}

				if (condHAtt.UseOrLogic)
				{
					enabled = enabled || prop2Enabled;
				}
				else
				{
					enabled = enabled && prop2Enabled;
				}
			}

			//Handle the unlimited property array
			var conditionalSourceFieldArray = condHAtt.ConditionalSourceFields;
			var conditionalSourceFieldInverseArray = condHAtt.ConditionalSourceFieldInverseBools;
			for (var index = 0; index < conditionalSourceFieldArray.Length; ++index)
			{
				SerializedProperty sourcePropertyValueFromArray = null;
				if (!property.isArray)
				{
					var propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
					var conditionPath = propertyPath.Replace(property.name, conditionalSourceFieldArray[index]); //changes the path to the conditionalsource property path
					sourcePropertyValueFromArray = property.serializedObject.FindProperty(conditionPath);

					//if the find failed->fall back to the old system
					if (sourcePropertyValueFromArray == null)
					{
						//original implementation (doens't work with nested serializedObjects)
						sourcePropertyValueFromArray = property.serializedObject.FindProperty(conditionalSourceFieldArray[index]);
					}
				}
				else
				{
					// original implementation(doens't work with nested serializedObjects) 
					sourcePropertyValueFromArray = property.serializedObject.FindProperty(conditionalSourceFieldArray[index]);
				}

				//Combine the results
				if (sourcePropertyValueFromArray != null)
				{
					var propertyEnabled = CheckPropertyType(sourcePropertyValueFromArray, condHAtt);
					if (conditionalSourceFieldInverseArray.Length >= index + 1 && conditionalSourceFieldInverseArray[index])
					{
						propertyEnabled = !propertyEnabled;
					}

					if (condHAtt.UseOrLogic)
					{
						enabled = enabled || propertyEnabled;
					}
					else
					{
						enabled = enabled && propertyEnabled;
					}
				}
			}


			//wrap it all up
			if (condHAtt.Inverse)
			{
				enabled = !enabled;
			}

			return enabled;
		}

		private bool CheckPropertyType(SerializedProperty sourcePropertyValue, ConditionalHideAttribute condHAtt)
		{
			//Note: add others for custom handling if desired
			switch (sourcePropertyValue.propertyType)
			{
				case SerializedPropertyType.Boolean:
					return sourcePropertyValue.boolValue;
				case SerializedPropertyType.ObjectReference:
					return sourcePropertyValue.objectReferenceValue != null;
				case SerializedPropertyType.Enum:
					return sourcePropertyValue.enumValueIndex != condHAtt.EnemValueIndex;
				default:
					Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
					return true;
			}
		}
	}
}
