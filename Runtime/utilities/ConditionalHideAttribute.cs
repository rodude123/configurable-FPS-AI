using System;
using UnityEngine;

namespace utilities
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
	                AttributeTargets.Class | AttributeTargets.Struct)]
	public class ConditionalHideAttribute : PropertyAttribute
	{
		public string ConditionalSourceField = "";
		public string ConditionalSourceField2 = "";
		public bool[] ConditionalSourceFieldInverseBools = { };
		public string[] ConditionalSourceFields = { };
		public int EnemValueIndex;
		public bool HideInInspector;
		public bool Inverse;

		public bool InverseCondition1 = false;
		public bool InverseCondition2 = false;
		public bool UseOrLogic = false;


		// Use this for initialization
		public ConditionalHideAttribute(string conditionalSourceField)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = false;
			Inverse = false;
		}

		public ConditionalHideAttribute(string conditionalSourceField, int enumValueIndex)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = false;
			Inverse = false;
			EnemValueIndex = enumValueIndex;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = hideInInspector;
			Inverse = false;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, int enumValueIndex)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = hideInInspector;
			Inverse = false;
			EnemValueIndex = enumValueIndex;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = hideInInspector;
			Inverse = inverse;
		}

		public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse, int enumValueIndex)
		{
			ConditionalSourceField = conditionalSourceField;
			HideInInspector = hideInInspector;
			Inverse = inverse;
			EnemValueIndex = enumValueIndex;
		}

		public ConditionalHideAttribute(bool hideInInspector = false)
		{
			ConditionalSourceField = "";
			HideInInspector = hideInInspector;
			Inverse = false;
		}

		public ConditionalHideAttribute(bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse, params string[] conditionalSourceFields)
		{
			ConditionalSourceFields = conditionalSourceFields;
			ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
			HideInInspector = hideInInspector;
			Inverse = inverse;
		}

		public ConditionalHideAttribute(bool hideInInspector, bool inverse, params string[] conditionalSourceFields)
		{
			ConditionalSourceFields = conditionalSourceFields;
			HideInInspector = hideInInspector;
			Inverse = inverse;
		}

		public ConditionalHideAttribute(bool hideInInspector, bool inverse, int enumValueIndex, params string[] conditionalSourceFields)
		{
			ConditionalSourceFields = conditionalSourceFields;
			HideInInspector = hideInInspector;
			Inverse = inverse;
			EnemValueIndex = enumValueIndex;
		}
	}
}