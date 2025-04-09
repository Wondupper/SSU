package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.repositories.GenresRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import com.example.MyRadiostation.models.Genre;
import lombok.RequiredArgsConstructor;
import org.springframework.ui.Model;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Optional;

@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class GenresController {
    @Autowired
    private final GenresRepository genresRepository;

    @GetMapping("/genres")
    public String allGenres(Model model  ) {
            Iterable<Genre> genres = genresRepository.findAll();
            model.addAttribute("genres", genres);
            return "genres";
    }

    @GetMapping("/genres-find/**")
    public String getGenreByGname(@RequestParam(name = "gname", required = false) String gname, Model model) {
        model.addAttribute("genres",genresRepository.findByGname(gname));
        return "genres-find";
    }

    @GetMapping("/genres/{id}")
    public String getGenre(@PathVariable(value = "id") Long id, Model model) {
        Optional<Genre> genre = genresRepository.findById(id);
        ArrayList<Genre> res = new ArrayList<>();
        genre.ifPresent(res::add);
        model.addAttribute("genre",res);
        return "genre-info";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/genres/{id}/edit")
    public String GenreEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<Genre> genre = genresRepository.findById(id);
        ArrayList<Genre> res = new ArrayList<>();
        genre.ifPresent(res::add);
        model.addAttribute("genre",res);
        return "genre-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/genres/create")
    public String genreCreate(Genre genre) {
        return "genre-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/genres/create")
    public String genreCreate(@RequestParam(required = false) String gname) {
        genresRepository.save(new Genre(gname));
        return "redirect:/genres";
    }


    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/genres/{id}/delete")
    public String genreDelete(@PathVariable(value="id") Long id) {
        Genre genre = genresRepository.findById(id).orElseThrow();
        genresRepository.delete(genre);
        return "redirect:/genres";
    }

    @PostMapping("/genres/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String genreEdit(@PathVariable(value = "id") Long id,@RequestParam(required = false) String gname) {
        Genre genre = genresRepository.findById(id).orElseThrow();
        genre.setGname(gname);
        genresRepository.save(genre);
        return "redirect:/genres";
    }

}