using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 이벤트 처리를 담당하는 클래스
/// </summary>
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,IDropHandler
{
    // 클릭 이벤트 발생 시 호출되는 액션
    public Action<PointerEventData> OnClickHandler = null;

    // 드래그 이벤트 발생 시 호출되는 액션
    public Action<PointerEventData> OnDragHandler = null;

    public Action<PointerEventData> OnBeginDragHandler = null;

    public Action<PointerEventData> OnEndDragHandler = null;

    public Action<PointerEventData> OnDropHandler = null;

    // Enter 키 입력 이벤트 발생 시 호출되는 액션
    public Action OnEnterPressHandler = null;

    // Tab 키 입력 이벤트 발생시 호출
    public Action OnTabPressHandler = null;

    // Esc 키 입력 이벤트 발생시 호출
    public Action OnEscapePressHandler = null;

    // C 키 입력 이벤트 발생시 호출
    public Action OnCPressHandler = null;
    //// Q 키 입력 이벤트 발생시 호출
    //public Action OnQPressHandler = null;

    //// E 키 입력 이벤트 발생시 호출
    //public Action OnEPressHandler = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler == null) return;
        OnBeginDragHandler.Invoke(eventData);
        throw new NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler == null) return;
        OnDragHandler.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragHandler == null) return;
        OnEndDragHandler.Invoke(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropHandler == null) return;
        OnDropHandler.Invoke(eventData);
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler == null) return;
        OnClickHandler.Invoke(eventData);
    }

    private void Update()
    {
        // PopUp이 띄워저 있는지 확인
        if (Managers.UI.GetTopPopupUI() != null)
        {
            // PopUp이 띄워져있다면 가장 위에 띄워져있는 Popup인지 확인
            if (Managers.UI.GetTopPopupUI() != transform.GetComponentInParent<UI_Popup>()) return;
        }
        // Popup이 띄워저 있지 않으면 FixedUI만 있는거니까 그냥 실행하면 될거 같다.

        // Enter 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnEnterPressHandler?.Invoke();
        }

        // Tab 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressHandler?.Invoke();
        }

        // Esc 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapePressHandler?.Invoke();
        }

        // C 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCPressHandler?.Invoke();
        }

        //// Q 키가 눌렸는지 확인
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    OnQPressHandler?.Invoke();
        //}

        //// E 키가 눌렸는지 확인
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    OnEPressHandler?.Invoke();
        //}
    }


}
