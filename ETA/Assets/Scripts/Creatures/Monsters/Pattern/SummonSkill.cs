using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// buffer랑 warrior가 죽은 것도 여기서 관리
[Serializable]
public class SummonSkill : MonoBehaviour
{
    // poolable, Resource Manager가 관리하면 parent가 mummy man이 되기 때문에 둘 다 사용하지 않음
    [Header("Have not Poolable")]
    [SerializeField] private GameObject _buffer;
    [SerializeField] private GameObject _warrior;
    private MummyBufferController _refBuffer;
    private MummyWarriorController _refWarrior;

    private NavMeshAgent _agent;

    [Header("사망, 소환 횟수 디버깅")]
    [SerializeField]  private int _bufferSummonCount = 0;
    [SerializeField]  private int _warriorSummonCount = 0;
    [SerializeField]  private int _bufferDeathCount = 0;                         // buffer와 warrior가 한 번씩 죽었을 경우, 글로벌에서 이를 감지하여 CLAP으로 전환, 단 공격 및 패턴 상태일 때는 안 됨
    [SerializeField]  private int _warriorDeathCount = 0;
    public int BufferSummonCount { get => _bufferSummonCount; private set => _bufferSummonCount = value; }
    public int WarriorSummonCount { get => _warriorSummonCount; private set => _warriorSummonCount = value; }
    public int BufferDeathCount { get => _bufferDeathCount; private set => _bufferDeathCount = value; }
    public int WarriorDeathCount { get => _warriorDeathCount; private set => _warriorDeathCount = value; }

    private float _bufferDist = 6.0f;
    private float _warriorDist = 3.0f;
    public float BufferDist { get => _bufferDist; set => _bufferDist = value; }
    public float WarriorDist { get => _warriorDist; set => _warriorDist = value; }


    private void Start()
    {
        _agent = gameObject.GetComponent<BaseController>().Agent;
    }

    // ------------------------------------- Subscribe Action ------------------------------------------------
    private void OnEnable()
    {
        GetComponent<MummyManController>().OnBossDestroyed += HandleDeathMan;
    }

    private void OnDisable()
    {
        GetComponent<MummyManController>().OnBossDestroyed -= HandleDeathMan;
    }

    // ------------------------------------- Summon Functions ------------------------------------------------
    public void Summon()
    {
        _bufferSummonCount++;
        _warriorSummonCount++;

        // 위치 세팅
        Vector3 rootBack = gameObject.transform.TransformPoint(Vector3.back * _bufferDist);
        Vector3 rootLeft = gameObject.transform.TransformPoint(Vector3.left * _warriorDist);
        _buffer.transform.position = rootBack;
        _warrior.transform.position = rootLeft;

        // 이펙트( Stop Action - Destroy )
        ParticleSystem warriorPS = Managers.Effect.Play(Define.Effect.Mummy_Clap, 0, transform);
        warriorPS.transform.position = _warrior.transform.position;
        ParticleSystem bufferPS = Managers.Effect.Play(Define.Effect.Mummy_Clap, 0, transform);
        bufferPS.transform.position = _buffer.transform.position;

        // 방향 세팅
        _buffer.transform.rotation = gameObject.transform.rotation;
        _warrior.transform.rotation = gameObject.transform.rotation;

        //_refBuffer = Instantiate(_buffer).GetComponent<MummyBufferController>();
        //_refWarrior = Instantiate(_warrior).GetComponent<MummyWarriorController>();
        if (PhotonNetwork.IsMasterClient)
        {
            _refBuffer = PhotonNetwork.Instantiate($"Prefabs/Creatures/Monsters/{_buffer.name}", _buffer.transform.position, _buffer.transform.rotation).GetComponent<MummyBufferController>();
            _refWarrior = PhotonNetwork.Instantiate($"Prefabs/Creatures/Monsters/{_warrior.name}", _warrior.transform.position, _warrior.transform.rotation).GetComponent<MummyWarriorController>();
            _refBuffer.OnDeath -= HandleDeathBuffer;
            _refWarrior.OnDeath -= HandleDeathWarrior;
            _refBuffer.OnDeath += HandleDeathBuffer;
            _refWarrior.OnDeath += HandleDeathWarrior;
        }

        // Action에 함수 세팅

    }

    public void DespawnAll()        // MummyMan이 죽었을 떄, Buffer와 Warrior의 체력을 0으로 세팅
    {
        if (_refBuffer != null)
        {
            _refBuffer.TakeDamage(_refBuffer.Stat.MaxHp + _refBuffer.Stat.Defense);
            //_refBuffer.Stat.Hp = 0;
        }
        if (_refWarrior != null)
        {
            _refWarrior.TakeDamage(_refBuffer.Stat.MaxHp + _refWarrior.Stat.Defense);
            //_refWarrior.Stat.Hp = 0;
        }
    }

    // ------------------------------------- Action Functions ------------------------------------------------
    private void HandleDeathBuffer()            // 죽을 때마다 count를 증가
    {
        //  Managers.Resource.Destroy(_buffer); // 죽는건 controller에서 관리함
        _bufferDeathCount++;
        GetComponent<PhotonView>().RPC("RPC_HandleDeathBuffer", RpcTarget.Others);
    }
    private void HandleDeathWarrior()
    {
        _warriorDeathCount++;
        GetComponent<PhotonView>().RPC("RPC_HandleDeathWarrior", RpcTarget.Others);
    }
    private void HandleDeathMan()
    {

        //TODO 한쪽만 해도 되나?
        DespawnAll();
    }

    [PunRPC]
    void RPC_HandleDeathBuffer()
    {
        _bufferDeathCount++;
    }
    [PunRPC]
    void RPC_HandleDeathWarrior()
    {
        _warriorDeathCount++;
    }
}
