using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// [ 한 명만 타겟팅 ]
/// 1. 0.5초마다 인식 범위에 적이 있는지 판단한다.
/// 2. 있으면 가장 가까운 적을 target으로 세팅한다.
/// 
/// [ 타겟팅한 적이 공격 범위에 있나?]
/// 1. Distance를 계산
/// </summary>
public class Detector : MonoBehaviour
{
    [Header("Set Values from the Inspector")]
    [SerializeField] public float detectRange;
    [SerializeField] public float attackRange;
    [SerializeField] public LayerMask targetLayerMask;

    [SerializeField] public Transform target {  get; private set; }

    private Ray ray;

    private void Start()
    {
        target = null;

        StartCoroutine(UpdateTarget());
    }

    private void OnDrawGizmos()
    {
        ray.origin = transform.position;
        Gizmos.color = Color.red;
        if (target == null ) Gizmos.DrawWireSphere(ray.origin, detectRange);
        else Gizmos.DrawWireSphere(ray.origin, attackRange);
    }

    IEnumerator UpdateTarget()
    {
        while (true)
        {
            Debug.Log("Detector - UpdateTarget");
            yield return new WaitForSeconds(0.5f);

            target = null;
            float closeDist = Mathf.Infinity;
            Collider[] enemies = Physics.OverlapSphere(transform.position, detectRange, targetLayerMask);

            foreach (Collider enemy in enemies)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy < closeDist)
                {
                    closeDist = distToEnemy;
                    target = enemy.transform;
                }
            }
        }
    }
}
