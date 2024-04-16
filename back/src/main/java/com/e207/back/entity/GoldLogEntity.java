package com.e207.back.entity;

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
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "gold_log")
public class GoldLogEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "gold_log_seq", nullable = false)
    private Long goldLogSeq;

    @ManyToOne
    @JoinColumn(name = "player_id", referencedColumnName = "player_id", nullable = false)
    private PlayerEntity player; // 연관된 Player 엔티티


    @Column(name = "gold_delta", nullable = false)
    private int goldDelta;

    @Column(name = "gold_log_reason", nullable = false, length = 16)
    private String goldLogReason;

    @CreationTimestamp
    @Column(name = "created_at", nullable = false, updatable = false)
    private LocalDateTime createdAt;


}
