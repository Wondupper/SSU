package com.example.MyRadiostation.repositories;

import com.example.MyRadiostation.models.ProgramsWithTracks;
import com.example.MyRadiostation.models.TracksOfArtists;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;


public interface TracksOfArtistsRepository extends JpaRepository<TracksOfArtists,Long> {
    List<TracksOfArtists> findByTrack_Id(Long idtrack);
    List<TracksOfArtists> findByTrack_tname(String tname);
    List<TracksOfArtists> findByArtist_Id(Long idartist);
    List<TracksOfArtists> findByArtist_surname(String surname);
}
