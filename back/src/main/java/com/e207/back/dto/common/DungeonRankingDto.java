package com.e207.back.dto.common;

import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.List;


@Getter
@Setter
public class DungeonRankingDto {

    String partyTitle;
    List<String> playerList;
    Long clearTime;
    LocalDateTime createdAt;
}
