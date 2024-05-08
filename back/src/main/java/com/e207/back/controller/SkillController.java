package com.e207.back.controller;

import com.e207.back.dto.request.SaveLearnedSkillsRequestDto;
import com.e207.back.dto.response.SaveLearnedSkillsResponseDto;
import com.e207.back.service.SkillService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/v1/skill")
@RequiredArgsConstructor
public class SkillController {

    private final SkillService skillService;
    @PutMapping("/learned")
    public ResponseEntity<? super SaveLearnedSkillsResponseDto> SaveLearnedSkills(@RequestBody SaveLearnedSkillsRequestDto requestBody){
        return skillService.saveLearnedSkills(requestBody);
    }
}
