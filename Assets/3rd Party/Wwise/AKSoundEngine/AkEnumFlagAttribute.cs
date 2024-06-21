// dnSpy decompiler from Assembly-CSharp.dll class: AkEnumFlagAttribute
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class AkEnumFlagAttribute : PropertyAttribute
{
	public AkEnumFlagAttribute(Type type)
	{
		this.Type = type;
	}

	public Type Type;
}
