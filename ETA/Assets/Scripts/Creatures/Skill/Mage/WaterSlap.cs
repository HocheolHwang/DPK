using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlap : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 10;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/WaterSlap.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        Vector3 targetPos = _skillSystem.TargetPosition;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(WaterSlapCoroutine(targetPos));

        yield return new WaitForSeconds(0.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }



    private IEnumerator WaterSlapCoroutine(Vector3 targetPos)
    {
        Managers.Sound.Play("Skill/WaterSlap");

        // 대상을 향해 회전하기
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = rotationToTarget;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.transform.position = targetPos;
        hitbox.transform.rotation = transform.rotation;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WaterSlap, 2.0f, gameObject.transform);
        ps.transform.position = hitbox.transform.position + transform.forward * -3;
        ps.transform.rotation = hitbox.transform.rotation;

        yield return new WaitForSeconds(0.20f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/WaterSlapEffect");

        Managers.Resource.Destroy(hitbox.gameObject);
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.WaterSlapEffect, 2.0f, transform);
        ps1.transform.position = hitbox.transform.position + transform.forward;
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.WaterSlapEffect1, 2.0f, transform);
        ps2.transform.position = hitbox.transform.position + transform.forward;
        ps2.transform.localScale = new Vector3(2, 2, 2);

        yield return new WaitForSeconds(0.20f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
