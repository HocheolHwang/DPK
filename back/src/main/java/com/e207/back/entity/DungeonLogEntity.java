package com.e207.back.entity;

import com.e207.back.entity.id.DungeonLogId;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;

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

    @Column(name = "clear_time", nullable = true)
    private Long clearTime;


}
