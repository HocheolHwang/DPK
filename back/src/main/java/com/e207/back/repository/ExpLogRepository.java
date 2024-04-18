package com.e207.back.repository;

import com.e207.back.entity.ExpLogEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ExpLogRepository extends JpaRepository<ExpLogEntity, Long> {
}
