using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceBone : Skill
{
    private Coroutine iceboneCoroutine;
    protected override void Init()
    {
        SetCoolDownTime(1);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(1, 3, 1);
        RangeType = Define.RangeType.Round;
        Damage = 15;
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/IceBone.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL4", 0.1f);
        Managers.Sound.Play("Skill/Holy");

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 10; i++)
        {
            iceboneCoroutine = StartCoroutine(IceBoneCoroutine());
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.25f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator IceBoneCoroutine()
    {
        // skillRange 내에서 랜덤한 x, z 좌표 생성
        float randomX = Random.Range(-skillRange.x / 2f, skillRange.x / 2f);
        float randomZ = Random.Range(-skillRange.z / 2f, skillRange.z / 2f);

        // y 좌표는 현재 위치의 y 좌표 사용
        Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition + randomPosition - transform.up;
        hitbox.transform.localScale = skillRange;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SpikeIce, hitbox.transform);
        Managers.Sound.Play("Skill/TargetSkill");

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(3.0f);
        Managers.Effect.Stop(ps);
    }

}
