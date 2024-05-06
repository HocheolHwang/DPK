using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWave : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 30;
        skillRange = new Vector3(2, 2, 2);
        base.Init();
        skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/SwordWave.png");
    }

    public override IEnumerator StartSkillCast()
    {
        // 대상을 향해 회전하기
        Vector3 directionToTarget = (_skillSystem.TargetPosition - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = rotationToTarget;

        _animator.CrossFade("ATTACK3", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            ParticleSystem ps = Managers.Effect.Play(Define.Effect.SwordWaveWhite, gameObject.transform);
            Managers.Sound.Play("Skill/TargetSkill");
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = gameObject.transform.position + transform.forward * 1f;
            hitbox.transform.localScale = skillRange;

            yield return new WaitForSeconds(0.2f);
            Managers.Resource.Destroy(hitbox.gameObject);
            Managers.Effect.Stop(ps);
        }

        yield return new WaitForSeconds(0.4f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
