package com.e207.back.service.implement;


import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.PlayerClassDto;
import com.e207.back.dto.request.ChangeExpRequestDto;
import com.e207.back.dto.request.ChangeGoldRequestDto;
import com.e207.back.dto.request.PlayerRankingRequestDto;
import com.e207.back.dto.response.ChangeExpResponseDto;
import com.e207.back.dto.response.ChangeGoldResponseDto;
import com.e207.back.dto.response.PlayerRankingResponseDto;
import com.e207.back.entity.ExpLogEntity;
import com.e207.back.entity.GoldLogEntity;
import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.id.PlayerClassId;
import com.e207.back.repository.ExpLogRepository;
import com.e207.back.repository.GoldLogRepository;
import com.e207.back.repository.PlayerClassRepository;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.PlayerService;
import com.e207.back.util.CustomUserDetails;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Slice;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class PlayerServiceImpl implements PlayerService {

    private final PlayerRepository playerRepository;
    private final PlayerClassRepository playerClassRepository;
    private final GoldLogRepository goldLogRepository;
    private final ExpLogRepository expLogRepository;

    @Override
    public ResponseEntity<? super PlayerRankingResponseDto> getPlayerRanking(PlayerRankingRequestDto dto) {
        List<PlayerClassDto> list = new ArrayList<>();
        int higherLevelCount = -1;
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            // 0번째 페이지 limit만큼 들고오기
            PageRequest pageRequest = PageRequest.of(0, dto.getLimit());
            Slice<PlayerClassEntity> playerClassEntities = playerClassRepository.findByOrderByPlayerLevelDesc(pageRequest);

            playerClassEntities.forEach((entity) ->{
                PlayerClassDto playerClass = new PlayerClassDto(entity.get_class().getClassName(),
                        entity.getPlayer().getPlayerNickname(),
                        entity.getPlayerLevel()
                );

                list.add(playerClass);
            });

            PlayerClassEntity entity = playerClassRepository.findTopByPlayerPlayerIdOrderByPlayerLevelDesc(playerId);
            String highestClassCode = entity.get_class().getClassCode();
            System.out.println(highestClassCode);
            higherLevelCount = playerClassRepository.countByClassCodeAndPlayerLevelGreaterThan(entity.getPlayerLevel());
            System.out.println(higherLevelCount);

        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return PlayerRankingResponseDto.success(list, higherLevelCount + 1);
    }

    @Override
    public ResponseEntity<? super ChangeGoldResponseDto> changeGold(ChangeGoldRequestDto dto) {

        long currentGold = -1;
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            Optional<PlayerEntity> player = playerRepository.findById(playerId);
            currentGold = player.get().getPlayerGold() + dto.getGoldDelta();

            player.get().setPlayerGold(currentGold);
            playerRepository.save(player.get());

            GoldLogEntity log = new GoldLogEntity();
            log.setPlayer(player.get());
            log.setGoldDelta(dto.getGoldDelta());
            log.setGoldLogReason(dto.getReason());
            goldLogRepository.save(log);


        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();

        }
        return ChangeGoldResponseDto.success(currentGold);
    }

    @Override
    public ResponseEntity<? super ChangeExpResponseDto> changeExp(ChangeExpRequestDto dto) {

        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();

            Optional<PlayerClassEntity> playerClassEntity = playerClassRepository.findById(new PlayerClassId(playerId, dto.getClassCode()));

            playerClassEntity.get().setPlayerExp(dto.getCurrentExp());
            playerClassEntity.get().setPlayerLevel(dto.getPlayerLevel());

            playerClassRepository.save(playerClassEntity.get());

            ExpLogEntity log = new ExpLogEntity();
            log.setPlayer(playerClassEntity.get().getPlayer());
            log.setClassEntity(playerClassEntity.get().get_class());
            log.setExpLogReason(dto.getReason());
            log.setCurrentLevel(dto.getPlayerLevel());
            log.setExpDelta(dto.getExpDelta());
            expLogRepository.save(log);



        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return ChangeExpResponseDto.success();
    }
}
