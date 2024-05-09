using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MummyManRush : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(6.0f, 2.0f, 1.0f);
    [SerializeField] float _upPos = 1.0f;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        _patternRange = _hitboxRange;
        _patternDmg = 150;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = _controller.transform.position + rootUp;

        Vector3 destToTarget = MonsterManager.Instance.GetBackPosPlayer(_controller.transform);
        destToTarget.y = _controller.transform.position.y;
        float duration = CalcTimeToDest(destToTarget);

        Debug.Log($"speed: {_controller.Stat.MoveSpeed} | destToTarget: {destToTarget} | duration: {duration}");

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg / 3, -1, false, duration);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.SetParent(_controller.transform);
        hitbox.transform.position = Pos;

        // EFFECT
        ParticleSystem rushingPS = Managers.Effect.Play(Define.Effect.Mummy_Rushing, transform);
        rushingPS.transform.SetParent(_controller.transform);
        rushingPS.transform.position = Pos;

        yield return new WaitForSeconds(duration);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Effect.Stop(rushingPS);

        // HIT BOX
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.GetComponent<SphereCollider>().radius = 6.0f;
        hitbox.transform.position = _controller.transform.position;
        // EFFECT
        ParticleSystem rushEndPS = Managers.Effect.Play(Define.Effect.Mummy_RushEnd, transform);
        rushEndPS.transform.position = _controller.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(rushEndPS.main.duration);
        Managers.Effect.Stop(rushEndPS);
    }

    private float CalcTimeToDest(Vector3 Destination)
    {
        NavMeshPath path = new NavMeshPath();
        NavMeshAgent agent = _controller.GetComponent<NavMeshAgent>();
        Destination.y = _controller.transform.position.y;

        // 현재 위치에서 목적지까지의 경로 계산
        if (agent.CalculatePath(Destination, path))
        {
            float pathLen = GetPathLength(path);
            return pathLen / agent.speed;
        }

        return -1;
    }

    private float GetPathLength(NavMeshPath path)
    {
        float len = 0;

        if (path.corners.Length < 2)
        {
            Debug.Log("유효하지 않은 경로");
            return len;
        }

        // 경로상의 모든 코너점을 순회하면서 두 점 사이의 거리를 구함
        for (int i = 0; i < path.corners.Length - 1; ++i)
        {
            len += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return len;
    }
}
