package com.e207.back.service;

import com.e207.back.dto.request.CreatePartyRequestDto;
import com.e207.back.dto.response.CreatePartyResponseDto;
import org.springframework.http.ResponseEntity;

public interface PartyService {

    ResponseEntity<? super CreatePartyResponseDto> createParty(CreatePartyRequestDto dto);
}
