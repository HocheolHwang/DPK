using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class KnightGTwoSkillAttack : Pattern
{
    [Header("개발 편의성")]
    [SerializeField] float _hitboxForwardLoc;
    [SerializeField] float _hitboxRadius = 2.6f;

    private Coroutine[] _coroutineList;

    public override void Init()
    {
        base.Init();

        _createTime = 0.47f;
        _hitboxForwardLoc = _controller.Detector.AttackRange;
        
        _coroutineList = new Coroutine[6];
        _patternDmg = 110;
    }

    public override IEnumerator StartPatternCast()
    {
        // ps 위치
        Vector3 loc = SetLocPS(transform, _hitboxForwardLoc);

        // first
        yield return new WaitForSeconds(_createTime);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.KnightG_TwoSkillAttack, 0, _controller.transform);
        ps.transform.position = loc;

        Vector3 locMidLeft = new Vector3(-0.7f, 0, 1.2f);
        Vector3 locMidright = new Vector3(0.7f, 0, 1.2f);
        Vector3 locLastLeft = new Vector3(-1.2f, 0, 2.4f);
        Vector3 locLastMid = new Vector3(0, 0, 2.4f);
        Vector3 locLastRight = new Vector3(1.2f, 0, 2.4f);

        _coroutineList[0] = StartCoroutine(CreateHitbox(ps.transform, Vector3.zero));
        
        // middle
        yield return new WaitForSeconds(0.5f);
        _coroutineList[1] = StartCoroutine(CreateHitbox(ps.transform, locMidLeft));
        _coroutineList[2] = StartCoroutine(CreateHitbox(ps.transform, locMidright));

        // last
        yield return new WaitForSeconds(0.5f);
        _coroutineList[3] = StartCoroutine(CreateHitbox(ps.transform, locLastLeft));
        _coroutineList[4] = StartCoroutine(CreateHitbox(ps.transform, locLastMid));
        _coroutineList[5] = StartCoroutine(CreateHitbox(ps.transform, locLastRight));

        //yield return new WaitForSeconds(ps.main.duration);
        //Managers.Resource.Destroy(ps.gameObject);

    }

    private Vector3 SetLocPS(Transform parent, float forwardLoc, float upLoc = 1.0f)
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * upLoc);
        Vector3 rootForward = transform.TransformDirection(Vector3.forward * forwardLoc);

        return parent.position + rootForward + rootUp;
    }

    IEnumerator CreateHitbox(Transform parent, Vector3 objectLoc)
    {
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxCircle").GetComponent<HitBox>();
        hitbox.SetUp(transform, _patternDmg);
        hitbox.GetComponent<SphereCollider>().radius = _hitboxRadius;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.SetParent(parent);
        hitbox.transform.localPosition = objectLoc;

        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
