package org.example.polis;

public interface PostfixCollection {
    int writeCommand(Command command);
    int writeVar(String value);
    int writeConst(Integer value);
    int writeAddress(Integer address);
    void setAddress(int index, Integer address);
}
