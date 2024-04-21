package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.CreatePartyRequestDto;
import com.e207.back.dto.request.EnterPartyRequestDto;
import com.e207.back.dto.response.CreatePartyResponseDto;
import com.e207.back.dto.response.EnterPartyResponseDto;
import com.e207.back.entity.PartyEntity;
import com.e207.back.entity.PartyMemberEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.id.PartyMemberId;
import com.e207.back.repository.PartyMemberRepository;
import com.e207.back.repository.PartyRepository;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.PartyService;
import com.e207.back.util.CustomUserDetails;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PartyServiceImpl implements PartyService {

    private final PartyRepository partyRepository;
    private final PartyMemberRepository partyMemberRepository;
    private final PlayerRepository playerRepository;

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

    @Override
    public ResponseEntity<? super EnterPartyResponseDto> enterParty(EnterPartyRequestDto dto) {
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            PartyMemberEntity newPartyMember = new PartyMemberEntity();
            newPartyMember.setId(new PartyMemberId(playerId, dto.getPartyId()));


            Optional<PlayerEntity> player = playerRepository.findById(playerId);
            Optional<PartyEntity> party = partyRepository.findById(dto.getPartyId());

            newPartyMember.setParty(party.get());
            newPartyMember.setPlayer(player.get());
            partyMemberRepository.save(newPartyMember);



        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return EnterPartyResponseDto.success();
    }
}
