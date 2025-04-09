package com.example.MyRadiostation.repositories;

import com.example.MyRadiostation.models.Track;
import org.springframework.data.jpa.repository.JpaRepository;

import com.example.MyRadiostation.models.Genre;

import java.util.List;

public interface GenresRepository extends JpaRepository<Genre,Long> {
    List<Genre> findByGname(String gname);
}
