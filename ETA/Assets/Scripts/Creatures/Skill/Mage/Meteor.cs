using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Skill
{
    private Coroutine meteorCoroutine;
    protected override void Init()
    {
        SetCoolDownTime(1);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(5, 5, 5);
        RangeType = Define.RangeType.Round;
        Damage = 200;
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/HolyHammer.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL5", 0.1f);
        Managers.Sound.Play("Skill/Holy");

        yield return new WaitForSeconds(2.0f);
        meteorCoroutine = StartCoroutine(MeteorCoroutine());
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator MeteorCoroutine()
    {
        // Hammer prefab을 읽어오고 생성
        GameObject hammerPrefab = Managers.Resource.Instantiate("Skill/HitBoxRect");
        // Hammer prefab을 타겟 위치로 이동
        hammerPrefab.transform.position = _skillSystem.TargetPosition + new Vector3(0f, 15f, 0f);
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.FireTrail, hammerPrefab.transform);
        yield return new WaitForSeconds(0.8f);

        // 대상과의 거리 계산
        float distanceToTarget = Vector3.Distance(ps2.transform.position, _skillSystem.TargetPosition);
        // 이동 속도 계산 (1초에 도달할 거리)
        float moveSpeed = 0.1f; // 이동 속도 조절
        // 대상까지 도달하기 위한 이동 시간 계산
        float moveTime = distanceToTarget / moveSpeed;
        Debug.Log($"moveTime: {moveTime}");

        // 대상까지 이동하기
        float elapsedTime = 0f;
        while (elapsedTime < 2)
        {
            // 실제로 이동하기
            ps2.transform.position = Vector3.Lerp(ps2.transform.position, _skillSystem.TargetPosition, elapsedTime / moveTime);
            elapsedTime += Time.unscaledDeltaTime; // Time.unscaledDeltaTime 사용
            yield return null; // 한 프레임 대기
        }

        Managers.Sound.Play("Skill/Crash");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.Explosion, hitbox.transform);
        yield return new WaitForSeconds(0.01f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Resource.Destroy(hammerPrefab.gameObject);
        Managers.Effect.Stop(ps2);

        yield return new WaitForSeconds(2.0f); // 이펙트 발동 시점 조절
        Managers.Effect.Stop(ps1);
    }

}
