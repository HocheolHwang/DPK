package com.e207.back.entity.id;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.Setter;

import java.io.Serializable;

@Embeddable
@Getter
@Setter
public class LearnedSkillId implements Serializable {
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