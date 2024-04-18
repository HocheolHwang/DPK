package com.e207.back.service;

import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.request.SelectClassRequestDto;
import com.e207.back.dto.response.CurrentClassResponseDto;

import com.e207.back.dto.response.SelectClassResponseDto;
import org.springframework.http.ResponseEntity;

public interface ClassService {
    ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(CurrentClassRequestDto dto);
    ResponseEntity<? super SelectClassResponseDto> selectClass(SelectClassRequestDto dto);
}
