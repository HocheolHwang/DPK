using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Meteor : Skill
{
    private Vector3 startPos;
    private Vector3 endPos;
    protected override void Init()
    {
        SetCoolDownTime(20);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(5, 5, 5);
        RangeType = Define.RangeType.Round;
        Damage = 100;
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Meteor.png");
        // endPos.position = _skillSystem.TargetPosition;
        // startPos.position = endPos.position +  new Vector3(0f, 15f, 0f);
    }
    public override IEnumerator StartSkillCast()
    {
        endPos = _skillSystem.TargetPosition;
        startPos = gameObject.transform.position + Vector3.up * 15 + Vector3.back * 15;
        _animator.CrossFade("SKILL5", 0.1f);
        Managers.Sound.Play("Skill/Holy");

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(MeteorCoroutine());
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator MeteorCoroutine()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;

        // ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.FireTrail, 4.0f, transform);
        GameObject stone = Managers.Resource.Instantiate("Effect/Meteor", null);
        stone.transform.position = startPos;
        yield return new WaitForSeconds(0.8f);

        // 대상까지 이동하기
        float time = 0f;
        while (time < 2)
        {
            // 실제로 이동하기
            stone.transform.position += (endPos - startPos) / 2 * Time.deltaTime;
            time += Time.deltaTime; // Time.unscaledDeltaTime 사용
            yield return null; // 한 프레임 대기
        }

        Managers.Resource.Destroy(stone.gameObject);
        Managers.Sound.Play("Skill/Crash");

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage * 3);
        hitbox.transform.position = endPos;

        // hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.Explosion, 3.0f, hitbox.transform);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

}
