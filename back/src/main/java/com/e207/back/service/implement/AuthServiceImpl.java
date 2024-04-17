package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.SignInRequestDto;
import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SignInResponseDto;
import com.e207.back.dto.response.SignUpResponseDto;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.provider.JwtProvider;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.AuthService;
import com.e207.back.utill.ValidationUill;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.temporal.ChronoUnit;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class AuthServiceImpl implements AuthService {

    private final PlayerRepository playerRepository;
    private final BCryptPasswordEncoder bCryptPasswordEncoder;
    private final JwtProvider jwtProvider;
    @Override
    public ResponseEntity<? super SignUpResponseDto> signUp(SignUpRequestDto dto) {

        PlayerEntity newPlayer = new PlayerEntity();
        newPlayer.setPlayerId(dto.getPlayerId());
        newPlayer.setPassword(bCryptPasswordEncoder.encode(dto.getPlayerPassword()));
        newPlayer.setPlayerNickname(dto.getNickname());

        if(playerRepository.findById(dto.getPlayerId()).isPresent()){
            return SignUpResponseDto.duplicateId();
        }

        if(!ValidationUill.isValidUsername(dto.getPlayerId())){
            return SignUpResponseDto.playerIdValidationFail();
        }
        if(!ValidationUill.isValidNickname(dto.getNickname())){
            return SignUpResponseDto.playerNickNameValidationFail();
        }
        if(!ValidationUill.isValidPassword(dto.getPlayerPassword())){
            return SignUpResponseDto.playerPasswordValidationFail();
        }
        if(!(dto.getPlayerPassword().equals(dto.getPlayerPasswordCheck()))){
            return SignUpResponseDto.playerPasswordCheckValidationFail();
        }

        playerRepository.save(newPlayer);

        return SignUpResponseDto.success();
    }

    @Override
    public ResponseEntity<? super SignInResponseDto> signIn(SignInRequestDto dto) {
        Optional<PlayerEntity> player = playerRepository.findById(dto.getPlayerId());

        String encodedPassword = bCryptPasswordEncoder.encode(dto.getPlayerPassword());
        if(!bCryptPasswordEncoder.matches(dto.getPlayerPassword(), player.get().getPassword())){
            return SignInResponseDto.playerPasswordValidationFail();
        }


        String accessToken = jwtProvider.createToken(player.get().getPlayerNickname(),player.get().getPlayerId(), 1, ChronoUnit.DAYS);

        return SignInResponseDto.success(accessToken);
    }
}
