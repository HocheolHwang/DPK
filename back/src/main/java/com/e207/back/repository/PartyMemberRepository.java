package com.e207.back.repository;


import com.e207.back.entity.PartyMemberEntity;
import com.e207.back.entity.id.PartyMemberId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PartyMemberRepository extends JpaRepository<PartyMemberEntity, PartyMemberId> {

}
