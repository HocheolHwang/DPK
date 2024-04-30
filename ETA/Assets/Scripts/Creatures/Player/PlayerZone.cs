using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerZone : MonoBehaviour
{
    // 근거리, 원거리, 일반과 보스 몬스터는 공격 사거리가 다르다.
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask TargetLayerMask;
    [SerializeField] public float Speed = 6;
    private float currentSpeed;  // 현재 이동 속도

    [SerializeField] private PlayerController playerController;
    float _delta;
    NavMeshAgent _agent;


    void Start()
    {
        currentSpeed = Speed;  // 시작할 때 현재 속도를 초기 속도로 설정
    }

    // Update is called once per frame
    void Update()
    {
        _agent.GetComponent<NavMeshAgent>();
        moveFront();
    }

    void moveFront()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(2,1,6), new Quaternion(), TargetLayerMask);
        if (enemies.Length <= 0)
        {

            //_delta += Time.deltaTime;
            //_delta = _delta >= currentSpeed ? currentSpeed : _delta;
            transform.position += transform.forward * Time.deltaTime * currentSpeed;
        }

    }

    public void StopMovement()
    {
        currentSpeed = 0;
        if (playerController != null)
        {
            playerController.ChangeState(playerController.IDLE_STATE);  // 상태를 IDLE로 변경
        }
    }

    public void StartMovement()
    {
        currentSpeed = Speed;
        if (playerController != null)
        {
            playerController.ChangeState(playerController.MOVE_STATE);  // 상태를 MOVE로 변경
        }
    }
}
