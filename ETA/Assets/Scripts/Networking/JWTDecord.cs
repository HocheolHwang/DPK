using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
public class JWTDecord : MonoBehaviour
{
    public static string DecodeJWT(string token)
    {
        string[] parts = token.Split('.');
        if (parts.Length != 3)
        {
            throw new InvalidOperationException("JWT 형식이 올바르지 않습니다.");
        }

        string header = parts[0];
        string payload = parts[1];

        string decodedHeader = DecodeBase64(header);
        string decodedPayload = DecodeBase64(payload);
        //Debug.Log("Header: " + decodedHeader);
        //Debug.Log("Payload: " + decodedPayload);
        //Console.WriteLine("Header: " + decodedHeader);
        //Console.WriteLine("Payload: " + decodedPayload);

        PlayerSignUpReqDto data = JsonUtility.FromJson<PlayerSignUpReqDto>(decodedPayload);
        return data.nickname;
    }

    private static string DecodeBase64(string input)
    {
        string output = input;
        output = output.Replace('-', '+').Replace('_', '/');  // Base64 URL 문자를 일반 Base64 문자로 변환
        switch (output.Length % 4)  // 패딩 추가
        {
            case 2: output += "=="; break;
            case 3: output += "="; break;
        }
        var base64EncodedBytes = Convert.FromBase64String(output);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
