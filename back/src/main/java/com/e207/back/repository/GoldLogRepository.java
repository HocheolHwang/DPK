package com.e207.back.repository;

import com.e207.back.entity.GoldLogEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface GoldLogRepository extends JpaRepository<GoldLogEntity, Long> {
}
