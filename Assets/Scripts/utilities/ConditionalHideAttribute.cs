using System;
using UnityEngine;

namespace utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = "";
        public string ConditionalSourceField2 = "";
        public string[] ConditionalSourceFields = new string[] { };
        public bool[] ConditionalSourceFieldInverseBools = new bool[] { };
        public bool HideInInspector = false;
        public bool Inverse = false;
        public bool UseOrLogic = false;
        public int EnemValueIndex = 0;

        public bool InverseCondition1 = false;
        public bool InverseCondition2 = false;


        // Use this for initialization
        public ConditionalHideAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
            this.Inverse = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, int enumValueIndex)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
            this.Inverse = false;
            this.EnemValueIndex = enumValueIndex;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = false;
        }
        
        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, int enumValueIndex)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = false;
            this.EnemValueIndex = enumValueIndex;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse, int enumValueIndex)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
            this.EnemValueIndex = enumValueIndex;
        }

        public ConditionalHideAttribute(bool hideInInspector = false)
        {
            this.ConditionalSourceField = "";
            this.HideInInspector = hideInInspector;
            this.Inverse = false;
        }

        public ConditionalHideAttribute(bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse, params string[] conditionalSourceFields)
        {
            this.ConditionalSourceFields = conditionalSourceFields;
            this.ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }

        public ConditionalHideAttribute(bool hideInInspector, bool inverse, params string[] conditionalSourceFields)
        {
            this.ConditionalSourceFields = conditionalSourceFields;        
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }

        public ConditionalHideAttribute(bool hideInInspector, bool inverse, int enumValueIndex, params string[] conditionalSourceFields)
        {
            this.ConditionalSourceFields = conditionalSourceFields;        
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
            this.EnemValueIndex = enumValueIndex;
        } 
    }
}



