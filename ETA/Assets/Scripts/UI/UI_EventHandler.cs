using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 이벤트 처리를 담당하는 클래스
/// </summary>
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    // 클릭 이벤트 발생 시 호출되는 액션
    public Action<PointerEventData> OnClickHandler = null;

    // 드래그 이벤트 발생 시 호출되는 액션
    public Action<PointerEventData> OnDragHandler = null;

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler == null) return;
        OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler == null) return;
        OnClickHandler.Invoke(eventData);
    }
}
