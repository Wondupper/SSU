package org.example.core;

public record Lexeme(LexemeType lexemeType,LexemeCategory lexemeCategory,
                     String value, int startIndex, int endIndex) { }