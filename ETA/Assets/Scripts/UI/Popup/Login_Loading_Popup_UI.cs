using UnityEngine;
using TMPro;

public class Login_Loading_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Texts
    {
        Tip_Content_Text
    }

    // UI 컴포넌트 바인딩 변수
    private TextMeshProUGUI tipContent;

    // 팁 텍스트 리스트
    private string[] tips = new string[]
    {
        "게임을 즐기세요!",
        "적절한 타이밍에 카운터 스킬을 사용해보세요.",
        "친구와 함께 플레이하면 더 재미있어요!",
        "콜라보 스킬은 매우 강력해요.",
        "플레이어는 자동으로 전투해요.",
        "공용 스킬은 누구나 사용할 수 있어요.",
        "[던전처리기사] 자격증을 잃어버리지 마세요!",
        "레벨을 올리면 스킬 포인트를 얻을 수 있어요."
        // 팁 추가 가능
    };


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 경고 문구 설정
        tipContent = GetText((int)Texts.Tip_Content_Text);

        // 랜덤한 팁 설정
        SetRandomTip();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 랜덤한 팁을 설정하는 메서드
    private void SetRandomTip()
    {
        // 랜덤 인덱스 생성
        int randomIndex = Random.Range(0, tips.Length);
        
        // 랜덤 텍스트 선택하여 할당
        tipContent.text = tips[randomIndex];
    }
}
