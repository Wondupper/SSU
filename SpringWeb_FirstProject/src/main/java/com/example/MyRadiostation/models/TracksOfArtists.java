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
@Table(name="Tracks_Of_Artists")
@Data
@Builder(toBuilder=true)
@AllArgsConstructor
@NoArgsConstructor
public class TracksOfArtists {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;

    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Artist",nullable = false)
    @JsonBackReference
    private Artist artist;

    @ManyToOne(cascade = CascadeType.REFRESH,fetch = FetchType.LAZY)
    @JoinColumn(name = "Id_Track",nullable = false)
    @JsonBackReference
    private Track track;

    public TracksOfArtists(Artist artist, Track track) {
        this.artist = artist;
        this.track = track;
    }
}