package com.e207.back.service;

import com.e207.back.dto.request.CreatePartyRequestDto;
import com.e207.back.dto.request.EnterPartyRequestDto;
import com.e207.back.dto.response.CreatePartyResponseDto;
import com.e207.back.dto.response.EnterPartyResponseDto;
import org.springframework.http.ResponseEntity;

public interface PartyService {

    ResponseEntity<? super CreatePartyResponseDto> createParty(CreatePartyRequestDto dto);
    ResponseEntity<? super EnterPartyResponseDto> enterParty(EnterPartyRequestDto dto);
}
