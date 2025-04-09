package com.example.MyRadiostation.models;

import com.fasterxml.jackson.annotation.JsonBackReference;
import jakarta.persistence.Column;
import jakarta.persistence.*;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Table(name="Speakers_In_Schedule")
@Data
@Builder(toBuilder=true)
@AllArgsConstructor
@NoArgsConstructor
public class SpeakersInSchedule {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;

    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Speaker",nullable = false)
    @JsonBackReference
    private Speaker speaker;

    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Program",nullable = false)
    @JsonBackReference
    private Schedule program;

    public SpeakersInSchedule(Speaker speaker, Schedule program) {
        this.speaker = speaker;
        this.program = program;
    }
}