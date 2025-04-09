package com.example.grpcjokeserver;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@JsonIgnoreProperties(ignoreUnknown = true)
public class JokeApiResponse {

    private boolean error;
    private String category;
    private String type;
    private String joke;
    private String setup;
    private String delivery;
    private String message;


    public boolean isError() {
        return error;
    }
    public void setError(boolean error) {
        this.error = error;
    }
    public String getCategory() {
        return category;
    }
    public void setCategory(String category) {
        this.category = category;
    }
    public String getType() {
        return type;
    }
    public void setType(String type) {
        this.type = type;
    }
    public String getJoke() {
        return joke;
    }
    public void setJoke(String joke) {
        this.joke = joke;
    }
    public String getSetup() {
        return setup;
    }
    public void setSetup(String setup) {
        this.setup = setup;
    }
    public String getDelivery() {
        return delivery;
    }
    public void setDelivery(String delivery) {
        this.delivery = delivery;
    }
    public String getMessage() {
        return message;
    }
    public void setMessage(String message) {
        this.message = message;
    }
}