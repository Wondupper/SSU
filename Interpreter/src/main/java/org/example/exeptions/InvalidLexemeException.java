package org.example.exeptions;

public class InvalidLexemeException extends RuntimeException{
    private final int position;
    private Object expectedLexeme;

    public InvalidLexemeException(Object expectedLexeme, int position) {
        super("Ожидолось " + expectedLexeme.toString() + " на позиции " + position);
        this.expectedLexeme = expectedLexeme;
        this.position = position;
    }

    public InvalidLexemeException(Object expectedLexeme1, Object expectedLexeme2, int position) {
        super("Ожидалось " + expectedLexeme1.toString() + " или " + expectedLexeme2.toString() + " На позиции " + position);
        this.expectedLexeme = expectedLexeme1;
        this.position = position;
    }
}
