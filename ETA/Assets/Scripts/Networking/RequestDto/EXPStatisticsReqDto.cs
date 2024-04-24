using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EXPStatisticsReqDto
{
    // 내 직업
    public string classCode;

    // 얻음 경험치
    public int expDelta;

    // 현재 레벨
    public int playerLevel;

    // 현재 경험치
    public int currentExp;

    // 경험치 얻은 경로
    public string reason;
}
