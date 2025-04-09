package com.example.MyRadiostation.models;

import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.Column;
import jakarta.persistence.*;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;

import java.time.LocalDate;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;


@Entity
@Table(name="Schedule")
@Data
@Builder(toBuilder=true)
@AllArgsConstructor
@NoArgsConstructor
public class Schedule {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;
    @Column(name = "pName")
    private String pname;
    @Column(name="Day_Of_Program")
    @DateTimeFormat(pattern = "yyyy-MM-dd")
    private LocalDate dayofprogram;
    @Column(name="Time_Start")
    private LocalTime timestart;
    @Column(name="Time_End")
    private LocalTime timeend;
    @OneToMany(cascade = CascadeType.ALL,fetch = FetchType.EAGER,mappedBy = "program")
    @JsonManagedReference
    private List<SpeakersInSchedule> speakersinschedule = new ArrayList<>();
    @OneToMany(cascade = CascadeType.ALL,fetch = FetchType.EAGER,mappedBy = "program")
    @JsonManagedReference
    private List<ProgramsWithTracks> programswithtracks = new ArrayList<>();

    public Schedule(String pname, LocalDate dayofprogram, LocalTime timestart, LocalTime timeend) {
        this.pname = pname;
        this.dayofprogram = dayofprogram;
        this.timestart = timestart;
        this.timeend = timeend;
    }
}
