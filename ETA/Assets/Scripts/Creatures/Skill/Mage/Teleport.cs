using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(5);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Teleport.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("TELEPORT_IN", 0.1f);
        Managers.Sound.Play("Skill/Teleport");
        StartCoroutine(TeleportCoroutine());

        yield return new WaitForSeconds(0.2f);
        gameObject.transform.position -= gameObject.transform.forward * 3;

        yield return new WaitForSeconds(0.1f);
        _animator.CrossFade("TELEPORT_OUT", 0.1f);

        yield return new WaitForSeconds(0.5f);
        // _controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator TeleportCoroutine()
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Teleport, 1.0f, transform);
        if (ps != null)
            ps.transform.SetParent(gameObject.transform);

        yield return new WaitForSeconds(0.1f);
    }
}
