package com.example.InfobesJWT.security.jwt;

import io.jsonwebtoken.Claims;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public final class JWTUtils {

    public static JWTAuthentication generate(Claims claims) {
        final JWTAuthentication jwtInfoToken = new JWTAuthentication();
        jwtInfoToken.setUsername(claims.getSubject());
        return jwtInfoToken;
    }

}