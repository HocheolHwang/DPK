package com.e207.back.dto.request;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class ChangeGoldRequestDto {

    private int goldDelta;

    @NotBlank
    private String reason;
}
