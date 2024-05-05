using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Party_Item : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // 파티 참가 확인 Popup UI 띄우기 버튼 변수
    private Button openPartyJoinConfirmButton;


    // ------------------------------ UI 초기화 ------------------------------
    private void Start()
    {
        // 버튼 컴포넌트 찾기
        openPartyJoinConfirmButton = GetComponentInChildren<Button>();

        // 클릭 이벤트에 메서드 연결
        openPartyJoinConfirmButton.onClick.AddListener(OpenPartyJoinConfirm);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 파티 참가 확인 Popup UI 띄우기 메서드
    private void OpenPartyJoinConfirm()
    {
        Transform partyInfo = gameObject.transform.GetChild(0);
        Managers.Photon.RoomName = partyInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        Debug.Log($"{partyInfo.name} 클릭함");

        Managers.UI.ShowPopupUI<Party_Join_Confirm_Popup_UI>("[Lobby]_Party_Join_Confirm_Popup_UI");
    }
}
