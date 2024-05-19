package com.e207.back.repository;

import com.e207.back.entity.LearnedSkillEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.SkillEntity;
import com.e207.back.entity.id.LearnedSkillId;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface LearnedSkillRepository extends JpaRepository<LearnedSkillEntity, LearnedSkillId> {

    List<LearnedSkillEntity> findByPlayerPlayerIdAndClassEntityClassCode(String playerId, String classCode);
    Optional<LearnedSkillEntity> findByPlayerPlayerIdAndSkillSkillCode(String playerId,String skillCode);

    List<LearnedSkillEntity> findByPlayerPlayerIdAndActive(String playerId, boolean active);
}
