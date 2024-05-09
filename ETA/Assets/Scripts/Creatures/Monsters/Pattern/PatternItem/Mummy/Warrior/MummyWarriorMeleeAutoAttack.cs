using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MummyWarriorMeleeAutoAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _forwardDivPos = 1.5f;
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 2.0f, 1.6f);
    [SerializeField] float _upPos = 1.0f;

    private Coroutine[] _coroutineList;
    private MummyManAnimationData _animData;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        _patternRange = _hitboxRange;

        _coroutineList = new Coroutine[2];
        _animData = _controller.GetComponent<MummyManAnimationData>();
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * (_controller.Detector.AttackRange / _forwardDivPos));
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upPos);
        Vector3 Pos = transform.position + rootForward + rootUp;

        // first attack
        yield return new WaitForSeconds(_createTime);
        _coroutineList[0] = StartCoroutine(CreateAutoAttack(AttackDamage, Pos));

        // second attack
        yield return new WaitForSeconds(_createTime * 2);
        _coroutineList[1] = StartCoroutine(CreateAutoAttack(AttackDamage, Pos));
    }

    IEnumerator CreateAutoAttack(int attackDMG, Vector3 Pos)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, attackDMG);
        hitbox.transform.localScale = _patternRange;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.position = Pos;
        Managers.Sound.Play("Sounds/Monster/Mummy/MummyAutoAttack_SND", Define.Sound.Effect);

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
