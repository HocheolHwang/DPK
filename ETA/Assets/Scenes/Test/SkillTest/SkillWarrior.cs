using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillWarrior
{
    public class TestSkill1 : ISkill
    {
        public override void InitSetting()
        {
            data.name = "테스트 스킬1";
            data.damage = 1000;
            data.casting = 1.5f;
            data.animator = "Skill01_1";
            data.effect = Resources.Load<GameObject>("Assets/Scenes/Test/SkillTest/skilleffect.prefab");
        }

        public override void Using()
        {
            Debug.Log($"Skill name = {data.name}");
            // 스킬 이펙트 prefab을 로드하고 생성합니다.
            GameObject effectInstance = Instantiate(data.effect, transform.position, Quaternion.identity);
            effectInstance.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            effectInstance.transform.localScale *= 1f; // 현재 scale에 상대적인 scale 값으로 설정
        }
    }

    public class TestSkill2 : ISkill
    {
        public override void InitSetting()
        {
            data.name = "테스트 스킬2";
            data.casting = 1.5f;
            data.animator = "Skill02_1";
            data.effect = Resources.Load<GameObject>("Assets/Scenes/Test/SkillTest/skilleffect.prefab");
        }

        public override void Using()
        {
            Debug.Log($"Skill name = {data.name}");
            // 스킬 이펙트 prefab을 로드하고 생성합니다.
            GameObject effectInstance = Instantiate(data.effect, transform.position, Quaternion.identity);
            effectInstance.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            effectInstance.transform.localScale *= 1f; // 현재 scale에 상대적인 scale 값으로 설정
        }
    }
}
