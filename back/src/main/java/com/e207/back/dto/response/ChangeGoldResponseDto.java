package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.ResponseEntity;


@Getter
@Setter
public class ChangeGoldResponseDto extends ResponseDto {
    private long currentGold;

    ChangeGoldResponseDto(long currentGold){
        super();
        this.currentGold = currentGold;
    }

    public static ResponseEntity<? super ChangeGoldResponseDto> success(long currentGold){
        ChangeGoldResponseDto responseBody = new ChangeGoldResponseDto(currentGold);
        return ResponseEntity.ok(responseBody);
    }
}
