//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LJR
{
	public class SceneDemo : MonoBehaviour
	{
		public RequestDemo1 RequestDemo1;
		public RequestDemo2 RequestDemo2;
		public RequestDemo3 RequestDemo3;
		public Text Responce;

        void Start()
        {
            /*↓↓↓DEBUG↓↓↓*/
            Setting.BaseUrl = "file:// + " + Application.dataPath + "/LJR/Demo/Server/";
            /*↑↑↑DEBUG↑↑↑*/
        }

		public void OnClickDemo1()
		{
			string id = "1";
			RequestDemo1.Send(id, (req, res) =>
			{
				Responce.text = "Status=" + req.Status.ToString() + "\n";
				Responce.text += "Message=" + req.Message + "\n";
				Responce.text += "\n";
				if (req.Status == RequestStatus.OK)
				{
					Responce.text += "user_id=" + res.user_id + "\n";
					Responce.text += "user_name=" + res.user_name + "\n";
					Responce.text += "power_ratio=" + res.power_ratio + "\n";
					Responce.text += "game_data=" + res.game_data.game_version + "," + res.game_data.parameter[0];
				}
				else if (req.Status == RequestStatus.Error)
				{
					Responce.text += req.Exception.StackTrace;
				}
			});
		}

		public void OnClickDemo2()
		{
			string get_value = "1";
			string post_value = "2";
			RequestDemo2.Send(get_value, post_value, (req, res) =>
			{
				Responce.text = "Status=" + req.Status.ToString() + "\n";
				Responce.text += "Message=" + req.Message + "\n";
				Responce.text += "\n";
				if (req.Status == RequestStatus.OK)
				{
                    Responce.text += "str_value=" + res.str_value + "\n";
                    Responce.text += "str_values=" + res.str_values[0] + "," + res.str_values[1] + "\n";
                    Responce.text += "int_value=" + res.int_value + "\n";
                    Responce.text += "int_values=" + res.int_values[0] + "\n";
                    Responce.text += "long_value=" + res.long_value + "\n";
                    Responce.text += "long_values=" + res.long_values[0] + "\n";
                    Responce.text += "float_value=" + res.float_value + "\n";
                    Responce.text += "float_values=" + res.float_values[0] + "\n";
                    Responce.text += "double_value=" + res.double_value + "\n";
                    Responce.text += "double_values=" + res.double_values[0] + "\n";
                    Responce.text += "bool_value=" + res.bool_value + "\n";
                    Responce.text += "bool_values=" + res.bool_values[0] + "\n";
                    Responce.text += "class_value=" + res.class_value.id + "," + res.class_value.name + "\n";
                    Responce.text += "class_values.id=" + res.class_values[0].id + "\n";
                    Responce.text += "class_values.name=" + res.class_values[0].name + "\n";
                    Responce.text += "class_values.inner_class=" + res.class_values[0].inner_class.id + "," + res.class_values[0].inner_class.name + "\n";
                    Responce.text += "class_values.game_data0=" + res.class_values[0].game_data[0].game_version + "," + res.class_values[0].game_data[0].parameter[0] + "\n";
                    Responce.text += "class_values.game_data1=" + res.class_values[0].game_data[1].game_version + "," + res.class_values[0].game_data[1].parameter[0];
                }
				else if (req.Status == RequestStatus.Error)
				{
					Responce.text += req.Exception.StackTrace;
				}
			});
		}

		public void OnClickDemo3()
		{
			RequestDemo3.Send ((req, res) => 
			{
				Responce.text = "Status=" + req.Status.ToString() + "\n";
				Responce.text += "Message=" + req.Message + "\n";
				Responce.text += "\n";
				if (req.Status == RequestStatus.Error)
				{
					Responce.text += req.Exception.StackTrace;
				}
			});
		}
	}
}
