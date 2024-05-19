package com.e207.back.dto.common;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.List;


@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class DungeonRankingDto {

    String partyTitle;
    List<String> playerList;
    Long clearTime;
    LocalDateTime createdAt;
}
