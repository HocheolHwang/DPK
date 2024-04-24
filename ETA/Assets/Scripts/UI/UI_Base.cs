using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 모든 UI 컴포넌트의 기본 클래스
/// UI 요소 바인딩과 이벤트 처리를 담당한다.
/// </summary>
public abstract class UI_Base : MonoBehaviour
{
    // UI 요소를 저장하는 딕셔너리. 타입별로 구분됨
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 초기화 메서드. 상속받는 클래스에서 구현해야 함
    public abstract void Init();

    private void Start()
    {
        Init();
    }

    // UI 요소를 찾아서 _objects 딕셔너리에 바인딩하는 메서드
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // Enum에서 정의된 이름들을 가져옴
        string[] names = Enum.GetNames(type);

        // UI 객체를 저장할 배열 생성
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        // 딕셔너리에 타입과 객체 배열을 추가
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // 여기 GameObject만 빼준 이유?
            // GameObject는 Component가 아니다 그래서 GameObject에 바로 접근할 순 없다.
            // 그래서 새로운 함수를 정의 했다.
            if (typeof(T) == typeof(GameObject)) objects[i] = Util.FindChild(gameObject, names[i], true);
            else objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            // 여기서 gameObject는 UI_Button(Canvas)
            // names[i]를 찾을것임
            // 요소를 찾지 못한 경우 로그 출력
            if (objects[i] == null) Debug.Log($"Not found {names[i]}");
        }
    }

    // _objects 딕셔너리에서 특정 타입의 UI 요소를 인덱스로 가져오는 메서드
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) return null;

        return objects[idx] as T;
    }

    // 특정 타입의 UI 요소를 쉽게 가져오기 위한 편의 메서드들
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // UI 이벤트를 추가하는 정적 메서드
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // UI_EventHandler 컴포넌트를 가져오거나, 없으면 추가한다.
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        // 이벤트 타입에 따라 적절한 이벤트 핸들러에 액션을 추가하거나 제거한다.
        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

    // Enter 키 이벤트를 추가하는 메서드
    public static void AddUIKeyEvent(GameObject go, Action action, KeyCode key)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        // 필요한 핸들러를 UI_EventHandler에 추가해서 넣을 것
        switch (key)
        {
            case KeyCode.KeypadEnter:
            case KeyCode.Return:
                evt.OnEnterPressHandler -= action;
                evt.OnEnterPressHandler += action;
                break;
            case KeyCode.Tab:
                evt.OnTabPressHandler -= action;
                evt.OnTabPressHandler += action;
                break;
        }
    }
}
