package com.e207.back.entity.id;

import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.Setter;

import java.io.Serializable;

@Embeddable
@Getter
@Setter
public class PartyMemberId implements Serializable {

    private String playerId;
    private String partyId;

    // 기본 생성자
    public PartyMemberId() {
    }

    // 매개변수 있는 생성자
    public PartyMemberId(String playerId, String partyId) {
        this.playerId = playerId;
        this.partyId = partyId;
    }

    // getters, setters, hashCode, equals 구현
}
