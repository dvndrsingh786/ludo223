//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
//----------------------------------------------
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace LJR
{
	/// <summary>
	/// Inspector上に任意のメソッドを実行するボタンを表示
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public sealed class MethodButtonAttribute : PropertyAttribute
	{
		public string Label;
		public string Method;
		public object[] Args;

        public MethodButtonAttribute(string label, string method, params object[] args)
		{
			Label = label;
			Method = method;
			Args = args;
		}
	}

    [CustomPropertyDrawer(typeof(MethodButtonAttribute))]
    public sealed class MethodButtonDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
            var attr = attribute as MethodButtonAttribute;
			if (GUI.Button(position, attr.Label))
			{
				try
				{
                    UnityEngine.Object obj = property.serializedObject.targetObject;
                    MethodInfo mi = obj.GetType().GetMethod(attr.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                    mi.Invoke(obj, attr.Args);
				}
				catch(Exception e)
				{
					Debug.LogException (e);
				}
			}
		}
	}
}
#endif
