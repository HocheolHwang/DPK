using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IprisPatternTwo : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 3.5f;
    [SerializeField] float _upPos = 1.0f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.1f;
        _patternDmg = 50;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 destToTarget = MonsterManager.Instance.GetBackPosPlayer(_controller.transform);
        
        float duration = CalcTimeToDest(destToTarget);
        Debug.Log($"이게 몇초나 나올까 {duration}");
        yield return new WaitForSeconds(duration);

        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = _controller.transform.position + rootUp;

        // 내려찍기 수행
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.transform.position = Pos;
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;

        // EFFECT
        ParticleSystem rushingPS = Managers.Effect.Play(Define.Effect.Ipris_PatternTwo, duration, transform);
        rushingPS.transform.position = Pos;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    private float CalcTimeToDest(Vector3 Destination)
    {
        NavMeshAgent agent = _controller.GetComponent<NavMeshAgent>();
        float moveSpeed = agent.speed;
        if (moveSpeed <= 0.1f)
        {
            Debug.Log($"{_controller.gameObject.name}의 속도({moveSpeed})가 0.1f보다 낮습니다.");
            return -1;
        }
        else if (moveSpeed > 8.0f)
        {
            moveSpeed = 8.0f;
        }

        float remainDist = Vector3.Distance(Destination, _controller.transform.position);
        if (remainDist < 2.0f)
        {
            remainDist = 2.0f;
        }

        float timeToDest = remainDist / moveSpeed;
        return timeToDest;
    }
}
