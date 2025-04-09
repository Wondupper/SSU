package org.example.ui;


import com.jakewharton.fliptables.FlipTable;
import org.example.interpritator.PrintElement;

import java.util.ArrayList;
import java.util.List;

public class InterpreterPrinter {
    private final List<PrintElement> elements;

    public InterpreterPrinter() {
        this.elements = new ArrayList<>();
    }

    public void addToTable(int index, String variableValue, String element, String stack) {
        PrintElement printElement = new PrintElement(index, variableValue, element, stack);
        elements.add(printElement);
    }

    public void print() {
        List<String> headers = new ArrayList<>(List.of("Шаг", "Элемент", "Стек", "Значения переменных", "Индекс"));
        List<String[]> data = new ArrayList<>(elements.size());
        for (Integer i = 0; i < elements.size()- 1; i++) {
            String[] row = new String[5];
            row[0] = i.toString();
            row[1] = elements.get(i).element();
            row[2] = elements.get(i).stack();
            row[3] = elements.get(i).variableValue();
            row[4] = elements.get(i).index().toString();
            data.add(row);
        }
        System.out.println(FlipTable.of(headers.toArray(new String[0]), data.toArray(new String[0][])));
    }
}
