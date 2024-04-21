package com.e207.back.service;

import com.e207.back.dto.request.DungeonEndRequestDto;
import com.e207.back.dto.request.DungeonRankingRequestDto;
import com.e207.back.dto.response.DungeonEndResponseDto;
import com.e207.back.dto.response.DungeonRankingResponseDto;
import org.springframework.http.ResponseEntity;

public interface DungeonService {

    ResponseEntity<? super DungeonEndResponseDto> endDungeon(DungeonEndRequestDto dto);
    ResponseEntity<? super DungeonRankingResponseDto> dungeonRanking(DungeonRankingRequestDto dto);
}
