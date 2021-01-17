//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
// auto-generated
//----------------------------------------------
using UnityEngine;
using System;

namespace LJR
{
	public class RequestDemo2 : Request<ResponceDemo2>
	{
		public static readonly string URL = "Demo2.json";

		public string get_value = "";
		public string post_value = "";
		public Action<RequestDemo2, ResponceDemo2> onFinish = null;

		public void Send(string get_value, string post_value, Action<RequestDemo2, ResponceDemo2> onFinish)
		{
			this.get_value = get_value;
			this.post_value = post_value;
			this.onFinish = onFinish;
			Send();
		}

		public override void Send()
		{
			get.Clear ();
			post.Clear ();
			get.Add("get_value", get_value);
			post.Add("post_value", post_value);
			Http.Send(this, URL, get, post, onFinish);
		}
	}

	[Serializable]
	public class ResponceDemo2 : Responce
	{
		public string str_value;
		public string[] str_values;
		public int int_value;
		public int[] int_values;
		public long long_value;
		public long[] long_values;
		public float float_value;
		public float[] float_values;
		public double double_value;
		public double[] double_values;
		public bool bool_value;
		public bool[] bool_values;
		public Class_value class_value;
		public Class_values[] class_values;
	}

	[Serializable]
	public class Class_value
	{
		public int id;
		public string name;
	}

	[Serializable]
	public class Class_values
	{
		public int id;
		public string name;
		public Inner_class inner_class;
		public Game_data[] game_data;
	}

	[Serializable]
	public class Inner_class
	{
		public int id;
		public string name;
	}
}
