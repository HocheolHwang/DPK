using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingShield : Skill
{
    private Coroutine thrwoingShield;
    private float duration = 2.0f;

    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 50;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/TelekineticSwords.png");
        SkillType = Define.SkillType.Target;
    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _animator.CrossFade("THROW_SHIELD", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);

        thrwoingShield = Managers.Coroutine.Run(ThrowingShieldCoroutine());

        yield return new WaitForSeconds(0.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }



    private IEnumerator ThrowingShieldCoroutine()
    {
        Managers.Sound.Play("Skill/ThrowingShield");

        Vector3 destination = _skillSystem.TargetPosition;

        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.ThrowingShield, duration, transform);
        ps1.transform.parent = null;
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true, duration);
        hitbox.transform.parent = ps1.transform;
        hitbox.transform.localScale = new Vector3(1,1,1);
        hitbox.transform.localPosition = new Vector3(0, 0, 0);

        float time = 0;
        Vector3 dir = (destination - transform.position).normalized;
        while(time < duration)
        {
            ps1.transform.rotation *= Quaternion.Euler(0, 0, 15);
            ps1.transform.position += dir * 20 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        
    }
}
