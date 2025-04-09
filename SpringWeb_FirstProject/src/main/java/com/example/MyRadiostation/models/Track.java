package com.example.MyRadiostation.models;
import com.fasterxml.jackson.annotation.JsonBackReference;
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
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Entity
@Table(name="Tracks")
@Data
@Builder(toBuilder=true)
@AllArgsConstructor
@NoArgsConstructor
public class Track {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;
    @Column(name = "tName")
    private String tname;
     @Column(name="Date_Create")
     @DateTimeFormat(pattern = "yyyy-MM-dd")
     private LocalDate datecreate;
     @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
     @JoinColumn(name = "Id_Album",nullable = false)
     @JsonBackReference
     private Album album;

    @OneToMany(cascade = CascadeType.ALL,fetch = FetchType.EAGER,mappedBy = "track")
    @JsonManagedReference
    private List<TracksOfArtists> tracksofartists = new ArrayList<>();
    @OneToMany(cascade = CascadeType.ALL,fetch = FetchType.EAGER,mappedBy = "track")
    @JsonManagedReference
    private List<ProgramsWithTracks> programswithtracks = new ArrayList<>();

    public Track(String tname, LocalDate datecreate, Album album) {
        this.datecreate = datecreate;
        this.tname = tname;
        this.album = album;
    }
}
