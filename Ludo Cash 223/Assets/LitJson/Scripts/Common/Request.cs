//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

namespace LJR
{
    /// <summary>
    /// リクエスト基底クラス
    /// </summary>
    /// <typeparam name="TResponce">レスポンスクラス</typeparam>
    public abstract class Request<TResponce> : MonoBehaviour where TResponce : Responce
	{
#if UNITY_EDITOR
        /// <summary>
        /// テストフラグ
        /// </summary>
        [MethodButton("Test", "OnTest")]
        public bool IsTest;
#endif
        /// <summary>
        /// ステータス
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message;
		/// <summary>
		/// エラー情報
		/// </summary>
		public Exception Exception;
        /// <summary>
        /// レスポンス
        /// </summary>
        public TResponce Responce;

		/// <summary>
		/// GETパラメータ
		/// </summary>
		protected Dictionary<string, string> get = new Dictionary<string, string>();
		/// <summary>
		/// POSTパラメータ
		/// </summary>
        protected Dictionary<string, string> post = new Dictionary<string, string>();

        /// <summary>
        /// 通信接続
        /// </summary>
        public abstract void Send();

#if UNITY_EDITOR
        /// <summary>
        /// テスト実行
        /// </summary>
        public void OnTest()
        {
            IsTest = true;
            Send();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
	}

    /// <summary>
    /// ステータス
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// 成功
        /// </summary>
        OK,
        /// <summary>
        /// エラー
        /// </summary>
        Error,
    }
}
