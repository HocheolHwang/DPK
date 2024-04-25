using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class SkillInfo
{
    public string SkillCode { get; set; }
    public string SkillName { get; set; }
    public string SkillDescription { get; set; }

    public int RequiredSkillPoint { get; set; }
    public int RequiredLevel { get; set; }

    public bool CanLearned { get; set; }
    public bool IsLearned { get; set; }

    public string PrerequisiteSkillCode { get; set; }

}

public class SkillTree : UI_Fixed
{

    Dictionary<string, SkillInfo> _skills = new Dictionary<string, SkillInfo>();
    
    public override void Init()
    {
        base.Init();
        // Data Load
        // 데이터를 어떻게 하지?
       
        
    }


    public void InitSkillInfo() // 단 한번만 불러오게 한다.(방법은 몰루) vs UI 뜰 때 마다 불러온다.
    {
        // 기본적으로 폴더에 있는 ScriptableObject를 모두 불러와서
        // 딕셔너리에 다 저장하자.
        // 저장할 때 기본적으로 레벨이 넘는지 안넘는지 확인해서 배울수 있는가? 를 체크 해주고 CanLearned를 설정
        // 그리고 정보들을 다 _skills에 저장을 해버린다.
        // => 선행스킬도 미리 ScriptableObject에 적어두고 여기서 저장해주자.
    }

    public void LoadLearnedSkil()
    {
        // DB에 배운 스킬 정보 요청
        // 가져온 스킬 전부 IsLearned = true를 해준다.
    }

    public void UpdateSkillIconUI()
    {
        // 여기서 모든 SkillIconUI를 돌면서 상태에 맞게 아이콘을 바꿔준다.
    }

    // 한번에 모든 스킬을 업데이트 해야할지
    // 직업에 맞는 스킬들만 UI를 건드려 줘야할지는 모르겠음
    // SetActive로 하면 좋을거 같은데 이부분은 Tab 구현이니까

    // 어떤 탭을 클릭함.
    // 그 탭 관련 스킬트리 창을 Active하고 나머진 InActive 시킴
    // 스킬트리 창의 UI들은 기본적으로 안배운 상태
    // 모든 스킬을 찾아봄
    // 그중에 특정 직업 스킬만 처리함
    // 현재 정보에 맞게 UI 업데이트

    // 스킬 코드 만으로 특정 UI에 접근할 방법이 있나?
    // enum 전체를 돌면서 GameObject.name을 이용해서 Dictionary에서 스킬정보를 가져오고
    // 스킬 정보를 토대로 UI를 업데이트하기
    // 근데 enum은 직업별로 나눈면 좋을수도있겠다.
    


    // 현재 직업 정보를 받아서 처음에 어떤 화면을 보여줄지 정해야함



}
