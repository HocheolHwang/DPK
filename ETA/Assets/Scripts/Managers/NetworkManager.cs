using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    string baseUrl = "http://localhost:8080/api/v1/";

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

    // 로그인
    IEnumerator LoginRequest(UnityWebRequest request)
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
            ResponseDto data = JsonUtility.FromJson<ResponseDto>(request.downloadHandler.text);
            PlayerManager.GetInstance().SetToken(data.accessToken);
        }
    }
    // 던전 클리어 랭크
    IEnumerator DungeonRankRequest(UnityWebRequest request)
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
            DungeonResDto data = JsonUtility.FromJson<DungeonResDto>(request.downloadHandler.text);
            //랭크 저장할 곳 필요함
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
        string token = PlayerManager.GetInstance().GetToken();
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

    // 회원가입시 호출되는 함수
    public void SignInCall(UserInfoDto dto)
    {
        string loginData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "auth/sign-in", loginData)));
    }

    // 로그인시 호출되는 함수
    public void SignUpCall(UserInfoDto dto)
    {
        string loginData = JsonUtility.ToJson(dto);
        StartCoroutine(LoginRequest(CreateRequest("POST", "auth/sign-up", loginData)));
    }

    // 파티 생성 요청
    public void CreatePartyCall(PartyReqDto dto)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "party", partyData)));
    }

    // 파티 참가 요청
    public void EnterPartyCall(PartyResDto dto)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "party/enter", partyData)));
    }
    
    // 던전 결과 전송
    public void EnterDungeonCall(DungeonReqDto dto)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "dungeon/end", partyData)));
    }

    // 던전 결과 랭크
    public void EnterDungeonCall(string dungeonCode)
    {
        StartCoroutine(DungeonRankRequest(CreateRequest("GET", "dungeon/rank?dungeon-code="+dungeonCode)));
    }
}
