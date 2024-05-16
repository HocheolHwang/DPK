using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackSwing : Pattern
{
    [Header("box options")]
    [SerializeField] float _forwardPos = 5.0f;
    [SerializeField] float _upPos = 0.5f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(7.0f, 1.0f, 6.0f);
    [SerializeField] Vector3 _psRange = new Vector3(1.8f, 1.0f, 1.0f);
    [SerializeField] float _addRotation_Z = 13.0f;

    private DragonAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 0.6f;
        _animData = GetComponent<DragonAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        yield return new WaitForSeconds(_createTime);

        StartCoroutine(AttackSwing(Pos));

        yield return new WaitForSeconds(_animData.SwingAttackAnim.length + _createTime);
        // coroutine 유지를 위한 잠깐의 시간
    }

    IEnumerator AttackSwing(Vector3 Pos)
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_AttackSwing, 0);
        ps.transform.localScale = _psRange;
        ps.transform.position = Pos;
        ps.transform.rotation = _controller.transform.rotation;
        Quaternion addRotation = Quaternion.Euler(0, 0, _addRotation_Z);
        ps.transform.rotation *= addRotation;
        
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _hitboxRange;
        hitbox.transform.position = Pos;
        hitbox.transform.rotation = _controller.transform.rotation;
        hitbox.transform.rotation *= addRotation;

        // remove the hitbox 
        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
