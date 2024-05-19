using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Input Field UI에서 Tab 키를 이용해 Input Field 간의 순환 이동을 가능하게 하는 컴포넌트입니다.
/// 배열의 마지막 Input Field에서 Tab을 누르면, 배열의 첫 번째 Input Field로 포커스가 이동합니다.
/// </summary>
public class Tab_Navigation : MonoBehaviour
{
    // 입력 필드 배열
    [Header("[ 입력 필드 배열 ]")]
    public Selectable[] inputs;

    void Update()
    {
        // Popup UI가 띄워저 있는지 확인
        if (Managers.UI.GetTopPopupUI() != null)
        {
            // PopUp이 띄워져있다면 가장 위에 띄워져있는 Popup인지 확인
            if (Managers.UI.GetTopPopupUI() != transform.GetComponentInParent<UI_Popup>()) return;
        }

        // Tab 키가 눌렸을 때의 동작을 처리
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 현재 선택된 UI 요소가 null이 아닌지 확인
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                // 현재 선택된 UI 요소를 가져옴
                Selectable current = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
                if (current != null)
                {
                    // 현재 선택된 요소가 inputs 배열 내에 있는지 확인하고, 그 인덱스를 찾음
                    int index = System.Array.IndexOf(inputs, current);
                    if (index >= 0)
                    {
                        // 현재 선택된 Input Field의 다음 Input Field를 계산
                        // 배열의 마지막 Input Field에서 Tab을 누르면, 첫 번째 Input Field로 돌아감
                        Selectable next = inputs[(index + 1) % inputs.Length];
                        if (next != null)
                        {
                            // 다음 Input Field가 Input Field 컴포넌트를 가지고 있으면, 포인터 클릭 이벤트를 시뮬레이션 함
                            InputField inputfield = next.GetComponent<InputField>();
                            if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(EventSystem.current));

                            // 다음 Input Field로 포커스를 이동
                            EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
                        }
                    }
                }
            }
            else
            {
                // 현재 선택된 UI 요소가 없을 경우
                if (inputs.Length > 0 && inputs[0] != null)
                {
                    // 첫 번째 Input Field로 포커스를 이동
                    EventSystem.current.SetSelectedGameObject(inputs[0].gameObject, new BaseEventData(EventSystem.current));
                }
            }
        }
    }

}
