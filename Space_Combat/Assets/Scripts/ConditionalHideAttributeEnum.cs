using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: Sebastian Lague

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideEnumAttribute : PropertyAttribute
{
    public string conditionalSourceField;
    public int enumIndex;

    public ConditionalHideEnumAttribute(string boolVariableName)
    {
        conditionalSourceField = boolVariableName;
    }

    public ConditionalHideEnumAttribute(string enumVariableName, int enumIndex)
    {
        conditionalSourceField = enumVariableName;
        this.enumIndex = enumIndex;
    }

}