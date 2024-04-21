package com.e207.back.service;

import com.e207.back.dto.request.DungeonEndRequestDto;
import com.e207.back.dto.response.DungeonEndResponseDto;
import org.springframework.http.ResponseEntity;

public interface DungeonService {

    ResponseEntity<? super DungeonEndResponseDto> endDungeon(DungeonEndRequestDto dto);
}
