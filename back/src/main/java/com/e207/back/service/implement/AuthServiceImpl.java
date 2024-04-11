package com.e207.back.service.implement;

import com.e207.back.dto.request.SignInRequestDto;
import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SignInResponseDto;
import com.e207.back.dto.response.SignUpResponseDto;
import com.e207.back.service.AuthService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

@Service
public class AuthServiceImpl implements AuthService {

    @Override
    public ResponseEntity<? super SignUpResponseDto> signUp(SignUpRequestDto dto) {
        return null;
    }

    @Override
    public ResponseEntity<? super SignInResponseDto> signIn(SignInRequestDto dto) {
        return null;
    }
}
