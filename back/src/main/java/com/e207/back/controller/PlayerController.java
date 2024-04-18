package com.e207.back.controller;

import com.e207.back.dto.request.PlayerRankingRequestDto;
import com.e207.back.dto.response.PlayerRankingResponseDto;
import com.e207.back.service.PlayerService;
import jakarta.websocket.server.PathParam;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1/player")
@RequiredArgsConstructor
public class PlayerController {
    private final PlayerService playerService;

    @GetMapping("/ranking")
    public ResponseEntity<? super PlayerRankingResponseDto> getPlayerRanking(@RequestParam(required = false, defaultValue ="10") int limit){
        PlayerRankingRequestDto requestBody = new PlayerRankingRequestDto();
        requestBody.setLimit(limit);
        return playerService.getPlayerRanking(requestBody);
    }
}
