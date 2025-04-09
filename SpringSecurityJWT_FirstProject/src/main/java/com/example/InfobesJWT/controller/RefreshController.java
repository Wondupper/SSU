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
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestMapping;

@RestController
@RequestMapping("/refresh")
@RequiredArgsConstructor
public class RefreshController {

    private final AuthService authService;

    @PostMapping()
    public ResponseEntity<?> getNewRefreshToken(@RequestBody RefreshJWTRequest request, HttpServletResponse response) {
        final String token = authService.refresh(request.getRefreshToken()).getRefreshToken();
        Cookie refreshToken = new Cookie("Refresh-Token",token);
        refreshToken.setHttpOnly(true);
        refreshToken.setPath("/refresh");
        response.addCookie(refreshToken);
        return ResponseEntity.ok().build();
    }

}

