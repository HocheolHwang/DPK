using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWave : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(10);
        Damage = 100;
        skillRange = new Vector3(4, 2, 6);
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/SwordWave.png");
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
            ParticleSystem ps = Managers.Effect.Play(Define.Effect.SwordWaveWhite, 0.0f, gameObject.transform);
            Managers.Sound.Play("Skill/TargetSkill");


            StartCoroutine(SwordWaveCoroutine(ps));

            yield return new WaitForSeconds(0.2f);
            
        }
        

        

        yield return new WaitForSeconds(0.4f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator SwordWaveCoroutine(ParticleSystem ps)
    {
        float duration = 0.5f;
        float time = 0;
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 5f;
        hitbox.transform.localScale = skillRange;
        while (time < duration)
        {
            hitbox.transform.position += gameObject.transform.forward * 16 * Time.deltaTime;
            ps.transform.position += gameObject.transform.forward * 16 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
