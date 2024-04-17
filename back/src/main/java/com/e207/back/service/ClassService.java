package com.e207.back.service;

import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.response.CurrentClassResponseDto;

import org.springframework.http.ResponseEntity;

public interface ClassService {
    ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(CurrentClassRequestDto dto);
}
