package com.example.MyRadiostation.repositories;

import com.example.MyRadiostation.models.Artist;
import com.example.MyRadiostation.models.Track;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;
import java.util.List;

public interface ArtistsRepository extends JpaRepository<Artist,Long> {
    List<Artist> findBySurname(String surname);
    List<Artist> findByDateofbirth(LocalDate dateofbirth);
}
