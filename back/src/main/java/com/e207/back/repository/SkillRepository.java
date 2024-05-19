package com.e207.back.repository;

import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.SkillEntity;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface SkillRepository extends JpaRepository<SkillEntity, String> {

    List<SkillEntity> findAllByRequiredLevel(int level);
}
