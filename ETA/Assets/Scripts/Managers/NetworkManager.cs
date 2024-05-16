using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class NetworkManager : MonoBehaviour
{
    //string baseUrl = "http://localhost:8080/api/v1/";
    string baseUrl = "https://k10e207.p.ssafy.io/api/v1/";

    IEnumerator SendWebRequest(UnityWebRequest request, Action callback = null)
    {
        yield return request.SendWebRequest();

        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Request Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            if(callback != null)
                callback?.Invoke();
            //Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
    IEnumerator SelectClassRequest(UnityWebRequest request, Action callback)
    {
        yield return request.SendWebRequest();

        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Request Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            //Debug.Log(request.result);
            //Debug.Log("Response: " + request.downloadHandler.text);
            callback?.Invoke();

        }
    }
    IEnumerator SendPartyRequest(UnityWebRequest request, Action callback = null)
    {
        yield return request.SendWebRequest();

        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Request Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            //Debug.Log(request.result);
            //Debug.Log("Response: " + request.downloadHandler.text);

            if(callback != null)
                callback?.Invoke();
        }
    }

    // 회원가입
    IEnumerator SignUpRequest(UnityWebRequest request, Action<string> callback)
    {
        yield return request.SendWebRequest();
        
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            // 회원가입 실패
            Debug.LogError($"[Request Error] {request.error}\n{request.downloadHandler.text}");

            // 에러 메세지 전달
            callback?.Invoke(message.message);
        }
        else
        {
            // 회원가입 성공
            //Debug.Log(request.result);
            //Debug.Log("Response: " + request.downloadHandler.text);

            // 성공 메세지 전달
            callback?.Invoke(message.message);
        }
    }

    // 로그인
    IEnumerator LoginRequest(UnityWebRequest request, Action<PlayerResDto> callback)
    {
        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
        //ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            // 로그인 실패
            Debug.LogError($"[Login Error] {request.error}\n{request.downloadHandler.text}");

            PlayerResDto data = JsonUtility.FromJson<PlayerResDto>(request.downloadHandler.text);

            // 에러 메세지 전달
            callback?.Invoke(data);
        }
        else
        {
            // 로그인 성공
            Debug.Log("Response: " + request.downloadHandler.text);
            PlayerResDto data = JsonUtility.FromJson<PlayerResDto>(request.downloadHandler.text);
            //PlayerManager.GetInstance().SetToken(data.accessToken);
            //PlayerManager.GetInstance().SetGold(data.playerGold);

            // 성공 메세지 전달
            callback?.Invoke(data);
        }
    }

    // 던전 클리어 랭크
    IEnumerator DungeonRankRequest(UnityWebRequest request, Action<DungeonRankListResDto> callback)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Rank Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            DungeonRankListResDto data = JsonUtility.FromJson<DungeonRankListResDto>(request.downloadHandler.text);
            //랭크 저장할 곳 필요함
            callback?.Invoke(data);
        }
    }

    // 플레이어 랭크
    IEnumerator PlayerRankRequest(UnityWebRequest request, Action<PlayerRankResDto> callback)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Rank Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            PlayerRankResDto data = JsonUtility.FromJson<PlayerRankResDto>(request.downloadHandler.text);

            // 플레이어 랭크
            callback?.Invoke(data);
        }
    }

    // 골드 변화
    IEnumerator GoldStatisticsRequest(UnityWebRequest request, Action<GoldStatisticsResDto> callback)
    {
        yield return request.SendWebRequest();
        ResponseMessage message = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Request Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log("Response: " + request.downloadHandler.text);
            GoldStatisticsResDto data = JsonUtility.FromJson<GoldStatisticsResDto>(request.downloadHandler.text);

            // 골드 변화
            //PlayerManager.GetInstance().SetGold(data.currendGold);
            callback?.Invoke(data);
        }
    }

    // 현재 직업 불러오기
    IEnumerator CurrentClassRequest(UnityWebRequest request, Action<CurClassDto> callback)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Current Class Error] {request.error}\n{request.downloadHandler.text}");
            CurClassDto data = new CurClassDto();

            // 없다면 디폴트 값을 설정해줘야함
            callback?.Invoke(data);
        }
        else
        {
            //Debug.Log("Response: " + request.downloadHandler.text);
            CurClassDto data = JsonUtility.FromJson<CurClassDto>(request.downloadHandler.text);

            // 직업 변화 
            callback?.Invoke(data);
        }
    }

    // 현재 스킬 저장하기
    IEnumerator LearnSkillRequest(UnityWebRequest request, Action callback)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Learn Skill Request Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            //Debug.Log("Response: " + request.downloadHandler.text);

            // 직업 변화 
            callback?.Invoke();
        }
    }
    // 모든 스킬 받아오기
    IEnumerator AllSkillRequest(UnityWebRequest request, Action<SkillResDto> callback)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Current Class Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            //Debug.Log("Response: " + request.downloadHandler.text);
            SkillResDto data = JsonUtility.FromJson<SkillResDto>(request.downloadHandler.text);

            // 직업 변화 
            callback?.Invoke(data);
        }
    }
    // 모든 레벨 받아오기
    IEnumerator AllLevelRequest(UnityWebRequest request, Action<AllCalssLevelResDto> callback)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Current Class Error] {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            AllCalssLevelResDto data = JsonUtility.FromJson<AllCalssLevelResDto>(request.downloadHandler.text);

            // 직업 변화 
            callback?.Invoke(data);
        }
    }


    UnityWebRequest CreateRequest(string method, string path, string json = null)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + path, method);

        //Debug.Log(baseUrl + path);
        //Debug.Log(json);
        request.downloadHandler = new DownloadHandlerBuffer();
        if (json != null)
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        }

        //토큰이 있으면 access token 실어보내기
        string token = Managers.Player.GetToken();
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
    public void SignInCall(PlayerSignInReqDto dto, Action<PlayerResDto> callback)
    {
        string loginData = JsonUtility.ToJson(dto);
        StartCoroutine(LoginRequest(CreateRequest("POST", "auth/sign-in", loginData), callback));
    }

    // 회원가입시 호출되는 함수
    public void SignUpCall(PlayerSignUpReqDto dto, Action<string> callback)
    {
        string registData = JsonUtility.ToJson(dto);
        StartCoroutine(SignUpRequest(CreateRequest("POST", "auth/sign-up", registData), callback));
    }

    // 파티 생성 요청
    public void CreatePartyCall(PartyReqDto dto, Action callback)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendPartyRequest(CreateRequest("POST", "party", partyData), callback));
    }

    // 파티 참가 요청
    public void EnterPartyCall(DungeonReqDto dto)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "party/enter", partyData)));
    }
    
    // 던전 결과 전송
    public void EndDungeonCall(DungeonReqDto dto, Action callback = null)
    {
        string partyData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("POST", "dungeon/end", partyData), callback));
    }

    // 던전 결과 랭크
    public void DungeonRankCall(string dungeonCode, Action<DungeonRankListResDto> callback)
    {
        //Debug.Log("dungeon/rank?dungeon-code=D00" + dungeonCode);
        StartCoroutine(DungeonRankRequest(CreateRequest("GET", "dungeon/ranking?dungeon-code=D00"+dungeonCode), callback));
    }

    // 플레이어 랭킹
    public void PlayerRankCall(int limit, Action<PlayerRankResDto> callback)
    {
        // 쿼리스트링으로 갯수 보내기
        // 없으면 10개라고 함
        StartCoroutine(PlayerRankRequest(CreateRequest("GET", "player/ranking?limit=" + limit), callback));
    }

    // 플레이어 골드 변화량
    // response로 현재 골드량 온다
    public void GoldStatisticsCall(GoldStatisticsResDto dto, Action<GoldStatisticsResDto> callback)
    {
        string goldData = JsonUtility.ToJson(dto);
        StartCoroutine(GoldStatisticsRequest(CreateRequest("PUT", "player/gold", goldData), callback));
    }

    // 플레이어 경험치 변화량 전송
    public void EXPStatisticsCall(EXPStatisticsReqDto dto, Action callback = null)
    {
        Debug.Log("경험치 변화 있나??");
        string expData = JsonUtility.ToJson(dto);
        StartCoroutine(SendWebRequest(CreateRequest("PUT", "player/exp", expData), callback));
    }

    // 직업 불러오기
    public void CurrentClassCall(Action<CurClassDto> callback)
    {
        StartCoroutine(CurrentClassRequest(CreateRequest("GET", "class/current"), callback));
    }

    // 직업 저장하기
    public void SelectClassCall(ClassReqDto dto, Action callback)
    {
        string classData = JsonUtility.ToJson(dto);
        StartCoroutine(SelectClassRequest(CreateRequest("POST", "class/select", classData), callback));
    }
    // 스킬 저장하기
    public void LearnSkillCall(SkillReqDto dto, Action callback)
    {
        string classData = JsonUtility.ToJson(dto);
        StartCoroutine(LearnSkillRequest(CreateRequest("PUT", "skill/learned", classData), callback));
    }
    // 스킬 불러오기
    public void AllSkillCall(Action<SkillResDto> callback)
    {
        StartCoroutine(AllSkillRequest(CreateRequest("GET", "skill/learned"), callback));
    }
    // 모든 레벨 불러오기
    public void AllLevelCall(Action<AllCalssLevelResDto> callback)
    {
        StartCoroutine(AllLevelRequest(CreateRequest("GET", "class"), callback)); ;
    }
}

[System.Serializable]
class ResponseMessage
{
    public string message;
}