package com.e207.back.entity;


import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;

import java.time.LocalDateTime;
import java.util.List;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "party")
public class PartyEntity {

    @Id
    @Column(name = "party_id", nullable = false, length = 36)
    private String partyId;

    @Column(name = "party_title", nullable = false)
    private String partyTitle = "None";

    @CreationTimestamp
    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;

    @OneToMany(mappedBy = "party")
    private List<PartyMemberEntity> partyMembers;

    // Getters, setters, equals, and hashCode methods
}