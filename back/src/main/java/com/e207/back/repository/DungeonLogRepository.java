package com.e207.back.repository;

import com.e207.back.entity.DungeonLogEntity;
import com.e207.back.entity.id.DungeonLogId;
import org.springframework.data.jpa.repository.JpaRepository;

public interface DungeonLogRepository extends JpaRepository<DungeonLogEntity, DungeonLogId> {
}
