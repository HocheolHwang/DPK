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
@Table(name = "exp_log")
public class ExpLogEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "exp_log_seq", nullable = false)
    private Long expLogSeq;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumns({
            @JoinColumn(name = "player_id", referencedColumnName = "player_id"),
            @JoinColumn(name = "class_code", referencedColumnName = "class_code")
    })
    private PlayerClassEntity playerClassEntity;

    @Column(name = "exp_delta", nullable = false)
    private int expDelta;

    @Column(name = "exp_log_reason", nullable = false, length = 16)
    private String expLogReason;

    @Column(name = "current_level", nullable = false)
    private int currentLevel;

    @CreationTimestamp
    @Column(name = "created_at", nullable = false, updatable = false)
    private LocalDateTime createdAt;
}
