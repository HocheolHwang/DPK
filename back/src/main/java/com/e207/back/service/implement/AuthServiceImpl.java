package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.request.SignInRequestDto;
import com.e207.back.dto.request.SignUpRequestDto;
import com.e207.back.dto.response.SignInResponseDto;
import com.e207.back.dto.response.SignUpResponseDto;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.service.AuthService;
import com.e207.back.utill.ValidationUill;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AuthServiceImpl implements AuthService {

    private final PlayerRepository playerRepository;
    private final BCryptPasswordEncoder bCryptPasswordEncoder;
    @Override
    public ResponseEntity<? super SignUpResponseDto> signUp(SignUpRequestDto dto) {

        PlayerEntity newPlayer = new PlayerEntity();
        newPlayer.setPlayerId(dto.getPlayerId());
        newPlayer.setPassword(bCryptPasswordEncoder.encode(dto.getUserPassword()));
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
        if(!ValidationUill.isValidPassword(dto.getUserPassword())){
            return SignUpResponseDto.playerPasswordValidationFail();
        }

        playerRepository.save(newPlayer);

        return SignUpResponseDto.success();
    }

    @Override
    public ResponseEntity<? super SignInResponseDto> signIn(SignInRequestDto dto) {
        return null;
    }
}
