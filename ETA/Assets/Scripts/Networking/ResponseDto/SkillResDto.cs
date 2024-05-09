using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillResDto
{
	public string message;
	public List<SkillResponseDto> skills;
	public List<SkillResponseDto> commonSkills;
}

[System.Serializable]
public class SkillResponseDto
{
	public int index;
	public string classCode;
	public string skillCode;
	public string skillName;
}