package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.models.*;
import com.example.MyRadiostation.repositories.TracksOfArtistsRepository;
import com.example.MyRadiostation.repositories.TracksRepository;
import com.example.MyRadiostation.repositories.ArtistsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;

import java.util.ArrayList;
import java.util.Optional;


@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class TracksOfArtistsController {
    @Autowired
    private final TracksOfArtistsRepository tracksOfArtistsRepository;
    @Autowired
    private final TracksRepository tracksRepository;
    @Autowired
    private final ArtistsRepository artistsRepository;

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracksofartists")
    public String allTracksOfArtists(Model model) {
            Iterable<TracksOfArtists> tracksofartists = tracksOfArtistsRepository.findAll();
            model.addAttribute("tracksofartists", tracksofartists);
            return "tracksofartists";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracksofartists-find/**")
    public String getTracksOfArtistsByTrackId(@RequestParam(name = "idtrack", required = false) Long idtrack,@RequestParam(name = "idartist", required = false) Long idartist, Model model) {
        if(idtrack !=null && idartist==null) {
            model.addAttribute("tracksofartists", tracksOfArtistsRepository.findByTrack_Id(idtrack));
            return "tracksofartists-find";
        }
        else if (idtrack ==null && idartist !=null) {
            model.addAttribute("tracksofartists",tracksOfArtistsRepository.findByArtist_Id(idartist));
            return "tracksofartists-find";
        }
        else {
            model.addAttribute("tracksofartists",tracksOfArtistsRepository.findByArtist_Id(idartist));
            return "tracksofartists-find";
        }
    }

//    @PreAuthorize("hasRole('ADMIN')")
//    @GetMapping("/tracksofartists-find/**")
//    public String getTracksOfArtistsByProgramId(@RequestParam(name = "idartist", required = false) Long idartist, Model model) {
//        model.addAttribute("tracksofartists",tracksOfArtistsRepository.findByArtist_Id(idartist));
//        return "tracksofartists-find";
//    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracksofartists/create")
    public String tracksofartistsCreate(TracksOfArtists tracksofartists) {
        return "tracksofartists-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/tracksofartists/create")
    public String TracksOfArtistsCreate(@RequestParam Long idtrack,@RequestParam Long idartist) {
        Artist artist =  artistsRepository.findById(idartist).orElseThrow();
        Track track = tracksRepository.findById(idtrack).orElseThrow();
        tracksOfArtistsRepository.save(new TracksOfArtists(artist,track));
        return "redirect:/tracksofartists";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracksofartists/{id}/edit")
    public String SpeakersInScheduleEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<TracksOfArtists> tracksofartists = tracksOfArtistsRepository.findById(id);
        ArrayList<TracksOfArtists> res = new ArrayList<>();
        tracksofartists.ifPresent(res::add);
        model.addAttribute("tracksofartists",res);
        return "tracksofartists-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/tracksofartists/{id}/delete")
    public String TracksOfArtistsDelete(@PathVariable(value="id") Long id) {
        TracksOfArtists tof = tracksOfArtistsRepository.findById(id).orElseThrow();
        tracksOfArtistsRepository.delete(tof);
        return "redirect:/tracksofartists";
    }

    @PostMapping("/tracksofartists/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String SpeakersInScheduleEdit(@PathVariable(value = "id") Long id,@RequestParam Long idtrack,@RequestParam Long idartist) {
        TracksOfArtists tracksofartists = tracksOfArtistsRepository.findById(id).orElseThrow();
        Track track =  tracksRepository.findById(idtrack).orElseThrow();
        Artist artist = artistsRepository.findById(idartist).orElseThrow();
        tracksofartists.setTrack(track);
        tracksofartists.setArtist(artist);
        tracksOfArtistsRepository.save(tracksofartists);
        return "redirect:/tracksofartists";
    }

}