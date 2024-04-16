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
@Table(name = "dungeon_log")
public class DungeonLogEntity {
    @EmbeddedId
    private DungeonLogId id;  // Embeddable 클래스를 사용한 복합 키


    @MapsId("partyId")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "party_id", insertable = false, updatable = false)
    private PartyEntity party;

    @MapsId("dungeonCode")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "dungeon_code", insertable = false, updatable = false)
    private DungeonEntity dungeon;

    @CreationTimestamp
    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;

    @Column(name = "is_cleared", nullable = false)
    private boolean isCleared;
}
@Embeddable
@Getter
@Setter
class DungeonLogId implements Serializable {

    private String dungeonCode;
    private String partyId;

    // 기본 생성자
    public DungeonLogId() {}

    // 매개변수 있는 생성자
    public DungeonLogId(String dungeonCode, String partyId) {
        this.dungeonCode = dungeonCode;
        this.partyId = partyId;
    }

    // getters, setters, hashCode, equals 구현
}