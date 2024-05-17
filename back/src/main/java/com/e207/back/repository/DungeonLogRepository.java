package com.e207.back.repository;

import com.e207.back.entity.DungeonEntity;
import com.e207.back.entity.DungeonLogEntity;
import com.e207.back.entity.id.DungeonLogId;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface DungeonLogRepository extends JpaRepository<DungeonLogEntity, DungeonLogId> {

    List<DungeonLogEntity> findTop5ByDungeonAndIsClearedTrueOrderByClearTimeAscCreatedAtAsc(DungeonEntity dungeon);

}
