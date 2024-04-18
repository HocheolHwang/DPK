package com.e207.back.service;

import com.e207.back.dto.request.PlayerRankingRequestDto;
import com.e207.back.dto.response.PlayerRankingResponseDto;
import org.springframework.http.ResponseEntity;

public interface PlayerService {

    ResponseEntity<? super PlayerRankingResponseDto> getPlayerRanking(PlayerRankingRequestDto dto);
}
