package com.example.MyRadiostation.models;

import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.Column;
import jakarta.persistence.*;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Entity
@Table(name="Albums")
@Data
@Builder(toBuilder=true)
@AllArgsConstructor
@NoArgsConstructor
public class Album {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Long id;
    @Column(name = "aName")
    private String aname;
    @Column(name="Date_Create")
    @DateTimeFormat(pattern = "yyyy-MM-dd")
    private LocalDate datecreate;
    @OneToMany(cascade = CascadeType.ALL,fetch = FetchType.EAGER,mappedBy = "album")
    @JsonManagedReference
    private List<Track> tracks = new ArrayList<>();

    public Album(String aname,LocalDate datecreate) {
        this.datecreate = datecreate;
        this.aname = aname;
    }
}
