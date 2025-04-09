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
@Table(name="Programs_With_Tracks")
@Data
@AllArgsConstructor
@Builder(toBuilder=true)
@NoArgsConstructor
public class ProgramsWithTracks {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;
    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Track",nullable = false)
    @JsonBackReference
    private Track track;

    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Program",nullable = false)
    @JsonBackReference
    private Schedule program;

    public ProgramsWithTracks(Track track, Schedule program) {
        this.track = track;
        this.program = program;
    }
}