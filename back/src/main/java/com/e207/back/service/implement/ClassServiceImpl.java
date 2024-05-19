package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.ClassDto;
import com.e207.back.dto.request.AllClassRequestDto;
import com.e207.back.dto.request.CurrentClassRequestDto;
import com.e207.back.dto.request.SelectClassRequestDto;
import com.e207.back.dto.response.AllClassResponseDto;
import com.e207.back.dto.response.CurrentClassResponseDto;
import com.e207.back.dto.response.SelectClassResponseDto;
import com.e207.back.entity.ClassEntity;
import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.PlayerClassLogEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.id.PlayerClassId;
import com.e207.back.repository.ClassRepository;
import com.e207.back.repository.PlayerClassLogRepository;
import com.e207.back.repository.PlayerClassRepository;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.ClassService;
import com.e207.back.util.CustomUserDetails;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ClassServiceImpl implements ClassService {

    private final PlayerClassLogRepository playerClassLogRepository;
    private final PlayerClassRepository playerClassRepository;
    private final PlayerRepository playerRepository;
    private final ClassRepository classRepository;
    @Override
    public ResponseEntity<? super CurrentClassResponseDto> getCurrentClass(CurrentClassRequestDto dto) {
        String classCode = null;
        Long currentExp = null;
        int currentLevel = 0;
        int skillPoint = 0;
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();

            Optional<PlayerEntity> player = playerRepository.findById(playerId);
            Optional<PlayerClassLogEntity> playerClassLogEntity = playerClassLogRepository.findTop1ByPlayerOrderByCreatedAtDesc(player.get());

            classCode = playerClassLogEntity.get().getClassEntity().getClassCode();

            PlayerClassId id = new PlayerClassId(playerId, classCode);
            Optional<PlayerClassEntity> playerClass = playerClassRepository.findById(id);

            currentExp = playerClass.get().getPlayerExp();
            currentLevel = playerClass.get().getPlayerLevel();
            skillPoint = playerClass.get().getSkillPoint();

        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return CurrentClassResponseDto.success(classCode, currentExp, currentLevel, skillPoint);
    }

    @Override
    public ResponseEntity<? super SelectClassResponseDto> selectClass(SelectClassRequestDto dto) {
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            PlayerClassLogEntity log = new PlayerClassLogEntity();
            System.out.println(dto.getClassCode());
            Optional<ClassEntity> classEntity = classRepository.findById(dto.getClassCode());
            Optional<PlayerEntity> player = playerRepository.findById(customUserDetails.getPlayerId());


            if(classEntity.isEmpty()){
                return SelectClassResponseDto.classNotFound(); // 없는 직업
            }
            if(player.isEmpty()){
                return ResponseDto.databaseError(); // 없는 유저
            }

            log.setClassEntity(classEntity.get());
            log.setPlayer(player.get());
            playerClassLogRepository.save(log);


        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return SelectClassResponseDto.success();
    }

    @Override
    public ResponseEntity<? super AllClassResponseDto> getAllClass(AllClassRequestDto dto) {
        List<ClassDto> list = new ArrayList<>();
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            List<PlayerClassEntity> entities = playerClassRepository.findByPlayerPlayerId(playerId);
            entities.forEach((e) ->{
                ClassDto tmp = new ClassDto();
                tmp.setClassCode(e.get_class().getClassCode());
                tmp.setLevel(e.getPlayerLevel());
                list.add(tmp);
            });
        }catch(Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return AllClassResponseDto.success(list);
    }
}
