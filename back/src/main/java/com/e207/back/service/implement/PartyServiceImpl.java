package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.CreatePartyRequestDto;
import com.e207.back.dto.response.CreatePartyResponseDto;
import com.e207.back.entity.PartyEntity;
import com.e207.back.repository.PartyRepository;
import com.e207.back.service.PartyService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class PartyServiceImpl implements PartyService {

    private final PartyRepository partyRepository;
    @Override
    public ResponseEntity<? super CreatePartyResponseDto> createParty(CreatePartyRequestDto dto) {

        try{
            PartyEntity newParty = new PartyEntity();
            System.out.println(dto.getPartyId());
            newParty.setPartyId(dto.getPartyId());
            newParty.setPartyTitle(dto.getPartyTitle());

            partyRepository.save(newParty);

        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return CreatePartyResponseDto.success();
    }
}
