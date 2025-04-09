package org.example.exeptions;

public class UnexpectedSymbolsException extends RuntimeException{
    public UnexpectedSymbolsException(int position){
        super("Найдены лишние символы на позиции: " + position);
    }
}
