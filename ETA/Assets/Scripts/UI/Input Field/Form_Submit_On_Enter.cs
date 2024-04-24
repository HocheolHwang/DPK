using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FormSubmitOnEnter 클래스는 Unity UI 폼에서 Enter 키를 누르는 것으로
/// 지정된 버튼의 클릭 이벤트를 발생시키는 기능을 제공함.
/// </summary>
public class Form_Submit_On_Enter : MonoBehaviour
{
    // 버튼
    [Header("[ 버튼 ]")]
    public Button submitButton; // 사용자가 Enter를 눌렀을 때 클릭되어야 할 버튼

    void Start()
    {
        Managers.Input.EnterKeyAction += SubmitForm;
    }

    void OnDestroy()
    {
        Managers.Input.EnterKeyAction -= SubmitForm;
    }

    private void SubmitForm()
    {
        // 지정된 버튼의 onClick 이벤트를 호출
        submitButton.onClick.Invoke();
    }
}
