using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    string baseUrl = "http://localhost:8080/";

    IEnumerator SendWebRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Web Request Error] {request.error}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    UnityWebRequest CreateRequest(string method, string path, string json = null)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + path, method);
        request.downloadHandler = new DownloadHandlerBuffer();
        if (json != null)
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        }

        //토큰이 있으면 access token 실어보내기
        string token = PlayerManager.GetInstance().getToken();
        if (token != null)
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
        }
        return request;
    }

    public void HTTPCall(string method, string path, string jsonfile = null)
    {
        StartCoroutine(SendWebRequest(CreateRequest(method, path, jsonfile)));
    }
}
