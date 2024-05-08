using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Select_Skill : UI_Popup
{

    private Vector3 startPosition;
    private Transform originalParent;


    // 8 개 설정해서 QWERASDF 설정
    private GameObject[] Slot = new GameObject[8];
    public override void Init()
    {
        base.Init();

        AddUIEvent(null, BeginDragEvent, Define.UIEvent.BeginDrag);
        AddUIEvent(null, BeginDragEvent, Define.UIEvent.Drag);
        AddUIEvent(null, BeginDragEvent, Define.UIEvent.EndDrag);
        AddUIEvent(null, BeginDragEvent, Define.UIEvent.Drop);

    }






    public void BeginDragEvent(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void DragEvent(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    public void EndDragEvent(PointerEventData eventData)
    {
        transform.position = startPosition;
    }
    public void DropEvent(PointerEventData eventData)
    {
        if (true)
        {
            GameObject item = eventData.pointerDrag.gameObject;
            GameObject slot = eventData.pointerCurrentRaycast.gameObject;
            slot.name = item.name;
        }
    }

    public void SetKeySlot(Define.SkillKey key)
    {
        // 스킬 슬롯에 이미 있는 스킬인가?


    }
}
