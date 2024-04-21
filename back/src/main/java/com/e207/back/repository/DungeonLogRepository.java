package com.e207.back.repository;

import com.e207.back.entity.DungeonEntity;
import com.e207.back.entity.DungeonLogEntity;
import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.id.DungeonLogId;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Slice;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface DungeonLogRepository extends JpaRepository<DungeonLogEntity, DungeonLogId> {

    List<DungeonLogEntity> findTop3ByDungeonOrderByClearTimeAscCreatedAtAsc(DungeonEntity dungeon);

}
