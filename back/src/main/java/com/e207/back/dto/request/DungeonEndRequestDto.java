package com.e207.back.dto.request;


import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class DungeonEndRequestDto {

    @NotBlank
    private String dungeonCode;
    @NotBlank
    private String partyId;

    private boolean isCleared;

    private Long clearTime;
}
