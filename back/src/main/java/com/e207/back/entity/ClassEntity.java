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
@Table(name = "class")
public class ClassEntity {
    @Id
    @Column(name = "class_code", nullable = false, length = 4)
    private String classCode;

    @Column(name = "class_name", nullable = false)
    private String className;
}
