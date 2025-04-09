package org.example.polis;

public enum Command {
    JMP,
    JZ,
    SET,
    ADD,
    SUB,
    MUL,
    DIV,
    CMPE,
    CMPNE,
    CMPL,
    CMPG,
    AND,
    OR;
    @Override
    public String toString() {
        return switch (this) {
            case JMP-> "JMP";
            case JZ-> "JZ";
            case SET-> "SET";
            case ADD-> "ADD";
            case SUB-> "SUB";
            case MUL-> "MUL";
            case DIV-> "DIV";
            case CMPE-> "CMPE";
            case CMPNE-> "CMPNE";
            case CMPL-> "CMPL";
            case CMPG-> "CMPG";
            case AND-> "AND";
            case OR-> "OR";
        };
    }
}
