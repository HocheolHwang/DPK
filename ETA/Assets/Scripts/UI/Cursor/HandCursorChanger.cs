using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Button UI 오브젝트에 마우스 커서가 들어오거나 나갈 때, 또는 클릭했을 때 커서 모양을 변경하는 컴포넌트입니다.
/// </summary>
public class HandCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //private static Texture2D handCursor;

    //private void Start()
    //{
    //    handCursor = Resources.Load<Texture2D>("Sprites/7. Common Sprites/Cursor Sprites/Hand Cursor");
    //    // handCursor = Resources.Load<Texture2D>("Hand Cursor");
    //}

    [Header("[ 커서 이미지 ]")]
    public Texture2D handCursor; // 손가락 모양의 커서 이미지

    // 마우스가 UI 오브젝트에 들어갈 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto); // 마우스 커서를 손가락 모양으로 변경
    }

    // 마우스가 UI 오브젝트에서 나갈 때 호출되는 메서드
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 마우스 커서를 기본 모양으로 변경
    }

    // UI 오브젝트를 클릭했을 때 호출되는 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 마우스 커서를 기본 모양으로 변경
    }
}
