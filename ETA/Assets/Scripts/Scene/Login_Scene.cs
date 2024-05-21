using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login_Scene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Server_Selection_Popup_UI>("[Login]_Server_Selection_Popup_UI");
        // Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/Login", Define.Sound.BGM);
    }

    public override void Clear()
    {
        Debug.Log("<color=red>Login Scene Clear</color>");
    }

    public override void OnJoinedLobby()
    {
        // 로그인 성공, 사용자 정보 로드 끝
        // 포톤(서버)에 Connection후
        // 로비 입장시 UI가 뜬다.

        // 여기서 첫 로그인 유무에 따라 다른 행동을 해야한다.
        // 첫 로그인 일 경우
        if (Managers.Player.GetFirst())
        {
            // 방을 만들어줘 줘야함
            // 왜냐하면, TutorialScene으로 넘어갈거기 때문에
            Guid newGuid = Guid.NewGuid();
            Managers.Photon.MakeRoom(Managers.Player.GetPlayerId() + newGuid.ToString()); // 자동으로 참여 하게 됨
        }
        else // 첫 로그인이 아닐 경우
        {
            // 그냥 바로 게임시작 UI 띄워준다.
            // UI를 이용해서 입장 하자

            OnBeforeLobby();
        }
    }

    public override void OnJoinedRoom()
    {
        OnBeforeTutorial();
    }

    void OnBeforeTutorial()
    {
        Managers.UI.ClosePopupUI();

        Managers.Network.AllSkillCall(LoadSkills);
        Managers.Photon.CloseRoom();

        // GameSystem On
        Managers.Player.SetPartyLeader(true);
        GameObject gameSystem = PhotonNetwork.Instantiate("Prefabs/GameSystem", new Vector3(), new Quaternion());
        gameSystem.name = "GameSystem";
    }

    void OnBeforeLobby()
    {
        Managers.UI.ClosePopupUI();
        Managers.Network.AllSkillCall(LoadSkills);
        //Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");
    }

    void LoadSkills(SkillResDto responseBody)
    {
        foreach (var skill in responseBody.skills)
        {
            switch (skill.classCode)
            {
                case "C001":
                    Managers.Player.warriorSkills[skill.index] = new SkillInfo { skillCode = skill.skillCode, skillName = skill.skillName };
                    break;
                case "C002":
                    Managers.Player.archerSkills[skill.index] = new SkillInfo { skillCode = skill.skillCode, skillName = skill.skillName };
                    break;
                case "C003":
                    Managers.Player.mageSkills[skill.index] = new SkillInfo { skillCode = skill.skillCode, skillName = skill.skillName };
                    break;
                case "C000":
                    break;
            }
            
        }
        //for (int i = 0; i < 8; i++)
        //{
        //    //Debug.Log(Managers.Player.warriorSkills[i]?.skillName);
        //    Debug.Log($"아처 스킬 : {Managers.Player.archerSkills[i]?.skillName}");

        //}
        Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");
    }
}
