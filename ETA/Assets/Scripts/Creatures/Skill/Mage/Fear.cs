using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 20;
        base.Init();
        skillRange = new Vector3(2, 2, 2);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Fear.png");
    }

    public override IEnumerator StartSkillCast()
    {
        Vector3 targetPos = _skillSystem.TargetPosition;
        _animator.CrossFade("CASTING_IN", 0.1f);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FearCoroutine(targetPos));

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("CASTING_OUT", 0.1f);

        yield return new WaitForSeconds(0.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator FearCoroutine(Vector3 targetPos)
    {
        Managers.Sound.Play("Skill/Fear");
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;

        // 대상을 향해 회전하기
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = rotationToTarget;

        Vector3 rootForward = transform.TransformDirection(Vector3.forward);
        Vector3 rootUp = transform.TransformDirection(Vector3.up);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _controller.Stat.AttackDamage, -1, true, 3.0f);
        hitbox.transform.localScale = skillRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = objectLoc;

        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.Fear, 3.0f, gameObject.transform);
        ps1.transform.localScale = skillRange;
        ps1.transform.position = hitbox.transform.position;

        float timer = 0;
        while (timer <= 3.0f)
        {
            Vector3 moveStep = hitbox.transform.forward * 7.0f * Time.deltaTime;
            hitbox.transform.position += moveStep;
            ps1.transform.position += moveStep;

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
