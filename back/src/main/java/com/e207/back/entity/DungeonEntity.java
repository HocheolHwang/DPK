package com.e207.back.entity;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "dungeon")
public class DungeonEntity {
    @Id
    @Column(name = "dungeon_code", nullable = false)
    private String dungeonCode;

    @Column(name = "dungeon_name", nullable = false)
    private String dungeonName;
}
