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
    }

    public override IEnumerator StartSkillCast()
    {
        ParticleSystem ps = Managers.Resource.Instantiate("Effect/Stone Slash").GetComponent<ParticleSystem>();
        ps.transform.position = transform.position + new Vector3(0f, 1f, 0f);

        // 애니메이션과 이펙트 재생
        for (int i = 0; i < 4; i++)
        {
            // 1, 3번째 칼 휘두를 때는 왼쪽으로, 2, 4번째 칼 휘두를 때는 오른쪽으로 이펙트 방향 조절
            float direction = (i % 2 == 0) ? -1f : 1f;
            ps.transform.rotation = Quaternion.Euler(direction * 45f,  -90f, 0f);

            if (i % 2 == 0)
                _animator.CrossFade("ATTACK1", 0.1f); // 1번째와 3번째 칼 휘두르기
            else
                _animator.CrossFade("ATTACK2", 0.1f); // 2번째와 4번째 칼 휘두르기

            ps.Play();

            // 히트박스 생성
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition;
            hitbox.transform.localScale = skillRange;

            yield return new WaitForSeconds(0.3f);
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.5f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
