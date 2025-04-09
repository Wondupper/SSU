package org.example.exeptions;

public class NoLexemesFoundException extends RuntimeException{

    public NoLexemesFoundException(String msg){
        super(msg);
    }

    public NoLexemesFoundException(String msg, Throwable cause){
        super(msg, cause);
    }
}
