package org.example.core;

import org.example.interpritator.Interpreter;
import org.example.lexical.LexicalAnalyzer;
import org.example.syntax.SyntaxAnalyzer;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class BeanConfiguration {
    @Bean
    public SyntaxAnalyzer syntaxAnalyzer(LexicalAnalyzer lexicalAnalyzer) {
        return new SyntaxAnalyzer(lexicalAnalyzer);
    }

    @Bean
    public LexicalAnalyzer lexicalAnalyzer() {
        return new LexicalAnalyzer();
    }

    @Bean
    public Interpreter interpreter(SyntaxAnalyzer syntaxAnalyzer) {
        return new Interpreter(syntaxAnalyzer);
    }
}
