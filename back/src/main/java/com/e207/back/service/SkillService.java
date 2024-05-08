package com.e207.back.service;

import com.e207.back.dto.request.SaveLearnedSkillsRequestDto;
import com.e207.back.dto.response.SaveLearnedSkillsResponseDto;
import org.springframework.http.ResponseEntity;

public interface SkillService {

    ResponseEntity<? super SaveLearnedSkillsResponseDto> saveLearnedSkills(SaveLearnedSkillsRequestDto dto);
}
