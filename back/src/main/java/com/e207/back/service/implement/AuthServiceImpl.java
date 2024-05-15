package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.SignInRequestDto;
import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SelectClassResponseDto;
import com.e207.back.dto.response.SignInResponseDto;
import com.e207.back.dto.response.SignUpResponseDto;
import com.e207.back.entity.*;
import com.e207.back.entity.id.LearnedSkillId;
import com.e207.back.entity.id.PlayerClassId;
import com.e207.back.provider.JwtProvider;
import com.e207.back.repository.*;
import com.e207.back.service.AuthService;
import com.e207.back.util.ValidationUtil;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.temporal.ChronoUnit;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class AuthServiceImpl implements AuthService {

    private final PlayerRepository playerRepository;
    private final ClassRepository classRepository;
    private final PlayerClassRepository playerClassRepository;
    private final PlayerClassLogRepository playerClassLogRepository;
    private final SkillRepository skillRepository;
    private final LearnedSkillRepository learnedSkillRepository;
    private final BCryptPasswordEncoder bCryptPasswordEncoder;
    private final JwtProvider jwtProvider;
    @Override
    @Transactional
    public ResponseEntity<? super SignUpResponseDto> signUp(SignUpRequestDto dto) {
        try{

            PlayerEntity newPlayer = new PlayerEntity();
            newPlayer.setPlayerId(dto.getPlayerId());
            newPlayer.setPassword(bCryptPasswordEncoder.encode(dto.getPlayerPassword()));
            newPlayer.setPlayerNickname(dto.getNickname());

            Optional<PlayerEntity> oldPlayer = playerRepository.findById(dto.getPlayerId());

            if(oldPlayer.isPresent()){
                return SignUpResponseDto.duplicateId();
            }

            Optional<PlayerEntity> sameNicknamePlayer = playerRepository.findByPlayerNickname(newPlayer.getPlayerNickname());

            if(sameNicknamePlayer.isPresent()){
                return SignUpResponseDto.duplicateNickname();
            }

            if(!ValidationUtil.isValidPlayerId(dto.getPlayerId())){
                return SignUpResponseDto.playerIdValidationFail();
            }
            if(!ValidationUtil.isValidNickname(dto.getNickname())){
                return SignUpResponseDto.playerNickNameValidationFail();
            }
            if(!ValidationUtil.isValidPassword(dto.getPlayerPassword())){
                return SignUpResponseDto.playerPasswordValidationFail();
            }
            if(!(dto.getPlayerPassword().equals(dto.getPlayerPasswordCheck()))){
                return SignUpResponseDto.playerPasswordCheckValidationFail();
            }

            playerRepository.save(newPlayer);
            // 캐릭터 3개 생성
            List<ClassEntity> classList = classRepository.findAll();

            classList.forEach((e) ->{
                PlayerClassEntity newPlayerClass = new PlayerClassEntity();

                PlayerClassId id = new PlayerClassId(dto.getPlayerId(), e.getClassCode());
                newPlayerClass.setPlayerLevel(1);
                newPlayerClass.setPlayerExp(0);
                newPlayerClass.setSkillPoint(0);
                newPlayerClass.set_class(e);
                newPlayerClass.setPlayer(newPlayer);
                newPlayerClass.setId(id);
                playerClassRepository.save(newPlayerClass);
            });

            // 워리어 캐릭터 선택
            PlayerClassLogEntity log = new PlayerClassLogEntity();
            Optional<ClassEntity> classEntity = classRepository.findById("C001");


            if(classEntity.isEmpty()){
                return ResponseDto.databaseError();
            }

            log.setClassEntity(classEntity.get());
            log.setPlayer(newPlayer);
            playerClassLogRepository.save(log);


            /// 스킬 생성

            List<SkillEntity> defaultSkills = skillRepository.findAllByRequiredLevel(1);
            int wCnt = 0;
            int aCnt = 0;
            int mCnt = 0;

            for(int i = 0; i < defaultSkills.size(); i++){

                SkillEntity skill = defaultSkills.get(i);
                System.out.println();
                LearnedSkillEntity newLearnedSkill = new LearnedSkillEntity();
                newLearnedSkill.setPlayer(newPlayer);
                newLearnedSkill.setActive(true);
                newLearnedSkill.setSkill(skill);
                for(int j = 0; j < classList.size(); j++){
                    ClassEntity c = classList.get(j);
                    System.out.println(c.getClassCode() + " " + skill.getSkillCode());
                    if(c.getClassCode().equals("C001") && skill.getSkillCode().startsWith("W")){
                        LearnedSkillId learnedSkillId = new LearnedSkillId(newPlayer.getPlayerId(), skill.getSkillCode(), c.getClassCode());
                        newLearnedSkill.setId(learnedSkillId);
                        newLearnedSkill.setSkillSlot(wCnt++);
                        newLearnedSkill.setClassEntity(c);
                        System.out.println("Warrior SKill Save" + skill.getSkillCode());
                    } else if (c.getClassCode().equals("C002") && skill.getSkillCode().startsWith("A")) {
                        LearnedSkillId learnedSkillId = new LearnedSkillId(newPlayer.getPlayerId(), skill.getSkillCode(), c.getClassCode());
                        newLearnedSkill.setId(learnedSkillId);
                        newLearnedSkill.setSkillSlot(aCnt++);
                        newLearnedSkill.setClassEntity(c);
                    } else if (c.getClassCode().equals("C003") && skill.getSkillCode().startsWith("M")){
                        LearnedSkillId learnedSkillId = new LearnedSkillId(newPlayer.getPlayerId(), skill.getSkillCode(), c.getClassCode());
                        newLearnedSkill.setId(learnedSkillId);
                        newLearnedSkill.setSkillSlot(mCnt++);
                        newLearnedSkill.setClassEntity(c);
                    }
                }
                learnedSkillRepository.save(newLearnedSkill);
            }




        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return SignUpResponseDto.success();
    }

    @Override
    public ResponseEntity<? super SignInResponseDto> signIn(SignInRequestDto dto) {
        String accessToken = null;
        boolean isFirst;
        long playerGold;
        try{
            Optional<PlayerEntity> player = playerRepository.findById(dto.getPlayerId());
            isFirst = player.get().isFirst();
            if(!bCryptPasswordEncoder.matches(dto.getPlayerPassword(), player.get().getPassword())){
                return SignInResponseDto.playerPasswordValidationFail();
            }

            if(isFirst){
                player.get().setFirst(false);
                playerRepository.save(player.get());
            }

            accessToken = jwtProvider.createToken(player.get().getPlayerNickname(),player.get().getPlayerId(), 1, ChronoUnit.DAYS);
            playerGold = player.get().getPlayerGold();
        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return SignInResponseDto.success(accessToken, isFirst,playerGold);
    }
}
