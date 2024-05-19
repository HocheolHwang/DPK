using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillReqDto
{
	public string classCode;
	public List<SkillRequestDto> skillList;
}

[System.Serializable]
public class SkillRequestDto
{
	public int index;
	public string skillCode;
}
