using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// TabNavigation 클래스는 Unity UI에서 Tab 키를 이용한 입력 필드 간의 순환 이동을 가능하게 함.
/// 사용자는 Tab 키를 눌러 지정된 입력 필드 배열(inputs) 내의 다음 필드로 포커스를 이동시킬 수 있음.
/// 배열의 마지막 입력 필드에서 Tab을 누르면, 배열의 첫 번째 입력 필드로 포커스가 이동함.
/// </summary>
public class Tab_Navigation : MonoBehaviour
{
    // 입력 필드 배열
    [Header("[ 입력 필드 배열 ]")]
    public Selectable[] inputs;

    void Update()
    {
        // PopUp이 띄워저 있는지 확인
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
                        // 현재 선택된 입력 필드의 다음 입력 필드를 계산
                        // 배열의 마지막 입력 필드에서 Tab을 누르면, 첫 번째 입력 필드로 돌아감
                        Selectable next = inputs[(index + 1) % inputs.Length];
                        if (next != null)
                        {
                            // 다음 입력 필드가 InputField 컴포넌트를 가지고 있으면, 포인터 클릭 이벤트를 시뮬레이션 함
                            InputField inputfield = next.GetComponent<InputField>();
                            if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(EventSystem.current));

                            // EventSystem을 통해 다음 입력 필드로 포커스를 이동
                            EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
                        }
                    }
                }
            }
            else
            {
                // 현재 선택된 UI 요소가 없을 때의 처리. 예를 들어, 첫 번째 입력 필드로 포커스를 설정할 수 있습니다.
                if (inputs.Length > 0 && inputs[0] != null)
                {
                    // 첫 번째 입력 필드로 포커스를 이동
                    EventSystem.current.SetSelectedGameObject(inputs[0].gameObject, new BaseEventData(EventSystem.current));
                }
            }
        }
    }

}
