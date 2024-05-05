using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// InputManager 클래스는 키보드 및 마우스 입력을 관리하는 기능을 제공합니다.
/// </summary>
public class InputManager
{
    // 키보드 및 마우스 입력 이벤트 액션
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    // 마우스 버튼 눌림 상태를 나타내는 변수
    bool _pressed = false;

    public void OnUpdate()
    {
        // UI를 무시해야 할 경우 입력 처리를 하지 않음
        //if (!IsPointerOverIgknoredUI()) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 키 입력 이벤트 처리
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();
        //if (KeyAction != null)
        //    KeyAction.Invoke();

        // 마우스 입력 이벤트 처리
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.Click);
                }
                _pressed = false;
            }
        }
    }

    private bool IsPointerOverIgnoredUI()
    {
        // 현재 포인터의 위치를 기반으로 새 PointerEventData 객체를 생성합니다.
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // 레이캐스트 결과를 담을 리스트를 생성합니다.
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = UnityEngine.Object.FindObjectOfType<GraphicRaycaster>();
        if (raycaster == null)
        {
            Debug.LogError("GraphicRaycaster not found in the current scene.");
            return false;
        }
        // 레이캐스트를 수행합니다.
        raycaster.Raycast(pointerData, results);
        //EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count == 0) return true;
        //Debug.Log(results.Count);
        foreach (var result in results)
        {
            //Debug.Log($"{result.gameObject.layer} == {LayerMask.GetMask("Ignored UI")}");
            if (1 << result.gameObject.layer == LayerMask.GetMask("Ignored UI")) // 특정 태그를 가진 UI를 무시합니다.
            {
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
