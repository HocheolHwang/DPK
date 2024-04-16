package com.e207.back.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.io.Serializable;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "learned_skill")
public class LearnedSkillEntity {

    @EmbeddedId
    private LearnedSkillId id;  // Embeddable 클래스를 사용한 복합 키

    @MapsId("playerId")  // LearnedSkillId 내의 playerId 필드를 매핑합니다
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_id")
    private PlayerEntity player;

    @MapsId("skillCode")  // LearnedSkillId 내의 skillCode 필드를 매핑합니다
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "skill_code")
    private SkillEntity skill;

    @Column(name = "skill_slot", nullable = false)
    private int skillSlot;

    @Column(name = "is_deleted", nullable = false)
    private boolean isDeleted;

    // Constructors, getters, and setters
}

@Embeddable
class LearnedSkillId implements Serializable {

    private String playerId;
    private String skillCode;

    // 기본 생성자
    public LearnedSkillId() {}

    // 매개변수 있는 생성자
    public LearnedSkillId(String playerId, String skillCode) {
        this.playerId = playerId;
        this.skillCode = skillCode;
    }

    // getters, setters, hashCode, equals 구현
}