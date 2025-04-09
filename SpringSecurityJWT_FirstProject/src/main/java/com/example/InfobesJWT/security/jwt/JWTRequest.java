package com.example.InfobesJWT.security.jwt;

import lombok.Getter;
import lombok.Setter;

@Setter
@Getter
public class JWTRequest {
    private String username;
    private String password;
}
