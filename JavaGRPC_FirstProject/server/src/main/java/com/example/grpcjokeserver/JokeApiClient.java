package com.example.grpcjokeserver;

import com.example.grpcjokeserver.JokeApiResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

@Service
public class JokeApiClient {

    private final RestTemplate restTemplate;
    private static final String BASE_URL = "https://v2.jokeapi.dev/joke";

    @Autowired
    public JokeApiClient(RestTemplateBuilder builder) {
        this.restTemplate = builder.build();
    }

    public String getRandomJoke() {
        String url = BASE_URL + "/Any";
        return fetchJoke(url);
    }

    public String getJokeByGenre(String genre) {
        String url = BASE_URL + "/" + genre;
        return fetchJoke(url);
    }

    public String getJokeByAge(int age) {
        String url = BASE_URL + "/Any";
        // Если возраст меньше 18, добавляем фильтр safe-mode
        if (age < 18) {
            url += "?safe-mode";
        }
        return fetchJoke(url);
    }

    private String fetchJoke(String url) {
        try {
            ResponseEntity<JokeApiResponse> response = restTemplate.getForEntity(url, JokeApiResponse.class);
            if (response.getStatusCode().is2xxSuccessful() && response.getBody() != null) {
                JokeApiResponse jokeResponse = response.getBody();
                if (jokeResponse.isError()) {
                    return "Error: " + jokeResponse.getMessage();
                } else {
                    if ("single".equalsIgnoreCase(jokeResponse.getType())) {
                        return jokeResponse.getJoke();
                    } else if ("twopart".equalsIgnoreCase(jokeResponse.getType())) {
                        return jokeResponse.getSetup() + " - " + jokeResponse.getDelivery();
                    } else {
                        return "Unknown joke format";
                    }
                }
            }
        } catch (Exception e) {
            return "Exception occurred: " + e.getMessage();
        }
        return "No joke found";
    }
}