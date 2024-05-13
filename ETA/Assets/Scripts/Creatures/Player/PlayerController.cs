using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using PlayerStates;
using Photon.Pun;

public class PlayerController : BaseController
{

    public State IDLE_STATE;
    public State MOVE_STATE;
    public State ATTACK_STATE;
    public State SKILL_STATE;
    public State COLLAVO_STATE;
    public State DIE_STATE;
    public State HOLD_STATE;
    public State GROGGY_STATE;
    public State GLOBAL_STATE;


    [SerializeField]
    public Transform _destination;
    public Define.SkillKey _usingSkill;
    public bool isFinished;

    public SkillSlot SkillSlot { get; set;}

    public static event Action OnPlayerDestroyed;

    private void Start()
    {
        Init();
    }

    protected override void Init()
    {
        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        MOVE_STATE = new MoveState(this);
        ATTACK_STATE = new AttackState(this);
        SKILL_STATE = new SkillState(this);
        COLLAVO_STATE = new CollavoState(this);
        DIE_STATE = new DieState(this);
        HOLD_STATE = new HoldState(this);
        GROGGY_STATE = new GroggyState(this);
        GLOBAL_STATE = new GlobalState(this);

        SkillSlot = gameObject.GetOrAddComponent<SkillSlot>();
        //photonView = gameObject.GetComponent<PhotonView>();
        
        _stateMachine.SetGlobalState(GLOBAL_STATE);

        // TODO : 번호에 따른 목적지 변경 해줘야함

        UI_CharacterNickName ui = Managers.UI.MakeWorldSpaceUI<UI_CharacterNickName>(transform);
        ui.NickName = photonView.Owner.NickName;


        _stateMachine.CurState = IDLE_STATE;

        //ChangeState(IDLE_STATE);


        if (photonView.IsMine)
        {
            // 내 캐릭터 포지션 잡기
            SetPosition();

            Managers.Input.KeyAction -= KeyEvent;
            Managers.Input.KeyAction += KeyEvent;

            Managers.Input.MouseAction -= MouseEvent;
            Managers.Input.MouseAction += MouseEvent;

            // 내캐릭터가 무엇인지 저장한다.
            var newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            newProperties["viewID"] = photonView.ViewID;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
            
        }
        //Debug.Log($"{photonView.IsMine}");


        // 캐릭터가 생성 될 때 마다 카운트를 해준다.
        // 내 화면에 모든 캐릭턱 생성 되기를 기다려야 하기 때문
        FindObjectOfType<GameSystem>().SendCharacherInstantiatedMsg();


        //if(gameObject.name.StartsWith("Archer"))FindObjectOfType<CollavoSystem>().AddCurrentSkill(this, "Cyclone");


        // if(PhotonNetwork.IsMasterClient) StartCoroutine(TESTGROGGY());
    }

    IEnumerator TESTGROGGY()
    {
        yield return new WaitForSeconds(3.0f);
        ChangeState(GROGGY_STATE);
        
    }

    private void OnDestroy()
    {
        FindObjectOfType<Dungeon_Scene>()?.AnyOneDied();
    }

    private void SetPosition()
    {
        int frontCnt = 0;
        int backCnt = 0;

        int myIndex = -1;
        bool isFront = true;
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("CurClass", out object val))
            {
                string classCode = (string)val;
                switch (classCode)
                {
                    case "C001":
                        if (player.IsLocal)
                        {
                            myIndex = frontCnt;
                            isFront = true;
                        }
                        frontCnt += 1;
                        break;
                    case "C002":
                    case "C003":
                        if (player.IsLocal)
                        {
                            myIndex = backCnt;
                            isFront = false;
                        }
                        backCnt += 1;
                        break;
                }


            }
        }

        int totalNum = isFront ? frontCnt : backCnt;
        string pos = isFront ? "FRONT_" : "BACK_";

        if(totalNum == 1 || totalNum == 0)
        {
            pos += "2";
        }
        else if(totalNum == 2)
        {
            switch (myIndex)
            {
                case 0:
                    pos += "1";
                    break;
                case 1:
                    pos += "3";
                    break;
            }
        }
        else if(totalNum == 3)
        {
            switch (myIndex)
            {
                case 0:
                    pos += "1";
                    break;
                case 1:
                    pos += "2";
                    break;
                case 2:
                    pos += "3";
                    break;
            }
        }
        Debug.Log(myIndex);
        Debug.Log(totalNum);
        Debug.Log(pos);

        _destination = GameObject.Find(pos).transform;
        transform.position = _destination.position;
        Debug.Log(Managers.Scene.CurrentScene.SceneType);
        if(Managers.Scene.CurrentScene.SceneType == Define.Scene.Tutorial)
        {
            Camera.main.GetComponent<CameraController>()._player = gameObject;
        }
        else
        {
            Camera.main.GetComponent<CameraController>()._player = _destination.parent.gameObject;
        }
        

    }


    void KeyEvent()
    {
        if (CurState is SkillState || CurState is DieState || CurState is HoldState || CurState is CollavoState || CurState is GroggyState) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _usingSkill = Define.SkillKey.Q;
            SyncUsingSkill(Define.SkillKey.Q);
            SkillSlot.SelectSkill(Define.SkillKey.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _usingSkill = Define.SkillKey.W;
            SyncUsingSkill(Define.SkillKey.W);
            SkillSlot.SelectSkill(Define.SkillKey.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _usingSkill = Define.SkillKey.E;
            SyncUsingSkill(Define.SkillKey.E);
            SkillSlot.SelectSkill(Define.SkillKey.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _usingSkill = Define.SkillKey.R;
            SyncUsingSkill(Define.SkillKey.R);
            SkillSlot.SelectSkill(Define.SkillKey.R);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _usingSkill = Define.SkillKey.A;
            SyncUsingSkill(Define.SkillKey.A);
            SkillSlot.SelectSkill(Define.SkillKey.A);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _usingSkill = Define.SkillKey.S;
            SyncUsingSkill(Define.SkillKey.S);
            SkillSlot.SelectSkill(Define.SkillKey.S);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _usingSkill = Define.SkillKey.D;
            SyncUsingSkill(Define.SkillKey.D);
            SkillSlot.SelectSkill(Define.SkillKey.D);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _usingSkill = Define.SkillKey.F;
            SyncUsingSkill(Define.SkillKey.F);
            SkillSlot.SelectSkill(Define.SkillKey.F);
        }

    }

    void MouseEvent(Define.MouseEvent evt)
    {
        if (Input.GetMouseButtonDown(1))
        {
            SkillSlot.CancleSkill();
        }
    }

    public override void DestroyEvent()
    {
        // 플레이어 죽었을때 이벤트 발생
        OnPlayerDestroyed?.Invoke();

        base.DestroyEvent();
        SkillSlot.Clear();
    }

    public override void AttackedEvent()
    {
        base.AttackedEvent();
        Managers.Sound.Play("Player/Attacked");
    }

    


    /// <summary>
    /// 다른 컴포넌트로 옮겨야 할듯
    /// </summary>


    public void SyncUsingSkill(Define.SkillKey key)
    {
        photonView.RPC("RPC_SetUsingSkill", RpcTarget.Others, key);
    }

    public void ChangeToIdleState()
    {
        photonView.RPC("RPC_ChangeToIdleState", RpcTarget.Others);
    }
    public void ChangeToMoveState()
    {
        photonView.RPC("RPC_ChangeToMoveState", RpcTarget.Others);
    }

    public void ChangeToAttackState()
    {
        photonView.RPC("RPC_ChangeToAttackState", RpcTarget.Others);
    }

    public void ChangeToSkillState()
    {
        photonView.RPC("RPC_ChangeToSkillState", RpcTarget.Others);
    }

    public void ChangeToHoldState()
    {
        photonView.RPC("RPC_ChangeToHoldState", RpcTarget.Others);
    }

    public void ChangeToCollavoState()
    {
        photonView.RPC("RPC_ChangeToCollavoState", RpcTarget.Others);
    }

    public void ChangeToDieState()
    {
        photonView.RPC("RPC_ChangeToDieState", RpcTarget.Others);
    }

    public void ChangeToGroggyState()
    {
        photonView.RPC("RPC_ChangeToGroggyState", RpcTarget.Others);
    }



    [PunRPC]
    void RPC_SetUsingSkill(Define.SkillKey key)
    {
        _usingSkill = key;
    }
    [PunRPC]
    void RPC_ChangeToIdleState()
    {
        ChangeState(IDLE_STATE);
    }

    [PunRPC]
    void RPC_ChangeToAttackState()
    {
        ChangeState(ATTACK_STATE);
    }

    [PunRPC]
    void RPC_ChangeToMoveState()
    {
        ChangeState(MOVE_STATE);
    }

    [PunRPC]
    void RPC_ChangeToSkillState()
    {
        ChangeState(SKILL_STATE);
    }

    [PunRPC]
    void RPC_ChangeToHoldState()
    {
        ChangeState(HOLD_STATE);
    }

    [PunRPC]
    void RPC_ChangeToCollavoState()
    {
        ChangeState(COLLAVO_STATE);
    }
    //RPC_ChangeToDieState
    [PunRPC]
    void RPC_ChangeToDieState()
    {
        ChangeState(DIE_STATE);
    }

    [PunRPC]
    void RPC_ChangeToGroggyState()
    {
        ChangeState(GROGGY_STATE);
    }

    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }
}
