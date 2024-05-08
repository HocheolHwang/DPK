package com.e207.back.dto.request;


import com.e207.back.dto.common.SavedSkill;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@Getter
@Setter
@NoArgsConstructor
public class SaveLearnedSkillsRequestDto {
    String classCode;
    List<SavedSkill> skillList;
}
