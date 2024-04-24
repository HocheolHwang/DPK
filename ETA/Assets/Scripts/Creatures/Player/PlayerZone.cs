using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    [SerializeField] public float detectRange;          // 근거리, 원거리, 일반과 보스 몬스터는 공격 사거리가 다르다.
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask targetLayerMask;

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
        

        //Collider[] enemies = Physics.OverlapBox(transform.position, detectRange, targetLayerMask);
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(2,1,6), new Quaternion(), targetLayerMask);

        if (enemies.Length > 0)
        {

        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * 6;
        }
        

    }
}
