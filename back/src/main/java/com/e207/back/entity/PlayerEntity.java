package com.e207.back.entity;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;
import org.springframework.boot.autoconfigure.domain.EntityScan;

import java.math.BigInteger;
import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "player")
public class PlayerEntity {
    @Id // 기본 키 지정
    @Column(name = "player_id", nullable = false, length = 16)
    private String playerId;

    @Column(name = "player_nickname", nullable = false, length = 16)
    private String playerNickname;

    @Column(name = "password", nullable = false, length = 64)
    private String password;

    @Column(name = "player_gold", nullable = false)
    private BigInteger playerGold = BigInteger.ZERO;


    @Column(name = "player_level", nullable = false)
    private int playerLevel = 1;

    @Column(name = "player_exp", nullable = false)
    private BigInteger playerExp = BigInteger.ZERO;

    @Column(name = "isFirst", nullable = false)
    private boolean isFirst = true;

    @CreationTimestamp
    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;
}
