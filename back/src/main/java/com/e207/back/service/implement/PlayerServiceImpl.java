package com.e207.back.service.implement;


import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.PlayerClassDto;
import com.e207.back.dto.request.PlayerRankingRequestDto;
import com.e207.back.dto.response.PlayerRankingResponseDto;
import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.repository.PlayerClassRepository;
import com.e207.back.service.PlayerService;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Slice;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class PlayerServiceImpl implements PlayerService {

    private final PlayerClassRepository playerClassRepository;
    @Override
    public ResponseEntity<? super PlayerRankingResponseDto> getPlayerRanking(PlayerRankingRequestDto dto) {
        List<PlayerClassDto> list = new ArrayList<>();
        try{
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


        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return PlayerRankingResponseDto.success(list);
    }
}
