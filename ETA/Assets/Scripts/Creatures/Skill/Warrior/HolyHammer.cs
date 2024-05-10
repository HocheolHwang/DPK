using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHammer : Skill
{
    private Coroutine holyhammerCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(10);
        base.Init();
        SkillType = Define.SkillType.Range;
        Damage = 30;
        skillRange = new Vector3(3, 3, 3);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/HolyHammer.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("BUFF1", 0.1f);
        Managers.Sound.Play("Skill/Holy");

        holyhammerCoroutine = StartCoroutine(HolyHammerCoroutine());

        yield return new WaitForSeconds(1.8f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator HolyHammerCoroutine()
    {
        // Hammer prefab을 읽어오고 생성
        GameObject hammerPrefab = Managers.Resource.Instantiate("Effect/Hammer");
        // Hammer prefab을 타겟 위치로 이동
        hammerPrefab.transform.position = transform.position + new Vector3(0f, 5f, 0f);
        yield return new WaitForSeconds(0.8f);

        // Hammer prefab을 플레이어를 향하도록 회전
        Vector3 direction = transform.position + new Vector3(0f, 5f, 0f) - _skillSystem.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90f, 0f, 0f);
        hammerPrefab.transform.rotation = rotation;

        // 대상과의 거리 계산
        float distanceToTarget = Vector3.Distance(hammerPrefab.transform.position, _skillSystem.TargetPosition);
        // 이동 속도 계산 (1초에 도달할 거리)
        float moveSpeed = 30f;
        // 대상까지 도달하기 위한 이동 시간 계산
        float moveTime = distanceToTarget / moveSpeed;

        _animator.CrossFade("ATTACK4", 0.1f);
        yield return new WaitForSeconds(0.1f);
        // 대상까지 이동하기
        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            // 실제로 이동하기
            hammerPrefab.transform.position = Vector3.Lerp(hammerPrefab.transform.position, _skillSystem.TargetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }
        // hammerPrefab.transform.position = _skillSystem.TargetPosition;

        Managers.Sound.Play("Skill/Crash");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SurfaceExplosionDirtStone, 2.0f, hitbox.transform);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(0.8f);
        Managers.Resource.Destroy(hammerPrefab.gameObject);
    }
}
