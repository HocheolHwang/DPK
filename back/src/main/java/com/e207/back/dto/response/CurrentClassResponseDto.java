package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

@Getter
@Setter
public class CurrentClassResponseDto extends ResponseDto {
    private String classCode;

    CurrentClassResponseDto(String classCode){
        super();
        this.classCode = classCode;
    }

    public static ResponseEntity<? super CurrentClassResponseDto> success(String classCode){
        CurrentClassResponseDto responseBody = new CurrentClassResponseDto(classCode);
        return ResponseEntity.ok(responseBody);
    }



}
