using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSword : Skill
{
    private Coroutine telekineticswordsCoroutine;
    Vector3 dest;
    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 20;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/TelekineticSwords.png");
        
    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _animator.CrossFade("SKILL1", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);
        Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, transform);
        GameObject go = Managers.Resource.Instantiate("Effect/MoonSword", null);
        go.transform.position = _skillSystem.TargetPosition + new Vector3(0, 20, 0);
        dest = _skillSystem.TargetPosition;

        telekineticswordsCoroutine = StartCoroutine(MoonSwordsCoroutine(go));
        yield return new WaitForSeconds(1.0f);
        Managers.Coroutine.Run(CreateHitBox());
        yield return new WaitForSeconds(1.5f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }



    private IEnumerator MoonSwordsCoroutine(GameObject go)
    {
        Managers.Sound.Play("Skill/MoonSword");
        float duration = 2.0f;
        float time = 0;
        while(time < duration)
        {
            go.transform.position += Vector3.down * Time.deltaTime * 8 * time;
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Managers.Resource.Destroy(go);
    }

    IEnumerator CreateHitBox()
    {
        for(int i = 0; i < 10; i++)
        {
            Managers.Sound.Play("Skill/TargetSkill");
            // HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            // hitbox.SetUp(transform, Damage);
            // hitbox.transform.position = dest;
            // yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.4f);
        Managers.Sound.Play("Skill/TargetSkill");
        HitBox box = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        box.SetUp(transform, Damage * 3);
        box.transform.position = dest;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.MoonSwordEffect, 0, box.transform);
        ps.transform.position = dest;
        

    }
}
