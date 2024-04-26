using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{

    enum CursorType
    {
        None,
        Target,
    }

    [SerializeField]
    public Define.SkillType currentType;
    CursorType currentCursor;


    //public SkillType CurrentSkillType { get { return currentType; } set { currentType = value; } };

    float originOutlineWidth = -1;
    GameObject origin;

    private Texture2D handCursor;
    ParticleSystem targetingGo;
    GameObject rangeObject;

    // Start is called before the first frame update
    void Start()
    {
        handCursor = Resources.Load<Texture2D>("Sprites/7. Common Sprites/Cursor Sprites/Enemy Cursor"); // 커서는 나중에 바꾸기
        targetingGo = Managers.Resource.Instantiate("Targeting").GetComponent<ParticleSystem>();
        targetingGo.Play();
        rangeObject = Managers.Resource.Instantiate("RangeObject");
        rangeObject.SetActive(false);
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
        
        

        if (raycastHit)
        {
            
            if (currentCursor != CursorType.Target)
            {
                Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
                currentCursor = CursorType.Target;
                targetingGo.gameObject.SetActive(true);
            }


            GameObject monster = hit.collider.gameObject;
            
            //monster.GetComponentInChildren<Renderer>().material.SetFloat("_Outline_Width", 40);
            targetingGo.transform.parent = monster.transform;
            targetingGo.transform.localPosition = Vector3.zero;
            targetingGo.transform.localScale = Vector3.one;
            targetingGo.transform.parent = null;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(monster.name);
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                targetingGo.gameObject.SetActive(false);
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

    void SelectRange()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground"));

        if (raycastHit)
        {
            rangeObject.gameObject.SetActive(true);
            rangeObject.gameObject.transform.localScale = new Vector3(4, 0.05f, 4);
            Debug.Log($"범위 스킬 지정 중 {hit.point}");
            
            rangeObject.transform.position = hit.point + Vector3.up * 0.05f;

            if (Input.GetMouseButtonDown(0))
            {
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                rangeObject.gameObject.SetActive(false);
                Debug.Log("범위 스킬 사용");
            }

        }
        else
        {

        }

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
