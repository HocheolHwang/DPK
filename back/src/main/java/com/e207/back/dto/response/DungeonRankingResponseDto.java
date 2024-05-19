package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.DungeonRankingDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

import java.util.List;


@Getter
@Setter

public class DungeonRankingResponseDto extends ResponseDto {

    List<DungeonRankingDto> rankingList;
    DungeonRankingResponseDto(List<DungeonRankingDto> rankingList){
        this.rankingList = rankingList;
    }

    public static ResponseEntity<? super DungeonRankingResponseDto> success(List<DungeonRankingDto> rankingList){
        DungeonRankingResponseDto reponseBody = new DungeonRankingResponseDto(rankingList);
        return ResponseEntity.ok(reponseBody);
    }
}
