using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class HTTPRequest : MonoBehaviour
{
    string url = "https://j10e107.p.ssafy.io/api/";
    //string port = "443";

    // GET
    // 경로만 지정
    IEnumerator GET(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + path);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) // Unity 2020.1 이후부터는 isNetworkError와 isHttpError 대신 result 사용
        {
            if (path.Equals("user/profile"))
            {

            }

            //Debug.Log(request.error);
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
    IEnumerator POST(string path, Dictionary<string, string> postParam)
    {
        // Dictionary를 직접 JSON 문자열로 변환
        StringBuilder jsonDataBuilder = new StringBuilder("{");
        foreach (var item in postParam)
        {
            jsonDataBuilder.Append($"\"{item.Key}\":\"{item.Value}\",");
        }
        if (jsonDataBuilder.Length > 1) // 마지막 쉼표를 제거하기 위함
        {
            jsonDataBuilder.Remove(jsonDataBuilder.Length - 1, 1);
        }
        jsonDataBuilder.Append("}");
        string jsonData = jsonDataBuilder.ToString();

        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
        UnityWebRequest postRequest = new UnityWebRequest(url + path, "POST");
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("error" + postRequest.error);
            //Debug.Log("result" + postRequest.result);
            UserData data = JsonUtility.FromJson<UserData>(postRequest.downloadHandler.text);

        }
        else // 통신 성공
        {

            if (path.Equals("user")) // 회원가입일 경우
            {
                // 로그인 화면으로 이동
            }
            else if (path.Equals("auth/login")) // 로그인일 경우
            {
                UserData data = JsonUtility.FromJson<UserData>(postRequest.downloadHandler.text);

                GetCall("user/profile");
            }
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

    public void POSTCall(string path, Dictionary<string, string> postParam)
    {
        StartCoroutine(POST(path, postParam));
    }
}
