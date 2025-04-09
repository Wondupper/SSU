package org.example.ui;

import com.jakewharton.fliptables.FlipTable;
import org.example.core.Lexeme;

import java.util.ArrayList;
import java.util.List;

public class LexemeTablePrinter {
    public static void printLexemeTable(List<Lexeme> lexemes) {
        List<String> headers = new ArrayList<>(List.of("Значение", "Тип лексемы", "Категория лексемы", "Начало лексемы", "Конец лексемы"));
        List<String[]> data = new ArrayList<>(lexemes.size());
        for (Lexeme lexeme : lexemes) {
            String[] row = new String[5];
            row[0] = lexeme.value();
            row[1] = lexeme.lexemeType().toString();
            row[2] = lexeme.lexemeCategory().toString();
            row[3] = String.valueOf(lexeme.startIndex());
            row[4] = String.valueOf(lexeme.endIndex());

            String.valueOf(data.add(row));
        }
        System.out.println(FlipTable.of(headers.toArray(new String[0]), data.toArray(new String[0][])));
    }
}
