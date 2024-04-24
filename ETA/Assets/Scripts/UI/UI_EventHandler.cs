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

    // Enter 키 입력 이벤트 발생 시 호출되는 액션
    public Action OnEnterPressHandler = null;

    // Tab 키 입력 이벤트 발생시 호출
    public Action OnTabPressHandler = null;

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

    private void Update()
    {
        // Enter 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnEnterPressHandler?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressHandler?.Invoke();
        }
    }
}
