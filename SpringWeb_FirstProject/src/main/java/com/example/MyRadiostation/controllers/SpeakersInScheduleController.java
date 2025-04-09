package com.example.MyRadiostation.controllers;
import com.example.MyRadiostation.models.*;
import com.example.MyRadiostation.repositories.SpeakersInScheduleRepository;
import com.example.MyRadiostation.repositories.SpeakersRepository;
import com.example.MyRadiostation.repositories.ScheduleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import lombok.RequiredArgsConstructor;
import org.springframework.ui.Model;

import java.util.ArrayList;
import java.util.Optional;

@EnableMethodSecurity
@Controller
@RequiredArgsConstructor
public class SpeakersInScheduleController {
    @Autowired
    private final SpeakersInScheduleRepository speakersInScheduleRepository;
    @Autowired
    private final SpeakersRepository speakersRepository;
    @Autowired
    private final ScheduleRepository scheduleRepository;

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakersinschedule")
    public String allSpeakersInSchedule(Model model) {
            Iterable<SpeakersInSchedule> speakersinschedule = speakersInScheduleRepository.findAll();
            model.addAttribute("speakersinschedule", speakersinschedule);
            return "speakersinschedule";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakersinschedule-find/**")
    public String getSpeakersInScheduleByTrackId(@RequestParam(name = "idspeaker", required = false) Long idspeaker,@RequestParam(name = "idprogram", required = false) Long idprogram, Model model) {
        if(idspeaker !=null && idprogram ==null) {
            model.addAttribute("speakersinschedule", speakersInScheduleRepository.findBySpeaker_Id(idspeaker));
            return "speakersinschedule-find";
        }
        else if (idspeaker ==null && idprogram !=null) {
            model.addAttribute("speakersinschedule",speakersInScheduleRepository.findByProgram_Id(idprogram));
            return "speakersinschedule-find";
        }
        else {
            model.addAttribute("speakersinschedule",speakersInScheduleRepository.findByProgram_Id(idprogram));
            return "speakersinschedule-find";
        }
    }

//    @PreAuthorize("hasRole('ADMIN')")
//    @GetMapping("/speakersinschedule-find/**")
//    public String getSpeakersInScheduleByProgramId(@RequestParam(name = "idprogram", required = false) Long idprogram, Model model) {
//        model.addAttribute("speakersinschedule",speakersInScheduleRepository.findByProgram_Id(idprogram));
//        return "speakersinschedule-find";
//    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakersinschedule/create")
    public String speakersinscheduleCreate(SpeakersInSchedule speakersinschedule) {
        return "speakersinschedule-create";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/speakersinschedule/create")
    public String SpeakersInScheduleCreate(@RequestParam Long idspeaker,@RequestParam Long idprogram) {
        Speaker speaker =  speakersRepository.findById(idspeaker).orElseThrow();
        Schedule program = scheduleRepository.findById(idprogram).orElseThrow();
        speakersInScheduleRepository.save(new SpeakersInSchedule(speaker,program));
        return "redirect:/speakersinschedule";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @PostMapping("/speakersinschedule/{id}/delete")
    public String SpeakersInScheduleDelete(@PathVariable(value="id") Long id) {
        SpeakersInSchedule sis = speakersInScheduleRepository.findById(id).orElseThrow();
        speakersInScheduleRepository.delete(sis);
        return "redirect:/speakersinschedule";
    }

    @PreAuthorize("hasRole('ADMIN')")
    @GetMapping("/speakersinschedule/{id}/edit")
    public String SpeakersInScheduleEdit(@PathVariable(value = "id") Long id, Model model) {
        Optional<SpeakersInSchedule> speakersinschedule = speakersInScheduleRepository.findById(id);
        ArrayList<SpeakersInSchedule> res = new ArrayList<>();
        speakersinschedule.ifPresent(res::add);
        model.addAttribute("speakersinschedule",res);
        return "speakersinschedule-edit";
    }

    @PostMapping("/speakersinschedule/{id}/edit")
    @PreAuthorize("hasRole('ADMIN')")
    public String SpeakersInScheduleEdit(@PathVariable(value = "id") Long id,@RequestParam Long idspeaker,@RequestParam Long idprogram) {
        SpeakersInSchedule speakersinschedule = speakersInScheduleRepository.findById(id).orElseThrow();
        Schedule program =  scheduleRepository.findById(idprogram).orElseThrow();
        Speaker speaker = speakersRepository.findById(idspeaker).orElseThrow();
        speakersinschedule.setProgram(program);
        speakersinschedule.setSpeaker(speaker);
        speakersInScheduleRepository.save(speakersinschedule);
        return "redirect:/speakersinschedule";
    }

}