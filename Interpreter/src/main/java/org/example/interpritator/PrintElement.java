package org.example.interpritator;

public record PrintElement(
        Integer index,
        String variableValue,
        String element,
        String stack
) { }
