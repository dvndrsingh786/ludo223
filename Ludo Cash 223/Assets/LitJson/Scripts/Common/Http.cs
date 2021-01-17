//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace LJR
{
    /// <summary>
    /// 通信実行クラス
    /// </summary>
	public static class Http
    {
        static Http()
        {
            // LitJsonで long, float が使えない問題の解消
            JsonMapper.RegisterExporter<float>((obj, writer) => { writer.Write(Convert.ToDouble(obj)); });
            JsonMapper.RegisterImporter<double, float>((input) => { return Convert.ToSingle(input); });
            JsonMapper.RegisterImporter<Int32, long>((input) => { return Convert.ToInt64(input); });
        }
        /// <summary>
        /// 通信接続
        /// </summary>
        /// <typeparam name="TRequest">リクエストクラス</typeparam>
        /// <typeparam name="TResponce">レスポンスクラス</typeparam>
        /// <param name="request">リクエスト</param>
        /// <param name="url">URL</param>
        /// <param name="get">GETパラメータ</param>
        /// <param name="post">POSTパラメータ</param>
        /// <param name="onFinish">通信終了時コールバック</param>
        public static void Send<TRequest, TResponce>(TRequest request, string url, Dictionary<string, string> get, Dictionary<string, string> post, Action<TRequest, TResponce> onFinish)
            where TRequest : Request<TResponce>
            where TResponce : Responce
        {
            request.StartCoroutine(SendRequest(request, url, get, post, onFinish));
        }

        /// <summary>
        /// 通信接続
        /// </summary>
        /// <typeparam name="TRequest">リクエストクラス</typeparam>
        /// <typeparam name="TResponce">レスポンスクラス</typeparam>
        /// <param name="request">リクエスト</param>
        /// <param name="url">URL</param>
        /// <param name="get">GETパラメータ</param>
        /// <param name="post">POSTパラメータ</param>
        /// <param name="onFinish">通信終了時コールバック</param>
        /// <returns>IEnumerator</returns>
        private static IEnumerator SendRequest<TRequest, TResponce>(TRequest request, string url, Dictionary<string, string> get, Dictionary<string, string> post, Action<TRequest, TResponce> onFinish)
            where TRequest : Request<TResponce>
            where TResponce : Responce
        {
            // 通信データ初期化
            request.Status = RequestStatus.None;
            request.Message = "";
			request.Exception = null;
            request.Responce = null;
            
            // URL作成
            url = Setting.BaseUrl + url;
			foreach (var pair in get)
			{
				url += ((!url.Contains("?")) ? "?" : "&") + pair.Key + "=" + pair.Value;
			}
            
            // 通信実行
            WWW www;
            if (post.Count == 0)
            {
                www = new WWW(url);
            }
            else
            {
                WWWForm form = new WWWForm();
                foreach (var pair in post)
                {
                    form.AddField(pair.Key, pair.Value);
                }
                www = new WWW(url, form);
            }

            // 通信完了を待つ
            while (!www.isDone && string.IsNullOrEmpty(www.error))
            {
                yield return null;
            }

            // 通信結果
            if (string.IsNullOrEmpty(www.error))
            {
                // 通信成功
                try
                {
                    // レスポンスクラス取得
                    request.Status = RequestStatus.OK;
					request.Responce = JsonMapper.ToObject<TResponce>(www.text);
                }
                catch(Exception e)
                {
                    // 予期せぬエラー(大抵レスポンスのフォーマットが間違ってる)
                    request.Status = RequestStatus.Error;
					request.Message = e.StackTrace;
					request.Exception = e;
                    request.Responce = null;
                }
            }
            else
            {
                // 通信失敗
                request.Status = RequestStatus.Error;
				request.Message = www.error;
				request.Exception = new Exception(www.error);
				request.Responce = null;
            }

            // 終了
            if (onFinish != null) { onFinish(request, request.Responce); }

#if UNITY_EDITOR
            // テスト通信の場合ログ出力
            if (request.IsTest)
            {
                request.IsTest = false;
                Debug.Log("[" + typeof(TRequest).Name + "/" + typeof(TResponce).Name + "]");
                Debug.Log(url);
                Debug.Log("Status:" + request.Status);
                if (request.Exception != null) { Debug.LogException(request.Exception); }
            }
#endif

			// 破棄
			www.Dispose ();
			www = null;
        }
    }
}
