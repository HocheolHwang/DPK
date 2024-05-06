using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlWind : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(5, 5, 5);
        skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Whirlwind.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("WHIRLWIND", 0.1f);
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.WhirlwindEffect1, gameObject.transform);
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.WhirlwindEffect2, gameObject.transform);
        for (int i = 0; i < 8; i++)
        {
            Managers.Sound.Play("Skill/Whirlwind");
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = gameObject.transform.position;
            hitbox.transform.localScale = skillRange;
            yield return new WaitForSeconds(0.2f);
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Effect.Stop(ps1);
        Managers.Effect.Stop(ps2);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
