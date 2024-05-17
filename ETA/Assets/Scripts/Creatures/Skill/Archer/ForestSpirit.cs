using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpirit : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(8);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
    }

    public override IEnumerator StartSkillCast()
    {
        // 소환 시작 
        _animator.CrossFade("SKILL6", 0.1f);
        
        Managers.Coroutine.Run(ForestSpiritCoroutine());

        //Vector3 spawnPosition = _skillSystem.TargetPosition;
        
        //if(PhotonNetwork.IsMasterClient) PhotonNetwork.Instantiate("Prefabs/Creatures/Player/ForestSpirit", spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
        
        yield return new WaitForSeconds(0.8f);

        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    IEnumerator ForestSpiritCoroutine()
    {   
        ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        ps01.transform.position = transform.position;
        //ps01.Play();
        yield return new WaitForSeconds(0.2f);

        Managers.Sound.Play("Skill/ForestSpiritSpawn");

        yield return new WaitForSeconds(0.7f);

        Vector3 spawnPosition = _skillSystem.TargetPosition;

        if (PhotonNetwork.IsMasterClient) PhotonNetwork.Instantiate("Prefabs/Creatures/Player/ForestSpirit", spawnPosition, Quaternion.identity);
            

        // 지정 위치에 소환
        Managers.Sound.Play("Skill/ForestSpiritSpawn");

        ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.ForestSpiritSpawn, 1.0f, gameObject.transform);
        ps02.transform.position = _skillSystem.TargetPosition;

    }
}
