package com.e207.back.entity;


import com.e207.back.entity.id.PlayerClassId;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.io.Serializable;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Table(name = "player_class")
public class PlayerClassEntity {
    @EmbeddedId
    private PlayerClassId id;  // Embeddable 클래스를 사용한 복합 키


    @MapsId("playerId")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_id", insertable = false, updatable = false)
    private PlayerEntity player;

    @MapsId("classCode")
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_code", insertable = false, updatable = false)
    private ClassEntity _class;

    @Column(name = "player_level", nullable = false)
    private int playerLevel = 1;

    @Column(name = "player_exp", nullable = false)
    private long playerExp = 0;

    @Column(name = "skill_point", nullable = false)
    private int skillPoint = 0;

}

//@Getter
//@Setter
//@Embeddable
//class PlayerClassId implements Serializable {
//    private String playerId;
//    private String classCode;
//
//    public PlayerClassId() {}
//
//    // 매개변수 있는 생성자
//    public PlayerClassId(String playerId, String classCode) {
//        this.playerId = playerId;
//        this.classCode = classCode;
//    }
//
//}
