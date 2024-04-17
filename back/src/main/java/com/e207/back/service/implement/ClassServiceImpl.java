package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.response.CurrentClassResponseDto;
import com.e207.back.entity.PlayerClassLogEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.repository.PlayerClassLogRepository;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.ClassService;
import com.e207.back.utill.CustomUserDetails;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Sort;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ClassServiceImpl implements ClassService {

    private final PlayerClassLogRepository playerClassLogRepository;
    private final PlayerRepository playerRepository;
    @Override
    public ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(CurrentClassRequestDto dto) {
        String classCode = null;
        try{
            Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
            CustomUserDetails customUserDetails = (CustomUserDetails) authentication.getPrincipal();

            String playerId = customUserDetails.getPlayerId();

            Optional<PlayerEntity> player = playerRepository.findById(playerId);
            Optional<PlayerClassLogEntity> playerClassLogEntity = playerClassLogRepository.findTop1ByPlayerOrderByCreatedAtDesc(player.get());

            classCode = playerClassLogEntity.get().getClassEntity().getClassCode();

        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return CurrentClassResponseDto.success(classCode);
    }
}
