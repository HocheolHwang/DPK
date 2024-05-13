using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShot : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
        Damage = 20;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/BubbleShot.png");
    }

    public override IEnumerator StartSkillCast()
    {
        Vector3 targetPos = _skillSystem.TargetPosition;
        _animator.CrossFade("CASTING_IN", 0.1f);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BubbleShotCoroutine(targetPos));

        yield return new WaitForSeconds(0.4f);
        _animator.CrossFade("CASTING_OUT", 0.1f);

        yield return new WaitForSeconds(0.5f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator BubbleShotCoroutine(Vector3 targetPos)
    {
        Managers.Sound.Play("Skill/BubbleShot");

        // 대상을 향해 회전하기
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = rotationToTarget;

        Vector3 rootForward = transform.TransformDirection(Vector3.forward);
        Vector3 rootUp = transform.TransformDirection(Vector3.up);
        Vector3 objectLoc = transform.position + rootForward + rootUp;

        HitBox hiddenbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hiddenbox.transform.localScale = new Vector3(2, 2, 2);
        hiddenbox.transform.rotation = transform.rotation;
        hiddenbox.transform.position = objectLoc;

        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.BubbleShot, 1.5f, gameObject.transform);
        ps1.transform.localScale = new Vector3(2, 2, 2);
        ps1.transform.position = hiddenbox.transform.position;

        float timer = 0;
        while (timer <= 3.0f)
        {
            Vector3 moveStep = hiddenbox.transform.forward * 7.0f * Time.deltaTime;
            hiddenbox.transform.position += moveStep;
            ps1.transform.position += moveStep;

            timer += Time.deltaTime;
            Debug.Log(Vector3.Distance(hiddenbox.transform.position, targetPos));
            if (Vector3.Distance(hiddenbox.transform.position, targetPos) < 1.1f)
            {
                Managers.Sound.Play("Skill/BubbleShotExplosion");

                HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
                hitbox.SetUp(transform, Damage);
                hitbox.transform.position = targetPos;

                ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.BubbleShotExplosion, 2.0f, hitbox.transform);
                ps2.transform.localScale = new Vector3(2, 2, 2);

                yield return new WaitForSeconds(0.1f);
                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Resource.Destroy(hiddenbox.gameObject);

                yield break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hiddenbox.gameObject);
    }
}
