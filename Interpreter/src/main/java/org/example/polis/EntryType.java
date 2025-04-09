package org.example.polis;

public enum EntryType {
    COMMAND,
    VAR,
    CONST,
    ADDRESS,
    EMPTY;

    @Override
    public String toString() {
        return switch (this) {
            case COMMAND-> "COMMAND";
            case VAR-> "VAR";
            case CONST-> "CONST";
            case ADDRESS-> "ADDRESS";
            case EMPTY-> "EMPTY";
        };
    }
}
