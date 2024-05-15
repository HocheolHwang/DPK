using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackDown : Pattern
{
    [Header("hit box options")]
    [SerializeField] float _forwardPos = 1.0f;
    [SerializeField] float _hitboxRadius = 1.0f;

    [Header("knockback box options")]
    [SerializeField] private float _knockBackCreateTime = 0.15f;
    [SerializeField] private int _knockBackPower = 5;
    [SerializeField] private float _knockBackTime = 0.5f;


    public override void Init()
    {
        base.Init();

        _createTime = 0.4f;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 Pos = transform.position + rootForward;

        yield return new WaitForSeconds(_createTime);

        StartCoroutine(AttackDown(Pos));

        yield return new WaitForSeconds(_createTime);
        // coroutine 유지를 위한 잠깐의 시간
    }

    IEnumerator AttackDown(Vector3 Pos)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Ipris_AttackSecond, transform);
        ps.transform.position = Pos;
        ps.transform.rotation = _controller.transform.rotation;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.position = Pos;

        //Managers.Sound.Play("Sounds/Monster/Ipris/IprisAttackSecond_SND", Define.Sound.Effect);

        // create the knockback box after remove the hitbox 
        yield return new WaitForSeconds(_knockBackCreateTime);
        Managers.Resource.Destroy(hitbox.gameObject);

        KnockBackBox knockBackBox = Managers.Resource.Instantiate("Skill/KnockBackBoxCircle").GetComponent<KnockBackBox>();
        knockBackBox.SetUp(transform, _knockBackPower, _knockBackTime);
        knockBackBox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        knockBackBox.transform.position = Pos;

        yield return new WaitForSeconds(_knockBackCreateTime);
        Managers.Resource.Destroy(knockBackBox.gameObject);
    }
}
