using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class Tutorial_Dialog_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Dialog_Content_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Next_Button
    }

    private TextMeshProUGUI dialogContent;

    private string[] dialogs = new string[]
    {
        "드디어 마지막 시험이다.",
        // 팁 추가 가능
    };

    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));

        dialogContent = GetText((int)Texts.Dialog_Content_Text);

        SetRandomTip();
    }

    private void SetRandomTip()
    {
        int randomIndex = Random.Range(0, dialogs.Length); // 랜덤 인덱스 생성
        dialogContent.text = dialogs[randomIndex]; // 랜덤 텍스트 선택하여 할당
    }
}
