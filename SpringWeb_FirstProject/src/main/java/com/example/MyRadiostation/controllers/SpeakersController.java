package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.models.Track;
import com.example.MyRadiostation.repositories.SpeakersRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import com.example.MyRadiostation.models.Speaker;
import lombok.RequiredArgsConstructor;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Date;
import java.util.Optional;

@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class SpeakersController {
    @Autowired
    private final SpeakersRepository speakersRepository;

    @GetMapping("/speakers")
    public String allSpeakers(Model model) {
            Iterable<Speaker> speakers = speakersRepository.findAll();
            model.addAttribute("speakers", speakers);
            return "speakers";
    }

    @GetMapping("/speakers-find/**")
    public String getSpeakerBySurname(@RequestParam(name = "surname", required = false) String surname,@RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, Model model) {
        if(surname!=null && dateofbirth==null) {
            model.addAttribute("speakers", speakersRepository.findBySurname(surname));
            return "speakers-find";
        }
        else if(surname==null && dateofbirth!=null) {
            model.addAttribute("speakers", speakersRepository.findByDateofbirth(dateofbirth));
            return "speakers-find";
        }
        else{
            model.addAttribute("speakers", speakersRepository.findBySurname(surname));
            return "speakers-find";
        }
    }

    @GetMapping("/speakers/{id}")
    public String getSpeaker(@PathVariable(value = "id") Long id, Model model) {
        Optional<Speaker> speaker = speakersRepository.findById(id);
        ArrayList<Speaker> res = new ArrayList<>();
        speaker.ifPresent(res::add);
        model.addAttribute("speaker",res);
        return "speaker-info";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakers/{id}/edit")
    public String SpeakerEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<Speaker> speaker = speakersRepository.findById(id);
        ArrayList<Speaker> res = new ArrayList<>();
        speaker.ifPresent(res::add);
        model.addAttribute("speaker",res);
        return "speaker-edit";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakers/create")
    public String speakerCreate(Speaker speaker) {
        return "speaker-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/speakers/create")
    public String speakerCreate(@RequestParam(required = false) String sname, @RequestParam(required = false) String surname, @RequestParam(required = false) Short age, @RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, @RequestParam(value = "dateofdeath",required = false) LocalDate dateofdeath) {
        speakersRepository.save(new Speaker(sname,surname,age,dateofbirth,dateofdeath));
        return "redirect:/speakers";
    }


    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/speakers/{id}/delete")
    public String speakerDelete(@PathVariable(value="id") Long id, Model model) {
        Speaker speaker = speakersRepository.findById(id).orElseThrow();
        speakersRepository.delete(speaker);
        return "redirect:/speakers";
    }

    @PostMapping("/speakers/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String speakerEdit(@PathVariable(value = "id") Long id,@RequestParam(required = false) String sname, @RequestParam(required = false) String surname, @RequestParam(required = false) Short age, @RequestParam(value = "dateofbirth",required = false) LocalDate dateofbirth, @RequestParam(value = "dateofdeath",required = false) LocalDate dateofdeath) {
        Speaker speaker = speakersRepository.findById(id).orElseThrow();
        speaker.setSname(sname);
        speaker.setSurname(surname);
        speaker.setAge(age);
        speaker.setDateofbirth(dateofbirth);
        speaker.setDateofdeath(dateofdeath);
        speakersRepository.save(speaker);
        return "redirect:/speakers";
    }

}