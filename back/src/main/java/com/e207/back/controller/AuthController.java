package com.e207.back.controller;

import com.e207.back.dto.request.SignInRequestDto;
import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SignInResponseDto;
import com.e207.back.dto.response.SignUpResponseDto;
import com.e207.back.service.AuthService;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1/auth")
@RequiredArgsConstructor
public class AuthController {

    private final AuthService authService;

    @PostMapping("/sign-up")
    public ResponseEntity<? super SignUpResponseDto> signUp(@RequestBody @Valid SignUpRequestDto requestBody){
        return null;
    }

    @PostMapping("/sign-in")
    public ResponseEntity<? super SignInResponseDto> signUp(@RequestBody @Valid SignInRequestDto requestBody){
        return null;
    }


}
