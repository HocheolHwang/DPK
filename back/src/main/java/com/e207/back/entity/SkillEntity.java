package com.e207.back.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "skill")
public class SkillEntity {

    @Id
    @Column(name = "skill_code", nullable = false, length = 4)
    private String skillCode;

    @Column(name = "skill_name", nullable = false)
    private String skillName = "None";

    @OneToMany(mappedBy = "skill")
    private List<LearnedSkillEntity> learnedSkills;
}
