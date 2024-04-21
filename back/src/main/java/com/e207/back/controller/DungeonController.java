package com.e207.back.controller;

import com.e207.back.dto.request.DungeonEndRequestDto;
import com.e207.back.dto.response.DungeonEndResponseDto;
import com.e207.back.service.DungeonService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/dungeon")
public class DungeonController {

    private final DungeonService dungeonService;
    @PostMapping("/end")
    public ResponseEntity<? super DungeonEndResponseDto> endDungeon(@RequestBody DungeonEndRequestDto requestBody){
        return dungeonService.endDungeon(requestBody);
    }
}
