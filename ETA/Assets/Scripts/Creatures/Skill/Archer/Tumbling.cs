using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbling : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        //skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("Tumbling", 0.1f);
        Managers.Sound.Play("Skill/Tumbling");
        Managers.Coroutine.Run(BackMove());
        yield return new WaitForSeconds(0.45f);


        ChangeToPlayerMoveState();
    }

    IEnumerator BackMove()
    {
        float duration = 0.3f;
        float time = 0;
        while (time < duration)
        {
            _controller.Agent.Move(gameObject.transform.forward * -12 * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }


    }
}
