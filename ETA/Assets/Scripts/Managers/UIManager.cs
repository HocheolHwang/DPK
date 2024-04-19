using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 관리자 클래스
/// </summary>
public class UIManager
{
    // "Popup UI"들의 정렬 순서를 관리하는 변수
    int _order = 10;

    // "Popup UI"들을 관리하기 위한 스택
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    // 현재 활성화된 "Scene UI"
    UI_Scene _sceneUI = null;

    // UI 요소들을 담을 최상위 루트 GameObject를 제공하는 프로퍼티
    public GameObject Root
    {
        get
        {
            // @UI_Root라는 이름의 GameObject를 찾아 반환하고, 없을 경우 새로 생성하여 반환
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    // 주어진 GameObject에 Canvas 컴포넌트를 추가하고 설정하는 메서드
    public void SetCanvas(GameObject go, bool sort = true)
    {
        // Canvas 컴포넌트를 추가하고, 렌더링 모드를 ScreenSpaceOverlay로 설정
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            // "Popup UI"의 정렬 순서를 조정
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            // "Scene UI"의 정렬 순서를 0으로 고정
            canvas.sortingOrder = 0;
        }
    }

    // UI 하위 요소를 동적으로 생성하는 메서드
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        // 이름이 지정되지 않은 경우, T의 이름을 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 지정된 이름의 프리팹을 로드하여 인스턴스화하고, 부모에 할당
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null) go.transform.SetParent(parent);

        // 요구되는 컴포넌트(T)를 추가하거나 가져와서 반환
        return go.GetOrAddComponent<T>();
    }

    // "Scene UI"를 표시하고 관리하는 메서드
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 이름이 지정되지 않은 경우, T의 이름을 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 지정된 이름의 "Scene UI" 프리팹을 인스턴스화하고 루트 GameObject에 추가
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);

        // 새로운 "Scene UI"를 표시할 때마다 현재 활성화된 "Scene UI"를 새로운 "Scene UI"로 교체
        _sceneUI = sceneUI;

        // go GameObject를 Root GameObject의 자식으로 설정
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    // "Popup UI"를 표시하고 스택에 추가하는 메서드
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 이름이 지정되지 않은 경우, T의 이름을 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 지정된 이름의 "Popup UI" 프리팹을 인스턴스화하고 루트 GameObject에 추가
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);

        // 새로운 "Popup UI"가 추가될 때 팝업 스택에 푸시
        _popupStack.Push(popup);

        // go GameObject를 Root GameObject의 자식으로 설정
        go.transform.SetParent(Root.transform);

        return popup;
    }

    // 지정된 "Popup UI"를 닫는 메서드
    public void ClosePopupUI(UI_Popup popup)
    {
        // 팝업 스택이 비어있으면 종료
        if (_popupStack.Count == 0)
            return;

        // 최상위 팝업이 입력된 팝업과 일치하지 않으면 종료
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    // 현재 활성화된 최상위 "Popup UI"를 닫는 메서드
    public void ClosePopupUI()
    {
        // 팝업 스택이 비어있으면 종료
        if (_popupStack.Count == 0)
            return;

        // 최상위 "Popup UI"를 팝하고, 해당 GameObject를 파괴하고 정렬 순서를 감소
        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    // 모든 "Popup UI"를 닫는 메서드
    public void CloseAllPopupUI()
    {
        // 팝업 스택에 있는 모든 "Popup UI"를 닫고, 스택을 비움
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    // 모든 UI 요소를 초기화하는 메서드
    public void Clear()
    {
        // 모든 "Popup UI"를 닫고, 현재 "Scene UI"를 null로 설정
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
