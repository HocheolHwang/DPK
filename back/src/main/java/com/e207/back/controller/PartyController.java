package com.e207.back.controller;

import com.e207.back.dto.request.CreatePartyRequestDto;
import com.e207.back.dto.request.EnterPartyRequestDto;
import com.e207.back.dto.response.CreatePartyResponseDto;
import com.e207.back.dto.response.EnterPartyResponseDto;
import com.e207.back.service.PartyService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/party")
public class PartyController {

    private final PartyService partyService;

    @PostMapping("")
    public ResponseEntity<? super CreatePartyResponseDto> createParty(@RequestBody CreatePartyRequestDto requestBody){
        return partyService.createParty(requestBody);
    }

    @PostMapping("/enter")
    public ResponseEntity<? super EnterPartyResponseDto> enterParty(@RequestBody EnterPartyRequestDto requestBody){
        return partyService.enterParty(requestBody);
    }


}
