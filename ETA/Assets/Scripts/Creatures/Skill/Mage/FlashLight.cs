using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(10);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(30, 10, 30);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/FlashLight.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FlashLightCoroutine());

        yield return new WaitForSeconds(0.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator FlashLightCoroutine()
    {
        Managers.Sound.Play("Skill/FlashLight");
        HitBox hiddenbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hiddenbox.transform.position = gameObject.transform.position + transform.up * 3;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.FlashLight, 2.0f, hiddenbox.transform);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(0.3f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
