package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.ClassDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;

import java.util.ArrayList;
import java.util.List;

@Getter
@Setter
public class AllClassResponseDto extends ResponseDto {
    List<ClassDto> myClasses = new ArrayList<>();

    AllClassResponseDto(List<ClassDto> myClasses){
        super();
        this.myClasses = myClasses;
    }

    public static ResponseEntity<? super  AllClassResponseDto> success(List<ClassDto> myClasses){
        AllClassResponseDto responseBody = new AllClassResponseDto(myClasses);
        return ResponseEntity.ok(responseBody);
    }

}
