package com.e207.back.service;

import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SignUpResponseDto;
import org.springframework.http.ResponseEntity;

public interface AuthService {
    ResponseEntity<? super SignUpResponseDto> signUp(SignUpRequestDto dto);
}
