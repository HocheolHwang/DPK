package com.e207.back.dto.response;


import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;


@Getter
@Setter
public class SignInResponseDto extends ResponseDto {
    String accessToken;
    SignInResponseDto(String accessToken){
        super();
        this.accessToken = accessToken;
    }

    public static ResponseEntity<? super SignInResponseDto> success(String accessToken) {
        SignInResponseDto responseBody = new SignInResponseDto(accessToken);
        return ResponseEntity.status(HttpStatus.OK).body(responseBody);
    }

    public static ResponseEntity<? super SignInResponseDto> playerPasswordValidationFail() {
        SignInResponseDto responseBody = new SignInResponseDto("");
        responseBody.setMessage("비밀번호가 틀렸습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
}
