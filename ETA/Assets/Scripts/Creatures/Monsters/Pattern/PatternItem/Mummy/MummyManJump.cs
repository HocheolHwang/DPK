using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManJump : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxRadius = 4.0f;

    private MummyManAnimationData _data;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        _patternDmg = 40;
        _data = GetComponent<MummyManAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        ParticleSystem auraPS = Managers.Effect.Play(Define.Effect.Mummy_JumpAura, transform);
        Managers.Sound.Play("Sounds/Monster/Mummy/MummyJumpAura_SND", Define.Sound.Effect);

        auraPS.transform.SetParent(_controller.transform);
        float duration = _data.JumpAnim.length * 2.0f;

        yield return new WaitForSeconds(duration);

        Managers.Effect.Stop(auraPS);

        ParticleSystem downPS = Managers.Effect.Play(Define.Effect.Mummy_JumpDown, transform);
        downPS.transform.position = transform.position;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage + _patternDmg);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = downPS.transform.rotation;
        hitbox.transform.position = downPS.transform.position;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(downPS.main.duration);
        Managers.Effect.Stop(downPS);
    }
}
