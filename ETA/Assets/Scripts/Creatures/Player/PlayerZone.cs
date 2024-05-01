using Photon.Pun;
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

    [SerializeField] public PlayerController playerController;
    float _delta;


    void Start()
    {
        currentSpeed = Speed;  // 시작할 때 현재 속도를 초기 속도로 설정
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var playerObj in playerObjects)
            {
                if (playerObj.GetComponent<PhotonView>().IsMine)
                {
                    playerController = playerObj.GetComponent<PlayerController>();
                    break;
                }

            }
        }
        moveFront();
    }

    void moveFront()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(2,1,6), new Quaternion(), TargetLayerMask);
        if (enemies.Length <= 0)
        {
            _delta += Time.deltaTime / 2;
            _delta = _delta >= 1.0f ? 1.0f : _delta;
            transform.position += transform.forward * Time.deltaTime * currentSpeed * _delta;
        }
        else
        {
            _delta = 0;
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
