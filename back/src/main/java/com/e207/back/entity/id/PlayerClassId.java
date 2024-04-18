package com.e207.back.entity.id;

import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.Setter;

import java.io.Serializable;

@Getter
@Setter
@Embeddable
public class PlayerClassId implements Serializable {
    private String playerId;
    private String classCode;

    public PlayerClassId() {}

    // 매개변수 있는 생성자
    public PlayerClassId(String playerId, String classCode) {
        this.playerId = playerId;
        this.classCode = classCode;
    }

}
