package com.e207.back.dto.response;


import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

@Getter
public class SignUpResponseDto extends ResponseDto {

    public static ResponseEntity<? super SignUpResponseDto> duplicateId() {
        SignUpResponseDto responseBody = new SignUpResponseDto();
        responseBody.setMessage("중복된 아이디 입니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
    public static ResponseEntity<? super SignUpResponseDto> playerIdValidationFail() {
        SignUpResponseDto responseBody = new SignUpResponseDto();
        responseBody.setMessage("아이디 형식이 틀렸습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
    public static ResponseEntity<? super SignUpResponseDto> playerNickNameValidationFail() {
        SignUpResponseDto responseBody = new SignUpResponseDto();
        responseBody.setMessage("닉네임 형식이 틀렸습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
    public static ResponseEntity<? super SignUpResponseDto> playerPasswordValidationFail() {
        SignUpResponseDto responseBody = new SignUpResponseDto();
        responseBody.setMessage("비밀번호 형식이 틀렸습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }

    public static ResponseEntity<? super SignUpResponseDto> playerPasswordCheckValidationFail() {
        SignUpResponseDto responseBody = new SignUpResponseDto();
        responseBody.setMessage("비밀번호가 일치하지 않습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
}
