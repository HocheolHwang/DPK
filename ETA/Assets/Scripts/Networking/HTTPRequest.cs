using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class HTTPRequest : MonoBehaviour
{
    string url = "http://localhost:8080/";
    //string port = "443";

    // GET
    // 경로만 지정
    IEnumerator GET(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + path);

        // Access Token
        if (UserInfo.GetInstance().getToken()!= null)
            request.SetRequestHeader("Authorization", "Bearer " + UserInfo.GetInstance().getToken());

        yield return request.SendWebRequest();
        //Debug.Log(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success) // Unity 2020.1 이후부터는 isNetworkError와 isHttpError 대신 result 사용
        {
            if (path.Equals("user/profile"))
            {

            }

        }
        else
        {
            if (path.Equals("user/profile"))
            {
                UserData data = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

            }
        }

    }
    public void GetCall(string path)
    {
        StartCoroutine(GET(path));
    }

    // POST
    // url + path = uri
    // dictionary로 값 전달
    IEnumerator POST(string path, string jsonfile)
    {
        //byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonfile);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonfile);

        UnityWebRequest postRequest = new UnityWebRequest(url + path, "POST");
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json; charset=utf-8");


        // Access Token
        if (UserInfo.GetInstance().getToken() != null)
            postRequest.SetRequestHeader("Authorization", "Bearer " + UserInfo.GetInstance().getToken());

        yield return postRequest.SendWebRequest();

        if (postRequest.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("error" + postRequest.error);
            //Debug.Log("result" + postRequest.result);
        }
        else // 통신 성공
        {
            TESTClass data = JsonUtility.FromJson<TESTClass>(postRequest.downloadHandler.text);
            Debug.Log(data.name);
        }
    }


    [System.Serializable]
    class UserData
    {
        public string accessToken;
        public string id;
        public string nickname;
        public string error;
        public string[] message;
    }

    [System.Serializable]
    class TESTClass
    {
        public string name;
        public int data;
        public float time;
    }

    public void PostCall(string path, string postParam)
    {
        StartCoroutine(POST(path, postParam));
    }
}
