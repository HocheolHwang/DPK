package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

@Getter
@Setter
public class CurrentClassResponseDto extends ResponseDto {
    private String classCode;
    private Long currentExp;
    private int playerLevel;
    private int skillPoint;

    CurrentClassResponseDto(String classCode, Long currentExp, int playerLevel, int skillPoint){
        super();
        this.classCode = classCode;
        this.currentExp = currentExp;
        this.playerLevel = playerLevel;
        this.skillPoint = skillPoint;
    }

    public static ResponseEntity<? super CurrentClassResponseDto> success(String classCode, Long currentExp, int playerLevel, int skillPoint){
        CurrentClassResponseDto responseBody = new CurrentClassResponseDto(classCode, currentExp, playerLevel, skillPoint);
        return ResponseEntity.ok(responseBody);
    }



}
