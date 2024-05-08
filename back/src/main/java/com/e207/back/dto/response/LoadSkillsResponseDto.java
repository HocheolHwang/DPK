package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.DungeonRankingDto;
import com.e207.back.dto.common.LoadedSkill;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

import java.util.List;

@Getter
@Setter
public class LoadSkillsResponseDto extends ResponseDto {

    List<LoadedSkill> skills;
    List<LoadedSkill> commonSkills;

    public LoadSkillsResponseDto(List<LoadedSkill> skills, List<LoadedSkill> commonSkills){
        this.skills = skills;
        this.commonSkills = commonSkills;
    }

    public static ResponseEntity<? super LoadSkillsResponseDto> success(List<LoadedSkill> skills, List<LoadedSkill> commonSkills){
        LoadSkillsResponseDto responseBody = new LoadSkillsResponseDto(skills, commonSkills);
        return ResponseEntity.ok(responseBody);
    }
}
