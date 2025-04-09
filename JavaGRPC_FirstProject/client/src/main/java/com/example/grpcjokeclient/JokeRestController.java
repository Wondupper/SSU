package com.example.grpcjokeclient;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/joke")
public class JokeRestController {

    private final JokeGrpcClientService jokeGrpcClientService;

    @Autowired
    public JokeRestController(JokeGrpcClientService jokeGrpcClientService) {
        this.jokeGrpcClientService = jokeGrpcClientService;
    }

    @GetMapping("/random")
    public ResponseEntity<String> getRandomJoke() {
        String joke = jokeGrpcClientService.getRandomJoke();
        return ResponseEntity.ok(joke);
    }

    @GetMapping("/genre")
    public ResponseEntity<String> getJokeByGenre(@RequestParam String genre) {
        String joke = jokeGrpcClientService.getJokeByGenre(genre);
        return ResponseEntity.ok(joke);
    }

    @GetMapping("/age")
    public ResponseEntity<String> getJokeByAge(@RequestParam int age) {
        String joke = jokeGrpcClientService.getJokeByAge(age);
        return ResponseEntity.ok(joke);
    }
}