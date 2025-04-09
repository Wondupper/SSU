package com.example.InfobesJWT.controller;

import com.example.InfobesJWT.security.jwt.JWTRequest;
import com.example.InfobesJWT.security.jwt.JWTResponse;
import com.example.InfobesJWT.service.AuthService;
import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestMapping;

@RestController
@RequestMapping("/log-in")
@RequiredArgsConstructor
public class LogInController {

    private final AuthService authService;

    @PostMapping()
    public ResponseEntity<?> login(@RequestBody JWTRequest authRequest, HttpServletResponse response) {
        final JWTResponse token = authService.login(authRequest);
        Cookie accessToken =  new Cookie("Access-Token",token.getAccessToken());
        accessToken.setHttpOnly(true);
        response.addCookie(accessToken);
        Cookie refreshToken = new Cookie("Refresh-Token",token.getRefreshToken());
        refreshToken.setHttpOnly(true);
        refreshToken.setPath("/refresh");
        response.addCookie(refreshToken);
        return ResponseEntity.ok().build();
    }
}

