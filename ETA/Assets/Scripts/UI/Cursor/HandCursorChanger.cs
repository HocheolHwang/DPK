using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Button UI 오브젝트에 마우스 커서가 들어오거나 나갈 때, 또는 클릭했을 때 커서 모양을 변경하는 컴포넌트입니다.
/// </summary>
public class HandCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 손가락 커서 이미지를 저장하는 변수
    private static Texture2D handCursor;

    private void Start()
    {
        // 손가락 커서 이미지를 Resources에서 로드하여 handCursor 변수에 저장
        handCursor = Resources.Load<Texture2D>("Sprites/7. Common Sprites/Cursor Sprites/Hand Cursor");
    }

    // 마우스가 UI 오브젝트에 들어갈 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스 커서를 손가락 모양으로 변경
        Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
    }

    // 마우스가 UI 오브젝트에서 나갈 때 호출되는 메서드
    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스 커서를 기본 모양으로 변경
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // UI 오브젝트를 클릭했을 때 호출되는 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 커서를 기본 모양으로 변경
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
