using Photon.Pun;
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

    public Vector3 SkillRange = new Vector3(0,0,0);
    public Define.RangeType RangeType;


    public Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        skillCursor = Resources.Load<Texture2D>("Sprites/Cursor Sprites/Enemy Cursor"); // 커서는 나중에 바꾸기

        targetingGo = Managers.Resource.Instantiate("Targeting").GetComponent<ParticleSystem>();
        targetingGo.Play();
        targetingGo.gameObject.SetActive(false);

        rangeObject = Managers.Resource.Instantiate("RangeObject");
        rangeObject.SetActive(false);

        myController = GetComponent<PlayerController>();
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
            case Define.SkillType.Immediately:
                ImmediatelyCast();
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
        if (currentCursor != CursorType.Target)
        {
            Cursor.SetCursor(skillCursor, Vector2.zero, CursorMode.Auto);
            currentCursor = CursorType.Target;
        }

        if (raycastHit)
        {

            targetingGo.gameObject.SetActive(true);

            GameObject monster = hit.collider.gameObject;
            TargetOn(monster);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(monster.name);
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                targetingGo.gameObject.SetActive(false);
                TargetPosition = hit.collider.transform.position;
                SyncTargetPosition(TargetPosition);
                myController.ChangeState(myController.SKILL_STATE);
            }
        }
        else
        {
            targetingGo.gameObject.SetActive(false);
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
            rangeObject.gameObject.transform.localScale = new Vector3(SkillRange.x, 0.0001f, SkillRange.z);
            if (currentCursor != CursorType.Range)
            {
                Cursor.SetCursor(skillCursor, Vector2.zero, CursorMode.Auto);
                currentCursor = CursorType.Range;
                rangeObject.gameObject.SetActive(true);
                rangeObject.transform.GetChild(0).gameObject.SetActive(false);
                rangeObject.transform.GetChild(1).gameObject.SetActive(false);
                rangeObject.transform.GetChild((int)RangeType).gameObject.SetActive(true);
                targetingGo.gameObject.SetActive(false);
            }


            rangeObject.transform.position = hit.point + Vector3.up * 0.05f;

            if (Input.GetMouseButtonDown(0))
            {
                currentType = Define.SkillType.None;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                rangeObject.SetActive(false);
                TargetPosition = hit.point;
                SyncTargetPosition(TargetPosition);
                myController.ChangeState(myController.SKILL_STATE);
                
                Debug.Log("범위 스킬 사용");
            }

        }
    }

    public void StartHolding()
    {
        currentType = Define.SkillType.None;
        myController.ChangeState(myController.HOLD_STATE);
        TargetPosition = transform.position + gameObject.transform.forward * 3;
        SyncTargetPosition(TargetPosition);
    }

    public void ImmediatelyCast()
    {
        currentType = Define.SkillType.None;
        Debug.Log(myController);
        myController.ChangeState(myController.SKILL_STATE);
        TargetPosition = transform.position + gameObject.transform.forward * 3;
        SyncTargetPosition(TargetPosition);
    }

    public void Clear()
    {
        targetingGo.gameObject.SetActive(false);
        rangeObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        currentType = Define.SkillType.None;
        currentCursor = CursorType.None;
    }


    
    void SyncTargetPosition(Vector3 position)
    {
        GetComponent<PhotonView>().RPC("RPC_SetTargetPosition", RpcTarget.Others, position);
    }

    [PunRPC]
    void RPC_SetTargetPosition(Vector3 position)
    {
        TargetPosition = position;
    }
}
