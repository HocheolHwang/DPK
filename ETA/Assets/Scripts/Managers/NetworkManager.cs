using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    //string baseUrl = "https://localhost:8080/api/v1/";
    string baseUrl = "https://k10e207.p.ssafy.io/api/v1/";

    IEnumerator SendWebRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
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

        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        Debug.Log("메세지 출력" + message.message);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Web Request Error] {request.error}");
        }
        else
        {
            
            Debug.Log("Response: " + request.downloadHandler.text);
            PlayerResDto data = JsonUtility.FromJson<PlayerResDto>(request.downloadHandler.text);
            PlayerManager.GetInstance().SetToken(data.accessToken);
            PlayerManager.GetInstance().SetGold(data.playerGold);

            // 토큰에서 닉네임 파싱
            JWTDecord.DecodeJWT(data.accessToken);
        }
    }

    // 던전 클리어 랭크
    IEnumerator DungeonRankRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
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

    // 플레이어 랭크
    IEnumerator PlayerRankRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Web Request Error] {request.error}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            PlayerRankResDto data = JsonUtility.FromJson<PlayerRankResDto>(request.downloadHandler.text);
            // 플레이어 랭크
        }
    }

    // 골드 변화
    IEnumerator GoldStatisticsRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Gold Request Error] {request.error}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            GoldStatisticsResDto data = JsonUtility.FromJson<GoldStatisticsResDto>(request.downloadHandler.text);

            // 골드 변화
            PlayerManager.GetInstance().SetGold(data.currendGold);
        }
    }

    // 현재 직업 불러오기
    IEnumerator CurrentClassRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Gold Request Error] {request.error}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            ClassReqDto data = JsonUtility.FromJson<ClassReqDto>(request.downloadHandler.text);

            // 직업 변화
            PlayerManager.GetInstance().SetClassCode(data.classCode);
        }
    }


    UnityWebRequest CreateRequest(string method, string path, string json = null)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + path, method);

        Debug.Log(baseUrl + path);
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

    // 로그인시 호출되는 함수
    public void SignInCall(PlayerSignInReqDto dto)
    {
        string loginData = JsonUtility.ToJson(dto);
        StartCoroutine(LoginRequest(CreateRequest("POST", "auth/sign-in", loginData)));
    }

    // 회원가입시 호출되는 함수
    public void SignUpCall(PlayerSignUpReqDto dto)
    {
        string registData = JsonUtility.ToJson(dto);
        Debug.Log("json : " + registData);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "auth/sign-up", registData)));
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

    // 플레이어 랭킹
    public void PlayerRankCall(int limit)
    {
        // 쿼리스트링으로 갯수 보내기
        // 없으면 10개라고 함
        StartCoroutine(PlayerRankRequest(CreateRequest("GET", "player/ranking?limit=" + limit)));
    }

    // 플레이어 골드 변화량
    // response로 현재 골드량 온다
    public void GoldStatisticsCall(GoldStatisticsResDto dto)
    {
        string goldData = JsonUtility.ToJson(dto);
        StartCoroutine(GoldStatisticsRequest(CreateRequest("PUT", "player/gold", goldData)));
    }

    // 플레이어 경험치 변화량 전송
    public void EXPStatisticsCall(EXPStatisticsReqDto dto)
    {
        string expData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("PUT", "palyer/exp", expData)));
    }

    // 직업 불러오기
    public void CurrentClassCall()
    {
        StartCoroutine(CurrentClassRequest(CreateRequest("PUT", "class/current")));
    }

    // 직업 불러오기
    public void SelectClassCall(ClassReqDto dto)
    {
        string classData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "class/select", classData)));
    }
}

[System.Serializable]
class ResponseMessage
{
    public string message;
}