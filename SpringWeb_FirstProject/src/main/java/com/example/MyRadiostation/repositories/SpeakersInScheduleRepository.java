package com.example.MyRadiostation.repositories;

import com.example.MyRadiostation.models.ProgramsWithTracks;
import com.example.MyRadiostation.models.SpeakersInSchedule;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface SpeakersInScheduleRepository extends JpaRepository<SpeakersInSchedule,Long> {
    List<SpeakersInSchedule> findBySpeaker_Id(Long idspeaker);
    List<SpeakersInSchedule> findBySpeaker_surname(String surname);
    List<SpeakersInSchedule> findByProgram_Id(Long idprogram);
    List<SpeakersInSchedule> findByProgram_pname(String pname);
}
