package com.example.InfobesJWT.service;

import com.example.InfobesJWT.model.User;
import com.example.InfobesJWT.security.jwt.JWTAuthentication;
import com.example.InfobesJWT.security.jwt.JWTProvider;
import com.example.InfobesJWT.security.jwt.JWTRequest;
import com.example.InfobesJWT.security.jwt.JWTResponse;
import io.jsonwebtoken.Claims;
import jakarta.security.auth.message.AuthException;
import lombok.NonNull;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class AuthService {

    private final UserService userService;
    private final Map<String, String> refreshStorage = new HashMap<>();
    private final JWTProvider jwtProvider;
    private final PasswordEncoder passwordEncoder;

    public JWTResponse login(@NonNull JWTRequest authRequest) {
        final User user = userService.getUserByUsername(authRequest.getUsername());
        if (passwordEncoder.matches(authRequest.getPassword(), user.getPassword())) {
            final String accessToken = jwtProvider.generateAccessToken(user.getUsername());
            final String refreshToken = jwtProvider.generateRefreshToken(user.getUsername());
            refreshStorage.put(user.getUsername(), refreshToken);
            return new JWTResponse(accessToken, refreshToken);
        } else {
            throw new RuntimeException("Invalid password");
        }
    }

    public JWTResponse signin(@NonNull JWTRequest authRequest) {
        userService.saveUser(authRequest.getUsername(), authRequest.getPassword());
        final String accessToken = jwtProvider.generateAccessToken(authRequest.getUsername());
        final String refreshToken = jwtProvider.generateRefreshToken(authRequest.getUsername());
        refreshStorage.put(authRequest.getUsername(), refreshToken);
        return new JWTResponse(accessToken, refreshToken);
    }

    public JWTResponse getAccessToken(@NonNull String refreshToken) {
        if (jwtProvider.validateRefreshToken(refreshToken)) {
            final Claims claims = jwtProvider.getRefreshClaims(refreshToken);
            final String login = claims.getSubject();
            final String saveRefreshToken = refreshStorage.get(login);
            if (saveRefreshToken != null && saveRefreshToken.equals(refreshToken)) {
                final User user = userService.getUserByUsername(login);
                final String accessToken = jwtProvider.generateAccessToken(user.getUsername());
                return new JWTResponse(accessToken, null);
            }
        }
        return new JWTResponse(null, null);
    }

    public JWTResponse refresh(@NonNull String refreshToken) {
        if (jwtProvider.validateRefreshToken(refreshToken)) {
            final Claims claims = jwtProvider.getRefreshClaims(refreshToken);
            final String login = claims.getSubject();
            final String saveRefreshToken = refreshStorage.get(login);
            if (saveRefreshToken != null && saveRefreshToken.equals(refreshToken)) {
                final User user = userService.getUserByUsername(login);
                final String accessToken = jwtProvider.generateAccessToken(user.getUsername());
                final String newRefreshToken = jwtProvider.generateRefreshToken(user.getUsername());
                refreshStorage.put(user.getUsername(), newRefreshToken);
                return new JWTResponse(accessToken, newRefreshToken);
            }
        }
        throw new RuntimeException("Невалидный JWT токен");
    }

    public JWTAuthentication getAuthInfo() {
        return (JWTAuthentication) SecurityContextHolder.getContext().getAuthentication();
    }

}
