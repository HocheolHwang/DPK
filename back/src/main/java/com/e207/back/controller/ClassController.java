package com.e207.back.controller;

import com.e207.back.dto.request.AllClassRequestDto;
import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.request.SelectClassRequestDto;
import com.e207.back.dto.response.AllClassResponseDto;
import com.e207.back.dto.response.CurrentClassResponseDto;
import com.e207.back.dto.response.SelectClassResponseDto;
import com.e207.back.service.ClassService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/v1/class")
@RequiredArgsConstructor
public class ClassController {


    private final ClassService classService;
    @GetMapping("/current")
    public ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(){
        CurrentClassRequestDto requestBody = new CurrentClassRequestDto();
        return classService.getCurrentClass(requestBody);
    }

    @PostMapping("/select")
    public ResponseEntity<? super SelectClassResponseDto> selectClass(@RequestBody SelectClassRequestDto requestBody){
        return classService.selectClass(requestBody);
    }

    @GetMapping("")
    public ResponseEntity<? super AllClassResponseDto> getAllClass(){
        AllClassRequestDto requestBody = new AllClassRequestDto();
        return classService.getAllClass(requestBody);
    }
}
