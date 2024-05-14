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
        SetCoolDownTime(15);
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
        startPos = endPos +  new Vector3(0f, 15f, 0f);
        _animator.CrossFade("SKILL5", 0.1f);
        Managers.Sound.Play("Skill/Holy");

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(MeteorCoroutine());
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator MeteorCoroutine()
    {
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.FireTrail, 4.0f, transform);
        ps2.transform.position = startPos;
        yield return new WaitForSeconds(0.8f);

        // 대상과의 거리 계산
        float distanceToTarget = Vector3.Distance(ps2.transform.position, endPos);
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
            ps2.transform.position = Vector3.Lerp(ps2.transform.position, endPos, elapsedTime / moveTime);
            elapsedTime += Time.unscaledDeltaTime; // Time.unscaledDeltaTime 사용
            yield return null; // 한 프레임 대기
        }

        Managers.Sound.Play("Skill/Crash");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position= endPos;
        
        // hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.Explosion, 3.0f, hitbox.transform);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

}
