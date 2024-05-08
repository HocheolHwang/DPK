using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MummyWarriorWindMill : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 4.0f;
    [SerializeField] float _upPos = 1.0f;

    private MummyWarriorAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 0.2f;
        _patternDmg = 10;

        _animData = _controller.GetComponent<MummyWarriorAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 windPos = transform.position + rootUp;

        // wind mill
        yield return new WaitForSeconds(_createTime);
        StartCoroutine(CreateWindMill(AttackDamage + _patternDmg, windPos));
    }

    IEnumerator CreateWindMill(int attackDMG, Vector3 Pos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, attackDMG);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = Pos;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.MummyWarrior_WindMill, _controller.transform);
        ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(_animData.WindMillAnim.length);
        Managers.Resource.Destroy(ps.gameObject);

        hitbox.SetActiveCollider();

        ps = Managers.Effect.Play(Define.Effect.MummyWarrior_WindMill, _controller.transform);
        ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(ps.main.duration);
        Managers.Resource.Destroy(ps.gameObject);
    }
}
