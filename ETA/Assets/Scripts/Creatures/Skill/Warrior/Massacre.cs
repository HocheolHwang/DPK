using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massacre : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(7, 7, 7);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Massacre.png");
    }

    public override IEnumerator StartSkillCast()
    {
        StartCoroutine(MassacreCoroutine());

        yield return new WaitForSeconds(0.7f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator MassacreCoroutine()
    {
        // 애니메이션과 이펙트 재생
        for (int i = 0; i < 4; i++)
        {
            Managers.Sound.Play("Skill/NormalAttack");
            Define.Effect effectName = (i % 2 == 0) ? Define.Effect.StoneSlash1 : Define.Effect.StoneSlash2;

            if (i % 2 == 0)
                _animator.CrossFade("ATTACK1", 0.1f); // 1번째와 3번째 칼 휘두르기
            else
                _animator.CrossFade("ATTACK2", 0.1f); // 2번째와 4번째 칼 휘두르기

            ParticleSystem ps = Managers.Effect.Play(effectName, 1.0f, gameObject.transform);

            // 히트박스 생성
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition + transform.forward * 1.5f;
            hitbox.transform.localScale = skillRange;

            yield return new WaitForSeconds(0.3f);
            Managers.Effect.Stop(ps);
            Managers.Resource.Destroy(hitbox.gameObject);
        }
    }
}
