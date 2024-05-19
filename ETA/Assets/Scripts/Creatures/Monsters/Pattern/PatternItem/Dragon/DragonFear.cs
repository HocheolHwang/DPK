using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFear : Pattern
{
    [Header("box options")]
    [SerializeField] float _hitboxRadius = 3.0f;
    [SerializeField] float _expandSpeed = 20f;
    [SerializeField] float _duration = 1.0f;

    private DragonAnimationData _animData;
    private DragonController _dcontroller;

    public override void Init()
    {
        base.Init();

        _createTime = 0.8f;
        _patternDmg = 10;
        _animData = GetComponent<DragonAnimationData>();
        _dcontroller = GetComponent<DragonController>();
    }

    public override IEnumerator StartPatternCast()
    {
        // 머리가 가장 위로 올라갔을 때 hitbox 생성 및 effect 재생
        yield return new WaitForSeconds(_createTime);

        Vector3 Pos = _dcontroller.FearEnableEffect.transform.position;
        StartCoroutine(Fear(Pos));

        yield return new WaitForSeconds(_duration + _animData.FearAttackAnim.length);
    }

    IEnumerator Fear(Vector3 Pos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _patternDmg, -1, false, _duration);
        hitbox.transform.rotation = _dcontroller.transform.rotation;
        hitbox.transform.position = Pos;
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Dragon_Fear, 0);
        ps.transform.position = hitbox.transform.position;

        Managers.Sound.Play("Sounds/Monster/Dragon/DragonFear_SND", Define.Sound.Effect);

        float timer = 0;
        while (timer <= _duration)
        {
            hitbox.GetComponent<SphereCollider>().radius += _expandSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    protected virtual void StrongLogic()
    {

    }

    protected virtual void AddGroggy()
    {

    }
}
