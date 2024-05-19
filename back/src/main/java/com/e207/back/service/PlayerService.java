package com.e207.back.service;

import com.e207.back.dto.request.ChangeExpRequestDto;
import com.e207.back.dto.request.ChangeGoldRequestDto;
import com.e207.back.dto.request.PlayerRankingRequestDto;
import com.e207.back.dto.response.ChangeExpResponseDto;
import com.e207.back.dto.response.ChangeGoldResponseDto;
import com.e207.back.dto.response.PlayerRankingResponseDto;
import org.springframework.http.ResponseEntity;

public interface PlayerService {

    ResponseEntity<? super PlayerRankingResponseDto> getPlayerRanking(PlayerRankingRequestDto dto);
    ResponseEntity<? super ChangeGoldResponseDto> changeGold(ChangeGoldRequestDto dto);
    ResponseEntity<? super ChangeExpResponseDto> changeExp(ChangeExpRequestDto dto);
}
