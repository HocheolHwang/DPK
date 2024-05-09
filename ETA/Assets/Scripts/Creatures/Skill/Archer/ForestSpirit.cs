using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpirit : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(10);
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(3, 3, 3);
        //RangeType = Define.RangeType.Square;
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ForestSpirit.png");
    }
    public override IEnumerator StartSkillCast()
    {
        // 소환 시작 
        _animator.CrossFade("SKILL6", 0.1f);
        //ParticleSystem ps01 = Managers.Resource.Instantiate("Effect/ForestSpiritSpawn").GetComponent<ParticleSystem>();
        //ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        //ps01.transform.position = transform.position;
        ////ps01.Play();
        //yield return new WaitForSeconds(0.2f);

        //Managers.Sound.Play("Skill/ForestSpiritSpawn");

        //yield return new WaitForSeconds(0.7f);

        //// 지정 위치에 소환
        //Managers.Sound.Play("Skill/ForestSpiritSpawn");
        ////ParticleSystem ps02 = Managers.Resource.Instantiate("Effect/ForestSpiritSpawn").GetComponent<ParticleSystem>();
        //ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        //ps02.transform.position = _skillSystem.TargetPosition;
        ////ps02.Play();
        ///
        Managers.Coroutine.Run(ForestSpiritCoroutine());

        Vector3 spawnPosition = _skillSystem.TargetPosition;
        //GameObject forestSpirit = Resources.Load<GameObject>("Prefabs/Creatures/Player/ForestSpirit");
        PhotonNetwork.Instantiate("Prefabs/Creatures/Player/ForestSpirit", spawnPosition, Quaternion.identity);
        //GameObject dummy = Instantiate(forestSpirit, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        

        yield return new WaitForSeconds(0.8f);
        //Managers.Resource.Destroy(ps01.gameObject);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    IEnumerator ForestSpiritCoroutine()
    {
        ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        ps01.transform.position = transform.position;
        //ps01.Play();
        yield return new WaitForSeconds(0.2f);

        Managers.Sound.Play("Skill/ForestSpiritSpawn");

        yield return new WaitForSeconds(0.7f);

        // 지정 위치에 소환
        Managers.Sound.Play("Skill/ForestSpiritSpawn");
        //ParticleSystem ps02 = Managers.Resource.Instantiate("Effect/ForestSpiritSpawn").GetComponent<ParticleSystem>();
        ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        ps02.transform.position = _skillSystem.TargetPosition;
        //ps02.Play();

    }
}
