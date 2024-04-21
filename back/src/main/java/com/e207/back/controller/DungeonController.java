package com.e207.back.controller;

import com.e207.back.dto.request.DungeonEndRequestDto;
import com.e207.back.dto.request.DungeonRankingRequestDto;
import com.e207.back.dto.response.DungeonEndResponseDto;
import com.e207.back.dto.response.DungeonRankingResponseDto;
import com.e207.back.service.DungeonService;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/dungeon")
public class DungeonController {

    private final DungeonService dungeonService;
    @PostMapping("/end")
    public ResponseEntity<? super DungeonEndResponseDto> endDungeon(@RequestBody DungeonEndRequestDto requestBody){
        return dungeonService.endDungeon(requestBody);
    }

    @GetMapping("/ranking")
    public ResponseEntity<? super DungeonRankingResponseDto> dungeonRanking(@RequestParam(required = true,name = "dungeon-code") String dungeonCode){
        DungeonRankingRequestDto requestBody = new DungeonRankingRequestDto();
        requestBody.setDungeonCode(dungeonCode);
        return dungeonService.dungeonRanking(requestBody);
    }
}
