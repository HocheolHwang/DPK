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

    @MapsId("classCode")  // LearnedSkillId 내의 skillCode 필드를 매핑합니다
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_code")
    private ClassEntity classEntity;


    @Column(name = "skill_slot", nullable = false)
    private int skillSlot;

    @Column(name = "active", columnDefinition = "boolean default true")
    private boolean active = true;


}

@Embeddable
@Getter
@Setter
class LearnedSkillId implements Serializable {
    @Column(name = "player_id")
    private String playerId;
    @Column(name = "skill_code")
    private String skillCode;
    @Column(name = "class_code")
    private String classCode;

    // 기본 생성자
    public LearnedSkillId() {}

    // 매개변수 있는 생성자
    public LearnedSkillId(String playerId, String skillCode, String classCode) {
        this.playerId = playerId;
        this.skillCode = skillCode;
        this.classCode = classCode;
    }

    // getters, setters, hashCode, equals 구현
}