using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyBufferRangedAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _duration = 1.5f;
    [SerializeField] float _speed = 14.0f;
    [SerializeField] float _hitboxRadius = 0.3f;

    private int penetration = 1;
    private Transform rightHandPos;

    public override void Init()
    {
        base.Init();

        rightHandPos = GetComponent<MummyBufferController>().StoneSpawned.transform;
        _createTime = 1f;
    }

    public override IEnumerator StartPatternCast()
    {
        // 오른손에 stone 생성
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Mummy_RangedAttack, transform);
        ps.transform.position -= transform.up;
        ps.transform.SetParent(rightHandPos.transform);
        
        yield return new WaitForSeconds(_createTime);   // throw the stone

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _attackDamage, penetration, false, _duration);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;

        ps.transform.SetParent(null);
        ps.transform.rotation = _controller.transform.rotation;
        hitbox.transform.rotation = _controller.transform.rotation;
        hitbox.transform.position = ps.transform.position;

        Managers.Sound.Play("Monster/Mummy/MummyRangedAttack_SND", Define.Sound.Effect);

        float timer = 0;
        Vector3 dir = DirectionToTarget(ps.transform.position);
        while (timer <= _duration)
        {
            Vector3 moveStep = dir * _speed * Time.deltaTime;
            hitbox.transform.position += moveStep;
            ps.transform.position += moveStep;

            timer += Time.deltaTime;

            if (hitbox.Penetration == 0)
            {
                // hit event를 여기서 실행시키면 됨
                // Stop Action -> Destroy
                ParticleSystem hitPs = Managers.Effect.Play(Define.Effect.Mummy_RangedHit, ps.transform);
                hitPs.transform.position = ps.transform.position;

                Managers.Resource.Destroy(hitbox.gameObject);
                Managers.Resource.Destroy(ps.gameObject);
                yield break;
            }

            yield return null;
        }
        Managers.Resource.Destroy(ps.gameObject);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
