package com.e207.back.dto.request;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class SignUpRequestDto {
    @NotBlank
    private String playerId;
    @NotBlank
    private String nickname;
    @NotBlank
    private String userPassword;
}
