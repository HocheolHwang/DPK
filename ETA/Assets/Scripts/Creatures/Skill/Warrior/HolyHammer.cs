using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHammer : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(7, 7, 7);
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("BUFF1", 0.1f);
        Managers.Sound.Play("Skill/Holy");
        // Hammer prefab을 읽어오고 생성
        GameObject hammerPrefab = Managers.Resource.Instantiate("Effect/Hammer");
        // Hammer prefab을 타겟 위치로 이동
        hammerPrefab.transform.position = transform.position + new Vector3(0f, 5f, 0f);
        yield return new WaitForSeconds(0.8f);

        // Hammer prefab을 플레이어를 향하도록 회전
        Vector3 direction = transform.position + new Vector3(0f, 5f, 0f) - _skillSystem.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90f, 0f, 0f);
        hammerPrefab.transform.rotation = rotation;
        hammerPrefab.transform.position = _skillSystem.TargetPosition;

        _animator.CrossFade("ATTACK4", 0.1f);
        Managers.Sound.Play("Skill/Crash");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SurfaceExplosionDirtStone, hitbox.transform);

        yield return new WaitForSeconds(0.8f);
        Managers.Effect.Stop(ps);
        Managers.Resource.Destroy(hammerPrefab.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
