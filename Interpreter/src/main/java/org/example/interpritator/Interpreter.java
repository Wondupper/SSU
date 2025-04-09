package org.example.interpritator;

import lombok.RequiredArgsConstructor;
import org.example.exeptions.InterpreterException;
import org.example.polis.Command;
import org.example.polis.EntryType;
import org.example.ui.PostfixCollectionPrinter;
import org.example.polis.PostfixEntry;
import org.example.syntax.SyntaxAnalyzer;
import org.example.ui.InterpreterPrinter;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

@RequiredArgsConstructor
public class Interpreter {
    private final SyntaxAnalyzer syntaxAnalyzer;
    private List<PostfixEntry<?>> stack;
    private List<PostfixEntry<?>> postfixEntryList;
    private Map<String, Integer> variables;
    private InterpreterPrinter interpreterPrinter;
    public void start(String program, String variablesFilePath) {
        syntaxAnalyzer.startAnalyze(program);
        PostfixCollectionPrinter.printPostfixEntryTable(syntaxAnalyzer.getPostfixList());
        stack = new LinkedList<>();
        postfixEntryList = new ArrayList<>(syntaxAnalyzer.getPostfixList().getPostfixList());
        variables = VarReader.parseVariablesFromFile(variablesFilePath);
        interpreterPrinter = new InterpreterPrinter();
        int previousIndex;
        int currentIndex = 0;
        while (currentIndex < postfixEntryList.size()) {
            previousIndex = currentIndex;
            PostfixEntry<?> entry = postfixEntryList.get(currentIndex);
            if (entry.getEntryType() != EntryType.COMMAND) {
                stack.addFirst(entry);
                currentIndex++;
            } else {
                switch ((Command) entry.getValue()) {
                    case JMP-> currentIndex = popAddress();
                    case JZ-> {
                        int address = popAddress();
                        int condition = popValue();
                        if (condition == 0) {
                            currentIndex = address;
                        } else {
                            currentIndex++;
                        }
                    }
                    case SET-> {
                        int value = popValue();
                        String var = popVar();
                        variables.put(var, value);
                        currentIndex++;
                    }
                    case ADD-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue(var1 + var2);
                        currentIndex++;
                    }
                    case SUB-> {
                        int var2 = popValue();
                        int var1 = popValue();
                        pushValue(var1- var2);
                        currentIndex++;
                    }
                    case MUL-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue(var1 * var2);
                        currentIndex++;
                    }
                    case DIV-> {
                        int var2 = popValue();
                        int var1 = popValue();
                        pushValue(var1 / var2);
                        currentIndex++;
                    }
                    case CMPE-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue((var1 == var2) ? 1 : 0);
                        currentIndex++;
                    }
                    case CMPNE-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue((var1 != var2) ? 1 : 0);
                        currentIndex++;
                    }
                    case CMPL-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue((var2 < var1) ? 1 : 0);
                        currentIndex++;
                    }
                    case CMPG-> {
                        int var1 = popValue();
                        int var2 = popValue();
                        pushValue((var2 > var1) ? 1 : 0);
                        currentIndex++;
                    }
                    case AND-> {
                        boolean var1 = popBooleanValue();
                        boolean var2 = popBooleanValue();
                        pushValue((var1 && var2) ? 1 : 0);
                        currentIndex++;
                    }
                    case OR-> {
                        boolean var1 = popBooleanValue();
                        boolean var2 = popBooleanValue();
                        pushValue((var1 || var2) ? 1 : 0);
                        currentIndex++;
                    }
                }
            }
            StringBuilder variablesStringBuilder = new StringBuilder();
            for (Map.Entry<String, Integer> variableEntry : variables.entrySet()) {
                if (!variablesStringBuilder.isEmpty()) {
                    variablesStringBuilder.append(", ");
                }
                variablesStringBuilder.append(variableEntry.getKey()).append("=").append(variableEntry.getValue());
            }
            StringBuilder stackStringBuilder = new StringBuilder();
            for (PostfixEntry<?> entryInStack : stack) {
                if (!stackStringBuilder.isEmpty()) {
                    stackStringBuilder.append(", ");
                }
                stackStringBuilder.append(entryInStack.getValue());
            }
            interpreterPrinter.addToTable(
                    previousIndex,
                    variablesStringBuilder.toString(),
                    entry.getValue().toString(),
                    stackStringBuilder.toString()
            );
        }
        interpreterPrinter.print();
    }
    private boolean popBooleanValue() {
        int value = popValue();
        return value == 1;
    }
    private int popValue() {
        PostfixEntry<?> entry = stack.removeFirst();
        if (entry.getEntryType() == EntryType.CONST) {
            return (int) entry.getValue();
        } else if (entry.getEntryType() == EntryType.VAR) {
            if (!variables.containsKey(entry.getValue().toString())) {
                throw new InterpreterException("Ошибка интерпретатора");
            }
            return variables.get(entry.getValue().toString());
        } else {
            throw new InterpreterException("Ошибка интерпретатора");
        }
    }
    private String popVar() {
        PostfixEntry<?> entry = stack.removeFirst();
        if (entry.getEntryType() == EntryType.VAR) {
            return entry.getValue().toString();
        } else {
            throw new InterpreterException("Ошибка интерпретатора");
        }
    }
    private int popAddress() {
        PostfixEntry<?> entry = stack.removeFirst();
        if (entry.getEntryType() == EntryType.ADDRESS) {
            return (int) entry.getValue();
        } else {
            throw new RuntimeException("Ошибка интерпретатора");
        }
    }
    private void pushValue(int value) {
        PostfixEntry<?> entry = new PostfixEntry<>(EntryType.CONST, value);
        stack.addFirst(entry);
    }
}
