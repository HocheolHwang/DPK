package com.e207.back.repository;

import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.id.PlayerClassId;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Slice;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PlayerClassRepository extends JpaRepository<PlayerClassEntity, PlayerClassId> {
//    Slice<User> findByOrderByScoreDesc(Pageable pageable);
    Slice<PlayerClassEntity> findByOrderByPlayerLevelDesc(Pageable pageable);
}
