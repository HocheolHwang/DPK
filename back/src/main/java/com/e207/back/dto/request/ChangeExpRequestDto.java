package com.e207.back.dto.request;


import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class ChangeExpRequestDto {

    private int expDelta; // 획득 경험치
    private Long currentExp;
    private int playerLevel;
    private String classCode;
    private String reason;
    // 어떤 직업의
    // 얻은 경험치는 몇이고
    // 현재 경험치는 몇이고
    // 그래서 레벨이 몇이냐
}
