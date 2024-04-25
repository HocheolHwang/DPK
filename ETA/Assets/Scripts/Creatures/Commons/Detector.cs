using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// [ 한 명만 타겟팅 ]
/// 1. 0.1초마다 인식 범위에 적이 있는지 판단한다.
/// 2. 있으면 가장 가까운 적을 target으로 세팅한다.
/// </summary>
public class Detector : MonoBehaviour
{
    [Header("Set Values from the Inspector")]
    [SerializeField] public float detectRange;
    [SerializeField] public float attackRange;              // 근거리, 원거리, 일반과 보스 몬스터는 공격 사거리가 다르다.
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask targetLayerMask;

    public Transform Target { get => _target; private set => _target = value; }

    private Ray _ray;

    private void Start()
    {
        _target = null;

        StartCoroutine(UpdateTarget());
    }

    private void OnDrawGizmos()
    {
        _ray.origin = transform.position;
        Gizmos.color = Color.red;
        if (Target == null ) Gizmos.DrawWireSphere(_ray.origin, detectRange);
        else Gizmos.DrawWireSphere(_ray.origin, attackRange);
    }

    public IEnumerator UpdateTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            Target = null;
            float closeDist = Mathf.Infinity;
            Collider[] enemies = Physics.OverlapSphere(transform.position, detectRange, targetLayerMask);

            foreach (Collider enemy in enemies)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy < closeDist)
                {
                    closeDist = distToEnemy;
                    Target = enemy.transform;
                }
            }
        }
    }

    public bool IsArriveToTarget()
    {
        return Vector3.Distance(_target.position, transform.position) < attackRange;
    }
}
