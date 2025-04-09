package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.models.Album;
import com.example.MyRadiostation.repositories.AlbumsRepository;
import com.example.MyRadiostation.repositories.TracksRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import com.example.MyRadiostation.models.Track;
import lombok.RequiredArgsConstructor;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Date;
import java.util.Optional;


@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class TracksController {
    @Autowired
    private final TracksRepository tracksRepository;
    @Autowired
     private final AlbumsRepository albumsRepository;

    @GetMapping("/tracks")
    public String allTracks(Model model) {
            Iterable<Track> tracks = tracksRepository.findAll();
            model.addAttribute("tracks", tracks);
            return "tracks";
    }

    @GetMapping("/newesttracks/**")
    public String NewestTracks(Model model) {
        model.addAttribute("tracks", tracksRepository.findFirstByOrderByDatecreateDesc());
        return "newesttracks";
    }

    @GetMapping("/trackslistenings/**")
    public String TracksListenings(Model model) {
        model.addAttribute("trackslistenings", tracksRepository.CountListeningsTracks());
        return "trackslistenings";
    }

    @GetMapping("/tracks-find/**")
    public String getTrackByTname(@RequestParam(name = "tname", required = false) String tname,@RequestParam(value = "datecreate",required = false) LocalDate datecreate, Model model) {
        if(tname!=null && datecreate==null) {
            model.addAttribute("tracks", tracksRepository.findByTname(tname));
            return "tracks-find";
        } else if (tname==null && datecreate!=null) {
            model.addAttribute("tracks", tracksRepository.findByDatecreate(datecreate));
            return "tracks-find";
        }
        else{
            model.addAttribute("tracks", tracksRepository.findByTname(tname));
            return "tracks-find";
        }
    }

    @GetMapping("/tracks/{id}")
    public String getTrack(@PathVariable(value = "id") Long id, Model model) {
        Optional<Track> track = tracksRepository.findById(id);
        ArrayList<Track> res = new ArrayList<>();
        track.ifPresent(res::add);
        model.addAttribute("track",res);
        return "track-info";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracks/{id}/edit")
    public String TrackEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<Track> track = tracksRepository.findById(id);
        ArrayList<Track> res = new ArrayList<>();
        track.ifPresent(res::add);
        model.addAttribute("track",res);
        return "track-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/tracks/create")
    public String trackCreate(Track track) {
        return "track-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/tracks/create")
    public String trackCreate(@RequestParam(required = false) String tname, @RequestParam(value = "datecreate",required = false) LocalDate datecreate, @RequestParam Long idalbum, Model model) {
        Album album = albumsRepository.findById(idalbum).orElseThrow();
        tracksRepository.save(new Track(tname,datecreate,album));
        return "redirect:/tracks";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/tracks/{id}/delete")
    public String trackDelete(@PathVariable(value="id") Long id, Model model) {
        Track track = tracksRepository.findById(id).orElseThrow();
        tracksRepository.delete(track);
        return "redirect:/tracks";
    }

    @PostMapping("/tracks/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String trackEdit(@PathVariable(value = "id") Long id,@RequestParam(required = false) String tname, @RequestParam(value = "datecreate",required = false) LocalDate datecreate, @RequestParam Long idalbum) {
        Track track = tracksRepository.findById(id).orElseThrow();
        Album album = albumsRepository.findById(idalbum).orElseThrow();
        track.setTname(tname);
        track.setDatecreate(datecreate);
        track.setAlbum(album);
        tracksRepository.save(track);
        return "redirect:/tracks";
    }
    
}
