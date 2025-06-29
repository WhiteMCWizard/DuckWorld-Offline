using System;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector.Helpers;

public class RevertInfo
{
	private MonoBehaviour MonoBehaviour;

	private Type Type;

	private object Instance;

	private MemberInfo[] MemberInfo;

	private object value;

	public RevertMode RuntimeRevert => (MonoBehaviour as IRevertable).RuntimeRevertMode;

	public RevertMode EditorRevert => (MonoBehaviour as IRevertable).EditorRevertMode;

	public RevertInfo(MonoBehaviour monoBehaviour, Type type, string memberName, object value)
	{
		MonoBehaviour = monoBehaviour;
		Type = type;
		this.value = value;
		MemberInfo = Type.GetMember(memberName);
	}

	public RevertInfo(MonoBehaviour monoBehaviour, object obj, string memberName, object value)
	{
		MonoBehaviour = monoBehaviour;
		Instance = obj;
		Type = obj.GetType();
		this.value = value;
		MemberInfo = Type.GetMember(memberName);
	}

	public void Revert()
	{
		if (!(Instance.ToString() != "null") || MemberInfo == null || MemberInfo.Length <= 0)
		{
			return;
		}
		if (MemberInfo[0] is FieldInfo)
		{
			FieldInfo fieldInfo = MemberInfo[0] as FieldInfo;
			if (fieldInfo.IsStatic || (!fieldInfo.IsStatic && Instance != null))
			{
				fieldInfo.SetValue(Instance, value);
			}
		}
		else if (MemberInfo[0] is PropertyInfo)
		{
			PropertyInfo propertyInfo = MemberInfo[0] as PropertyInfo;
			propertyInfo.SetValue(Instance, value, null);
		}
		else if (MemberInfo[0] is MethodInfo)
		{
			MethodInfo methodInfo = MemberInfo[0] as MethodInfo;
			if (methodInfo.IsStatic || (!methodInfo.IsStatic && Instance != null))
			{
				object[] parameters = new object[1] { value };
				methodInfo.Invoke(Instance, parameters);
			}
		}
	}
}
