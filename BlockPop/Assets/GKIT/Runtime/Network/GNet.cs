using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GKIT.Runtime.Network
{
    public class GNet
    {

        public static bool IsAvailable
        {
            get
            {
                switch (Application.internetReachability)
                {
                    case NetworkReachability.NotReachable: return false;
                }
                return true;
            }
        }


        public static int NETWORK_TIMEOUT = 20;

        /// <summary>
        /// Get 方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public static void Get(string url, Action<GNetResponse> callback,  int timeout = 20)
        {
            UnityWebRequest req = UnityWebRequest.Get(url);
            req.timeout = timeout;
            SendRequest(req, callback);
        }
        
        public static UnityWebRequestAsyncOperation Get(string url, int timeout = 20)
        {
            UnityWebRequest req = UnityWebRequest.Get(url);
            req.timeout = timeout;
            return req.SendWebRequest();
        }

        /// <summary>
        /// Post 方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public static void Post(string url, Dictionary<string, string> postData, Action<GNetResponse> callback,  int timeout = 20)
        {
            UnityWebRequest req = UnityWebRequest.Post(url, postData);
            req.timeout = timeout;
            SendRequest(req, callback);
        }
        
        public static void Post(string url, WWWForm form, Action<GNetResponse> callback,  int timeout = 20)
        {
            UnityWebRequest req = UnityWebRequest.Post(url, form);
            req.timeout = timeout;
            SendRequest(req, callback);
        }

        public static UnityWebRequestAsyncOperation Post(string url, Dictionary<string, string> postData, int timeout = 20)
        {
            UnityWebRequest req = UnityWebRequest.Post(url, postData);
            req.timeout = timeout;
            return req.SendWebRequest();
        }
        

        /// <summary>
        /// 发送网络请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static async void SendRequestAsync(UnityWebRequest request, Action<GNetResponse> callback)
        {
            request.timeout = NETWORK_TIMEOUT;
            var ao = request.SendWebRequest();
            await ao;
            GNetResponse resp = new GNetResponse();
            resp.code = (int)request.responseCode;
            resp.url = request.url;
            resp.method = request.method;
            resp.error = request.error;
            if (request.downloadHandler != null)
            {
                resp.data = request.downloadHandler.data;
                resp.text = request.downloadHandler.text;
            }
            callback?.Invoke(resp);
        }
        
        public static void SendRequest(UnityWebRequest request, Action<GNetResponse> callback)
        {
            request.timeout = NETWORK_TIMEOUT;
            var webRequest = request.SendWebRequest();
            webRequest.completed += ao =>
            {
                if (ao.isDone)
                {
                    GNetResponse resp = new GNetResponse();
                    resp.code = (int)request.responseCode;
                    resp.url = request.url;
                    resp.method = request.method;
                    resp.error = request.error;
                    if (request.downloadHandler != null)
                    {
                        resp.data = request.downloadHandler.data;
                        resp.text = request.downloadHandler.text;
                    }
                    callback?.Invoke(resp);
                }
            };
        }
    }

    /// <summary>
    /// 请求返回
    /// </summary>
    public class GNetResponse
    {
        public string url;
        public int code;
        public string method;
        public string text;
        public byte[] data;
        public string error;
        public bool Success => code == 200;
    }
    
    /// <summary>
    /// 用于扩展AO, 返回GetAwaiter这个方法
    /// </summary>
    public static class ExtensionMethods
    {
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj => { tcs.SetResult(null); };
            return ((Task)tcs.Task).GetAwaiter();
        }
    }
    
    
    
    
}