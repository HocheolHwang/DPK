using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    // 손가락 커서 이미지를 저장하는 변수
    private static Texture2D handCursor;

    private void Start()
    {
        // 손가락 커서 이미지를 Resources에서 로드하여 handCursor 변수에 저장
        handCursor = Resources.Load<Texture2D>("Sprites/Cursor Sprites/Hand Cursor");
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            // Highlight 해제
            DisableOutline(highlight);
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            Transform hitTransform = raycastHit.transform;
            if (hitTransform.CompareTag("Selectable") && hitTransform != selection)
            {
                Transform groupParent = FindGroupParent(hitTransform);
                if (groupParent != null)
                {
                    // Highlight 설정
                    EnableOutline(groupParent);
                    highlight = groupParent;

                    // 커서를 손가락 모양으로 변경
                    Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                highlight = null;

                // 오브젝트 위에 없을 때 기본 커서로 복원
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    DisableOutline(selection);
                }
                selection = highlight;
                EnableOutline(selection);

                // 선택된 오브젝트에 따라 다른 Popup UI 띄움
                switch (highlight.name)
                {
                    case "Character_Selection":
                        Managers.UI.ShowPopupUI<Character_Select_Popup_UI>("[Lobby]_Character_Select_Popup_UI");
                        break;
                    case "Spell_Enchant":
                        // Managers.UI.ShowPopupUI<Spell_Enchant_Popup_UI>("[Lobby]_Spell_Enchant_Popup_UI");
                        break;
                    case "Leader_Board":
                        // Managers.UI.ShowPopupUI<Leader_Board_Popup_UI>("[Lobby]_Leader_Board_Popup_UI");
                        break;
                    default:
                        break;
                }

                highlight = null;

                // 오브젝트 위에 없을 때 기본 커서로 복원
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                if (selection)
                {
                    DisableOutline(selection);
                    selection = null;
                }
            }
        }
    }

    // 그룹의 부모를 찾는 함수
    private Transform FindGroupParent(Transform child)
    {
        Transform parent = child.parent;
        while (parent != null)
        {
            if (parent.name == "Spell_Enchant" || parent.name == "Character_Selection" || parent.name == "Leader_Board")
            {
                return parent;
            }
            parent = parent.parent;
        }
        return null;
    }

    // 그룹에 속한 모든 오브젝트에 Outline을 활성화하는 함수
    private void EnableOutline(Transform group)
    {
        foreach (Transform child in group)
        {
            Outline outline = child.gameObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = child.gameObject.AddComponent<Outline>();
                outline.OutlineColor = new Color32(255, 210, 87, 255);
                outline.OutlineWidth = 7.0f;
            }
            outline.enabled = true;
        }
    }

    // 그룹에 속한 모든 오브젝트의 Outline을 비활성화하는 함수
    private void DisableOutline(Transform group)
    {
        foreach (Transform child in group)
        {
            Outline outline = child.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }
}
