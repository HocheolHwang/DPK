using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightning : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(15, 15, 15);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/ChainLightning.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL3", 0.1f);
        Vector3 targetPos = gameObject.transform.position;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ChainLightningCoroutine(targetPos));

        yield return new WaitForSeconds(0.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator ChainLightningCoroutine(Vector3 targetPos)
    {
        Managers.Sound.Play("Skill/FlashLight");
        List<Transform> monstersInHitbox = new List<Transform>();
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Thunder3, 2.0f, transform);
        ps.transform.position = gameObject.transform.position;
        ps.transform.localScale = skillRange / 5;

        // 히트박스 내부의 "Monster" 태그를 가진 오브젝트 찾기
        Collider[] colliders = Physics.OverlapBox(targetPos, skillRange / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Monster"))
            {
                monstersInHitbox.Add(collider.transform);
            }
        }

        // 내 위치와 monstersInHitbox 요소들 간의 거리 계산 및 정렬
        List<(Transform, float)> distanceSorted = monstersInHitbox
            .Select(m => (m, Vector3.Distance(transform.position, m.position)))
            .OrderBy(t => t.Item2)
            .ToList();

        // 거리 순서대로 hitbox 생성 및 위치 설정
        foreach ((Transform monster, float distance) in distanceSorted)
        {
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.localScale = new Vector3(1, 1, 1);
            hitbox.transform.position = monster.position;

            ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.ChainLightning, 2.0f, transform);
            ps1.transform.position = hitbox.transform.position;
            ps1.transform.localScale = skillRange / 5;
            ps.transform.position = hitbox.transform.position;

            yield return new WaitForSeconds(0.1f); // 약간의 시간 차이를 두고 hitbox 생성
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.3f);
        // Managers.Resource.Destroy(hitbox.gameObject);
    }
}
