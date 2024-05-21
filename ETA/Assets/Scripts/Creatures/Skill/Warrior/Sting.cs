using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sting : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 50;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Sting.png");
    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        // 대상을 향해 회전하기
        Vector3 directionToTarget = (_skillSystem.TargetPosition - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = rotationToTarget;

        // 애니메이션 실행
        _animator.CrossFade("SKILL5", 0.1f);
        yield return new WaitForSeconds(0.1f);

        // 대상과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, _skillSystem.TargetPosition)-1f;

        // 이동 속도 계산 (1초에 도달할 거리)
        float moveSpeed = 25f;

        // 대상까지 도달하기 위한 이동 시간 계산
        float moveTime = distanceToTarget / moveSpeed;

        // 대상까지 이동하기
        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            // 실제로 이동하기
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }
        // ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/SwordVolleyBlue").GetComponent<ParticleSystem>();

        yield return new WaitForSeconds(0.2f);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage * 2, -1, true);
        hitbox.transform.position = _skillSystem.TargetPosition;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/TargetSkill");

        yield return new WaitForSeconds(0.5f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }
}
