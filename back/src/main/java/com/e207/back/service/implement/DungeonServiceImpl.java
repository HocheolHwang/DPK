package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.DungeonRankingDto;
import com.e207.back.dto.common.PlayerClassDto;
import com.e207.back.dto.request.DungeonEndRequestDto;
import com.e207.back.dto.request.DungeonRankingRequestDto;
import com.e207.back.dto.response.DungeonEndResponseDto;
import com.e207.back.dto.response.DungeonRankingResponseDto;
import com.e207.back.entity.DungeonEntity;
import com.e207.back.entity.DungeonLogEntity;
import com.e207.back.entity.PartyEntity;
import com.e207.back.entity.id.DungeonLogId;
import com.e207.back.repository.DungeonLogRepository;
import com.e207.back.repository.DungeonRepository;
import com.e207.back.repository.PartyRepository;
import com.e207.back.service.DungeonService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class DungeonServiceImpl implements DungeonService {

    private final DungeonRepository dungeonRepository;
    private final DungeonLogRepository dungeonLogRepository;
    private final PartyRepository partyRepository;
    @Override
    public ResponseEntity<? super DungeonEndResponseDto> endDungeon(DungeonEndRequestDto dto) {
        try{
            Optional<DungeonEntity> dungeon = dungeonRepository.findById(dto.getDungeonCode());
            Optional<PartyEntity> party = partyRepository.findById(dto.getPartyId());
            DungeonLogEntity log = new DungeonLogEntity();

            log.setId(new DungeonLogId(dto.getDungeonCode(), dto.getPartyId()));
            log.setDungeon(dungeon.get());
            log.setCleared(dto.isCleared());
            log.setParty(party.get());
            log.setClearTime(dto.getClearTime());

            dungeonLogRepository.save(log);

        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return DungeonEndResponseDto.success();
    }

    @Override
    public ResponseEntity<? super DungeonRankingResponseDto> dungeonRanking(DungeonRankingRequestDto dto) {
        List<DungeonRankingDto> list = new ArrayList<>();
        try{
            Optional<DungeonEntity> dungeon = dungeonRepository.findById(dto.getDungeonCode());
            List<DungeonLogEntity> entities = dungeonLogRepository.findTop3ByDungeonAndIsClearedTrueOrderByClearTimeAscCreatedAtAsc(dungeon.get());

            entities.forEach((e) -> {
                DungeonRankingDto ranking = new DungeonRankingDto();
                ranking.setClearTime(e.getClearTime());
                ranking.setCreatedAt(e.getCreatedAt());
                ranking.setPartyTitle(e.getParty().getPartyTitle());

                List<String> newPlayerList = new ArrayList<>();
                e.getParty().getPartyMembers().forEach((e2)->{
                    newPlayerList.add(e2.getPlayer().getPlayerNickname());
                });
                ranking.setPlayerList(newPlayerList);

                list.add(ranking);
            });


        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return DungeonRankingResponseDto.success(list);
    }
}
