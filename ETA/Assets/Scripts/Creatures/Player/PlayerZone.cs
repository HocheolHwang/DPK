using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    [SerializeField] public float DetectRange;          // 근거리, 원거리, 일반과 보스 몬스터는 공격 사거리가 다르다.
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask TargetLayerMask;
    [SerializeField] public float Speed = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveFront();
    }

    void moveFront()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(2,1,6), new Quaternion(), TargetLayerMask);
        if (enemies.Length <= 0)
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
        }

    }
}
