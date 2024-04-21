package com.e207.back.repository;

import com.e207.back.entity.DungeonEntity;
import org.springframework.data.jpa.repository.JpaRepository;

public interface DungeonRepository extends JpaRepository<DungeonEntity, String> {
}
