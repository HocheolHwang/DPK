package com.e207.back.dto.request;


import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class CreatePartyRequestDto {

    @NotBlank
    private String partyId;
    @NotBlank
    private String partyTitle;

}
