package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.PlayerClassDto;
import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.PlayerClassLogEntity;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

import java.util.ArrayList;
import java.util.List;


@Getter
@Setter
public class PlayerRankingResponseDto extends ResponseDto {

    List<PlayerClassDto> rankingList;
    int personalRanking;
    PlayerRankingResponseDto(List<PlayerClassDto> rankingList, int personalRanking){
        super();
        this.rankingList = rankingList;
        this.personalRanking = personalRanking;
    }

    public static ResponseEntity<? super PlayerRankingResponseDto> success(List<PlayerClassDto> rankingList, int personalRanking){
        PlayerRankingResponseDto responseBody = new PlayerRankingResponseDto(rankingList, personalRanking);
        return ResponseEntity.ok(responseBody);
    }
}
