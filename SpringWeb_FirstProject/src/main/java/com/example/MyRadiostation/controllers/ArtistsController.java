package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.models.Genre;
import com.example.MyRadiostation.models.Track;
import com.example.MyRadiostation.repositories.ArtistsRepository;
import com.example.MyRadiostation.repositories.GenresRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import com.example.MyRadiostation.models.Artist;
import lombok.RequiredArgsConstructor;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Date;
import java.util.Optional;

@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class ArtistsController {
    @Autowired
    private final ArtistsRepository artistsRepository;
    @Autowired
    private final GenresRepository genresRepository;

    @GetMapping("/artists")
    public String allArtists(Model model) {
            Iterable<Artist> artists = artistsRepository.findAll();
            model.addAttribute("artists", artists);
            return "artists";
    }

    @GetMapping("/artists-find/**")
    public String getArtistBySurname(@RequestParam(name = "surname", required = false) String surname, @RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, Model model) {
        if(surname!=null && dateofbirth==null) {
            model.addAttribute("artists", artistsRepository.findBySurname(surname));
            return "artists-find";
        } else if (surname==null && dateofbirth!=null) {
            model.addAttribute("artists", artistsRepository.findByDateofbirth(dateofbirth));
            return "artists-find";
        }
        else{
            model.addAttribute("artists", artistsRepository.findBySurname(surname));
            return "artists-find";
        }
    }

    @GetMapping("/artists/{id}")
    public String getArtist(@PathVariable(value = "id") Long id, Model model) {
        Optional<Artist> artist = artistsRepository.findById(id);
        ArrayList<Artist> res = new ArrayList<>();
        artist.ifPresent(res::add);
        model.addAttribute("artist",res);
        return "artist-info";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/artists/{id}/edit")
    public String ArtistEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<Artist> artist = artistsRepository.findById(id);
        ArrayList<Artist> res = new ArrayList<>();
        artist.ifPresent(res::add);
        model.addAttribute("artist",res);
        return "artist-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/artists/create")
    public String artistCreate(Artist artist) {
        return "artist-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/artists/create")
    public String artistCreate(@RequestParam(value = "aname", required = false) String aname, @RequestParam(value = "surname", required = false) String surname, @RequestParam(value = "age", required = false) Short age, @RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, @RequestParam(value = "dateofdeath",required = false) LocalDate dateofdeath, @RequestParam Long idgenre,Model model) {
        Genre genre =  genresRepository.findById(idgenre).orElseThrow();
        artistsRepository.save(new Artist(aname,surname,age,dateofbirth,dateofdeath,genre));
        return "redirect:/artists";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/artists/{id}/delete")
    public String artistDelete(@PathVariable(value="id") Long id) {
        Artist artist = artistsRepository.findById(id).orElseThrow();
        artistsRepository.delete(artist);
        return "redirect:/artists";
    }

    @PostMapping("/artists/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String artistEdit(@PathVariable(value = "id") Long id,@RequestParam(value = "aname", required = false) String aname, @RequestParam(value = "surname", required = false) String surname, @RequestParam(value = "age", required = false) Short age, @RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, @RequestParam(value = "dateofdeath",required = false) LocalDate dateofdeath, @RequestParam Long idgenre) {
        Artist artist = artistsRepository.findById(id).orElseThrow();
        artist.setAname(aname);
        artist.setSurname(surname);
        artist.setAge(age);
        artist.setDateofbirth(dateofbirth);
        artist.setDateofdeath(dateofdeath);
        Genre genre = genresRepository.findById(idgenre).orElseThrow();
        artist.setGenre(genre);
        artistsRepository.save(artist);
        return "redirect:/artists";
    }

}