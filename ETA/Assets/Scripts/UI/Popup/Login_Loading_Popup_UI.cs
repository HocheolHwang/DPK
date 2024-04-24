using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Login_Loading_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Tip_Content_Text
    }

    // 클래스 멤버 변수로 선언
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

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 경고 문구
        tipContent = GetText((int)Texts.Tip_Content_Text);

        // 랜덤한 팁 텍스트 설정
        SetRandomTip();
    }

    // 랜덤한 팁을 설정하는 메서드
    private void SetRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length); // 랜덤 인덱스 생성
        tipContent.text = tips[randomIndex]; // 랜덤 텍스트 선택하여 할당
    }
}
