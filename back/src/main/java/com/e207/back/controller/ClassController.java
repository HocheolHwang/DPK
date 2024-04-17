package com.e207.back.controller;

import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.response.CurrentClassResponseDto;
import com.e207.back.service.ClassService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1")
@RequiredArgsConstructor
public class ClassController {


    private final ClassService classService;
    @GetMapping("/class/current")
    public ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(){
        CurrentClassRequestDto requestBody = new CurrentClassRequestDto();
        return classService.getCurrentClass(requestBody);
    }
}
