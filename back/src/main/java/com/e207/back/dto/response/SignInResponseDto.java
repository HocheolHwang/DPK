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
    boolean isFirst;
    long playerGold;
    SignInResponseDto(String accessToken, boolean isFirst, long playerGold){
        super();
        this.accessToken = accessToken;
        this.isFirst = isFirst;
        this.playerGold = playerGold;
    }

    public static ResponseEntity<? super SignInResponseDto> success(String accessToken, boolean isFirst, long playerGold) {
        SignInResponseDto responseBody = new SignInResponseDto(accessToken, isFirst, playerGold);
        return ResponseEntity.status(HttpStatus.OK).body(responseBody);
    }

    public static ResponseEntity<? super SignInResponseDto> playerPasswordValidationFail() {
        SignInResponseDto responseBody = new SignInResponseDto("", false, 0);
        responseBody.setMessage("비밀번호가 틀렸습니다.");
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
    }
}
