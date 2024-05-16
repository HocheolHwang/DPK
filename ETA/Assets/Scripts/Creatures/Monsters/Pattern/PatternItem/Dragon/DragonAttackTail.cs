using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackTail : Pattern
{
    [Header("box options")]
    [SerializeField] float _forwardPos = 4.0f;
    [SerializeField] float _upPos = 0.5f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(12.0f, 1.0f, 8.0f);

    [Header("knockback box options")]
    [SerializeField] private int _knockBackPower = 1;
    [SerializeField] private float _knockBackTime = 0.5f;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;

    public override void Init()
    {
        base.Init();

        _createTime = 1.2f;
        _animData = GetComponent<DragonAnimationData>();
        _dcontroller = GetComponent<DragonController>();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * _forwardPos);
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        _dcontroller.TailEffect.Play();

        yield return new WaitForSeconds(_createTime);

        StartCoroutine(AttackTail(Pos));

        yield return new WaitForSeconds(_animData.TailAttackAnim.length - _createTime);
        // coroutine 유지를 위한 잠깐의 시간
        _dcontroller.TailEffect.Stop();
    }

    IEnumerator AttackTail(Vector3 Pos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage);
        hitbox.transform.localScale = _hitboxRange;
        hitbox.transform.position = Pos;
        hitbox.transform.rotation = _controller.transform.rotation;

        // DragonTail_SND
        Managers.Sound.Play("Sounds/Monster/Dragon/DragonTail_SND", Define.Sound.Effect);

        KnockBackBox knockBackBox = Managers.Resource.Instantiate("Skill/KnockBackBoxRect").GetComponent<KnockBackBox>();
        knockBackBox.SetUp(transform, _knockBackPower, _knockBackTime);
        knockBackBox.transform.localScale = _hitboxRange;
        knockBackBox.transform.position = Pos;

        // remove the hitbox 
        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Resource.Destroy(knockBackBox.gameObject);
    }
}
