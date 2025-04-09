package org.example.interpritator;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

public class VarReader {
    public static Map<String, Integer> parseVariablesFromFile(String filePath) {
        Map<String, Integer> variablesMap = new HashMap<>();
        try (BufferedReader reader = new BufferedReader(new FileReader(filePath)))
        {
            String currentLine;
            while ((currentLine = reader.readLine()) != null) {
                currentLine = currentLine.trim();
                if (currentLine.contains("=")) {
                    String[] tokens = currentLine.split("=");
                    if (tokens.length == 2) {
                        String key = tokens[0].trim();
                        try {
                            int value = Integer.parseInt(tokens[1].trim());
                            variablesMap.put(key, value);
                        } catch (NumberFormatException ex) {
                            System.err.println("Неправильное значение переменной: " + tokens[1].trim());
                        }
                    } else {
                        System.err.println("Неверный формат строки: " + currentLine);
                    }
                }
            }
        } catch (IOException ex) {
            throw new RuntimeException("Ошибка при чтении файла", ex);
        }
        return variablesMap;
    }
}
