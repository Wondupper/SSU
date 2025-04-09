package org.example;

import org.example.core.BeanConfiguration;
import org.example.core.Lexeme;
import org.example.interpritator.Interpreter;
import org.example.lexical.LexicalAnalyzer;
import org.example.syntax.SyntaxAnalyzer;
import org.example.ui.LexemeTablePrinter;
import org.example.ui.PostfixCollectionPrinter;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;

public class Main {
    public static void main(String[] args) throws IOException {
        String input = Files.readString(Path.of("src/main/resources/test.txt"));
        String variables = "src/main/resources/variables.txt";
        AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(BeanConfiguration.class);
        SyntaxAnalyzer syntaxAnalyzer = context.getBean(SyntaxAnalyzer.class);
        //if(syntaxAnalyzer.startAnalyze(input)) PostfixCollectionPrinter.printPostfixEntryTable(syntaxAnalyzer.getPostfixList());
        Interpreter interpreter = context.getBean(Interpreter.class);
        interpreter.start(input, variables);
    }
}