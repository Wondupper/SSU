package com.example.InfobesJWT.controller;

import com.example.InfobesJWT.security.jwt.JWTResponse;
import com.example.InfobesJWT.security.jwt.RefreshJWTRequest;
import com.example.InfobesJWT.service.AuthService;
import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/access")
@RequiredArgsConstructor
public class AccessController {

    private final AuthService authService;

    @PostMapping()
    public ResponseEntity<?> getNewAccessToken(@RequestBody RefreshJWTRequest request, HttpServletResponse response) {
        final String token = authService.getAccessToken(request.getRefreshToken()).getAccessToken();
        Cookie accessToken = new Cookie("Access-Token",token);
        accessToken.setHttpOnly(true);
        response.addCookie(accessToken);
        return ResponseEntity.ok().build();
    }
}
