using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{

    enum CursorType
    {
        None,
        Target,
        Range,
        Holding
    }

    [SerializeField]
    public Define.SkillType currentType;
    CursorType currentCursor;
    PlayerController myController;

    private Texture2D skillCursor;
    ParticleSystem targetingGo;
    GameObject rangeObject;

    // Start is called before the first frame update
    void Start()
    {
        skillCursor = Resources.Load<Texture2D>("Sprites/7. Common Sprites/Cursor Sprites/Enemy Cursor"); // 커서는 나중에 바꾸기

        targetingGo = Managers.Resource.Instantiate("Targeting").GetComponent<ParticleSystem>();
        targetingGo.Play();
        targetingGo.gameObject.SetActive(false);

        rangeObject = Managers.Resource.Instantiate("RangeObject");
        rangeObject.SetActive(false);

        myController = GameObject.Find("Warrior").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentType)
        {
            case Define.SkillType.Target:
                FindTarget();
                break;
            case Define.SkillType.Range:
                SelectRange();
                break;
            case Define.SkillType.Holding:
                StartHolding();
                break;
            default:
                targetingGo.gameObject.SetActive(false);
                rangeObject.SetActive(false);
                if (currentCursor != CursorType.None)
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    currentCursor = CursorType.None;
                }
                break;
        }
    }

    


    void FindTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Monster"));

        rangeObject.SetActive(false);

        if (raycastHit)
        {
            if (currentCursor != CursorType.Target)
            {
                Cursor.SetCursor(skillCursor, Vector2.zero, CursorMode.Auto);
                currentCursor = CursorType.Target;
                targetingGo.gameObject.SetActive(true);
            }

            GameObject monster = hit.collider.gameObject;
            TargetOn(monster);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(monster.name);
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                targetingGo.gameObject.SetActive(false);
                myController.ChangeState(myController.SKILL_STATE);
            }
        }
        else
        {
            if (currentCursor != CursorType.None)
            {
                targetingGo.gameObject.SetActive(false);
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                currentCursor = CursorType.None;
            }
        }


    }

    void TargetOn(GameObject target)
    {
        targetingGo.transform.parent = target.transform;
        targetingGo.transform.localPosition = Vector3.zero;
        targetingGo.transform.localScale = Vector3.one;
        targetingGo.transform.parent = null;
    }

    void SelectRange()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground"));

        if (raycastHit)
        {
            if (currentCursor != CursorType.Range)
            {
                Cursor.SetCursor(skillCursor, Vector2.zero, CursorMode.Auto);
                currentCursor = CursorType.Range;
                rangeObject.gameObject.SetActive(true);
                rangeObject.gameObject.transform.localScale = new Vector3(4, 0.01f, 4);
                targetingGo.gameObject.SetActive(false);
            }

            Debug.Log($"범위 스킬 지정 중 {hit.point}");
            
            rangeObject.transform.position = hit.point + Vector3.up * 0.05f;

            if (Input.GetMouseButtonDown(0))
            {
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                rangeObject.gameObject.SetActive(false);
                myController.ChangeState(myController.SKILL_STATE);
                Debug.Log("범위 스킬 사용");
            }

        }
    }

    public void StartHolding()
    {
        currentType = Define.SkillType.None;
        Debug.Log(myController);
        myController.ChangeState(myController.HOLD_STATE);
    }

    public void Clear()
    {
        targetingGo.gameObject.SetActive(false);
        rangeObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        currentType = Define.SkillType.None;
        currentCursor = CursorType.None;
    }
}
