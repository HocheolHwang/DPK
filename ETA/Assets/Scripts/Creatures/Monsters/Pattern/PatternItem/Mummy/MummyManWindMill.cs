using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MummyManWindMill : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 4.0f;
    [SerializeField] float _upPos = 1.0f;

    private MummyManAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 0.2f;
        _patternDmg = 20;

        _animData = _controller.GetComponent<MummyManAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        Debug.Log("!!!!윈드밀 써라 데발");
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 windPos = transform.position + rootUp;

        // wind mill
        yield return new WaitForSeconds(_createTime);
        Managers.Coroutine.Run(CreateWindMill(AttackDamage + _patternDmg, windPos));
        
    }

    IEnumerator CreateWindMill(int attackDMG, Vector3 Pos)
    {

        Debug.Log("윈드밀 발생");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, attackDMG);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = Pos;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Mummy_WindMill, 0, _controller.transform);
        ps.transform.position = hitbox.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Effect.Stop(ps);
    }
}
