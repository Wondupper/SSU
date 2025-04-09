package com.example.MyRadiostation.repositories;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.MyRadiostation.models.ProgramsWithTracks;

import java.util.List;

public interface ProgramsWithTracksRepository extends JpaRepository<ProgramsWithTracks,Long> {
    List<ProgramsWithTracks> findByTrack_Id(Long idtrack);
    List<ProgramsWithTracks> findByTrack_tname(String tname);
    List<ProgramsWithTracks> findByProgram_Id(Long idprogram);
    List<ProgramsWithTracks> findByProgram_pname(String pname);
}
