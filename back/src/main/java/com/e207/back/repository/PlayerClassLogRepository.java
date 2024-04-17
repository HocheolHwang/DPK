package com.e207.back.repository;


import com.e207.back.entity.PlayerClassLogEntity;
import com.e207.back.entity.PlayerEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface PlayerClassLogRepository extends JpaRepository<PlayerClassLogEntity, Long> {
    Optional<PlayerClassLogEntity> findTop1ByPlayerOrderByCreatedAtDesc(PlayerEntity player);
}
