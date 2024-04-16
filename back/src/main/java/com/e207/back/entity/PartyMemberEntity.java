package com.e207.back.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;

import java.io.Serializable;
import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "party_member")
public class PartyMemberEntity {

    @EmbeddedId
    private PartyMemberId id;  // Embeddable 클래스를 사용한 복합 키

    @MapsId("playerId")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_id", referencedColumnName = "player_id")
    private PlayerEntity player;


    @MapsId("partyId")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "party_id", referencedColumnName = "party_id")
    private PartyEntity party;

    @CreationTimestamp
    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;
    // Getters, setters, equals, and hashCode methods
}

@Embeddable
@Getter
@Setter
class PartyMemberId implements Serializable {

    private String playerId;
    private Integer partyId;

    // 기본 생성자
    public PartyMemberId() {}

    // 매개변수 있는 생성자
    public PartyMemberId(String playerId, Integer partyId) {
        this.playerId = playerId;
        this.partyId = partyId;
    }

    // getters, setters, hashCode, equals 구현
}