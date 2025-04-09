package com.example.MyRadiostation.controllers;

import com.example.MyRadiostation.models.Album;
import com.example.MyRadiostation.models.Track;
import com.example.MyRadiostation.repositories.AlbumsRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PostAuthorize;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Date;
import java.util.Optional;

@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class AlbumsController {
    @Autowired
    private final AlbumsRepository albumsRepository;


    @GetMapping("/albums")
    public String allAlbums(Model model) {
            Iterable<Album> albums = albumsRepository.findAll();
            model.addAttribute("albums", albums);
            return "albums";
    }

    @GetMapping("/albums-find/**")
    public String getAlbumByAname(@RequestParam(name = "aname", required = false) String aname,@RequestParam(name = "datecreate", required = false) LocalDate datecreate, Model model) {
        if(aname !=null && datecreate==null) {
            model.addAttribute("albums", albumsRepository.findByAname(aname));
            return "albums-find";
        }
        else if (aname ==null && datecreate!=null) {
            model.addAttribute("albums", albumsRepository.findByDatecreate(datecreate));
            return "albums-find";
        }
        else{
            model.addAttribute("albums", albumsRepository.findByAname(aname));
            return "albums-find";
        }
    }

    @GetMapping("/albums/{id}")
    public String getAlbum(@PathVariable(value = "id") Long id, Model model) {
        Optional<Album> album = albumsRepository.findById(id);
        ArrayList<Album> res = new ArrayList<>();
        album.ifPresent(res::add);
        model.addAttribute("album",res);
        return "album-info";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/albums/{id}/edit")
    public String AlbumEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<Album> album = albumsRepository.findById(id);
        ArrayList<Album> res = new ArrayList<>();
        album.ifPresent(res::add);
        model.addAttribute("album",res);
        return "album-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/albums/create")
    public String albumCreate(Album album) {
        return "album-create";
    }

    @PostMapping("/albums/create")
    @PreAuthorize("hasRole('ADMIN')")
    public String albumCreate(@RequestParam(required = false) String aname, @RequestParam(value = "datecreate", required = false) LocalDate datecreate, Model model) {
        albumsRepository.save(new Album(aname, datecreate));
        return "redirect:/albums";
    }

    @PostMapping("/albums/{id}/delete")
    @PreAuthorize("hasRole('ADMIN')")
    public String albumDelete(@PathVariable(value = "id") Long id) {
        Album album = albumsRepository.findById(id).orElseThrow();
        albumsRepository.delete(album);
        return "redirect:/albums";
    }

    @PostMapping("/albums/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String albumEdit(@PathVariable(value = "id") Long id,@RequestParam(required = false) String aname, @RequestParam(value = "datecreate", required = false) LocalDate datecreate) {
        Album album = albumsRepository.findById(id).orElseThrow();
        album.setAname(aname);
        album.setDatecreate(datecreate);
        albumsRepository.save(album);
        return "redirect:/albums";
    }

}
