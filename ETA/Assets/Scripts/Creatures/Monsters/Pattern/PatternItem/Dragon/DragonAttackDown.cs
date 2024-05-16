using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackDown : Pattern
{
    [Header("hit box options")]
    [SerializeField] float _forwardPos = 4.8f;
    [SerializeField] float _upPos = 0.1f;
    [SerializeField] float _hitboxRadius = 5.5f;

    [Header("knockback box options")]
    [SerializeField] private float _knockBackCreateTime = 0.15f;
    [SerializeField] private int _knockBackPower = 5;
    [SerializeField] private float _knockBackTime = 0.5f;

    private DragonAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 1.05f;
        _animData = GetComponent<DragonAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        yield return new WaitForSeconds(_createTime);

        StartCoroutine(AttackDown(Pos));

        yield return new WaitForSeconds(_animData.DownAttackAnim.length + _createTime);
        // coroutine 유지를 위한 잠깐의 시간
    }

    IEnumerator AttackDown(Vector3 Pos)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_AttackDown, 0);
        ps.transform.position = Pos;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.position = Pos;

        // create the knockback box after remove the hitbox 
        yield return new WaitForSeconds(_knockBackCreateTime);
        Managers.Resource.Destroy(hitbox.gameObject);

        
        KnockBackBox knockBackBox = Managers.Resource.Instantiate("Skill/KnockBackBoxCircle").GetComponent<KnockBackBox>();
        knockBackBox.SetUp(transform, _knockBackPower, _knockBackTime);
        knockBackBox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        knockBackBox.transform.position = Pos;

        Debug.Log($"knockBack box: {knockBackBox.gameObject.name}");

        yield return new WaitForSeconds(_knockBackCreateTime);
        Managers.Resource.Destroy(knockBackBox.gameObject);
    }
}
