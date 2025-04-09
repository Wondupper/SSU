package org.example.syntax;

import lombok.Getter;
import lombok.RequiredArgsConstructor;
import org.example.core.Lexeme;
import org.example.core.LexemeCategory;
import org.example.core.LexemeType;
import org.example.exeptions.InvalidLexemeException;
import org.example.exeptions.LexicalAnalysisException;
import org.example.exeptions.NoLexemesFoundException;
import org.example.exeptions.SyntaxAnalisisException;
import org.example.lexical.LexicalAnalyzer;
import org.example.polis.Command;
import org.example.polis.PostfixList;
import org.example.ui.LexemeTablePrinter;

import java.util.ListIterator;
import java.util.NoSuchElementException;
import java.util.Stack;

@RequiredArgsConstructor
public class SyntaxAnalyzer {
    private final LexicalAnalyzer lexicalAnalyzer;
    private ListIterator<Lexeme> lexemeIterator;
    private Lexeme currentLexeme;
    @Getter
    private PostfixList postfixList;
    private final Stack<Integer> endIndexStack = new Stack<>();
    private final Stack<Integer> elseifOrElseStack = new Stack<>();

    public boolean startAnalyze(String input) {
        try {
            lexicalAnalyzer.startAnalyze(input);
            LexemeTablePrinter.printLexemeTable(lexicalAnalyzer.getLexemes());
            checkLexemesAvailable();
            postfixList = new PostfixList();
            lexemeIterator = lexicalAnalyzer.getLexemes().listIterator();
            checkIfElseifElseBlocks();
            postfixList.writeEmpty();
            return true;
        } catch (IllegalArgumentException ex) {
            throw new LexicalAnalysisException("Ошибка при лексическом анализе: ", ex);
        } catch (InvalidLexemeException | NoSuchElementException ex) {
            throw new SyntaxAnalisisException("Ошибка при синтаксическом анализе: ", ex);
        }
    }

    private void checkLexemesAvailable() {
        if (lexicalAnalyzer.getLexemes().isEmpty()) {
            throw new NoLexemesFoundException("Список лексем пуст");
        }
    }

    private void checkIfElseifElseBlocks() {
        checkIfStatement();
        checkElseIfBlocks();
        checkElseStatement();
        checkEnd();
    }

    private void checkIfStatement() {
        goNextIfPossibleStart();
        if (currentLexeme.lexemeType() != LexemeType.IF) {
            throw new InvalidLexemeException("Ожидался if", currentLexeme.startIndex());
        }
        checkCondition();
        elseifOrElseStack.push(postfixList.writeAddress(null));
        postfixList.writeCommand(Command.JZ);
        checkThen();
        checkStatement();
        endIndexStack.push(postfixList.writeAddress(null));
        postfixList.writeCommand(Command.JMP);
        postfixList.setAddress(elseifOrElseStack.pop(), postfixList.getNextIndex());
        goNextIfPossibleEndAfterStart();
    }

    private void checkElseIfBlocks() {
        if (currentLexeme.lexemeType() == LexemeType.ELSEIF) {
            if (!elseifOrElseStack.isEmpty()) {
                postfixList.setAddress(elseifOrElseStack.pop(), postfixList.getNextIndex());
            }
            checkElseIfStatement();
        } else if (currentLexeme.lexemeType() != LexemeType.ELSE && currentLexeme.lexemeType() != LexemeType.END) {
            throw new InvalidLexemeException("Ожидался elseif, else или end", currentLexeme.startIndex());
        }
    }

    private void checkElseIfStatement() {
        checkCondition();
        elseifOrElseStack.push(postfixList.writeAddress(null));
        postfixList.writeCommand(Command.JZ);
        checkThen();
        checkStatement();
        endIndexStack.push(postfixList.writeAddress(null));
        postfixList.writeCommand(Command.JMP);
        postfixList.setAddress(elseifOrElseStack.pop(), postfixList.getNextIndex());
        goNextIfPossibleEndAfterStart();
        checkElseIfBlocks();
    }

    private void checkElseStatement() {
        if (currentLexeme.lexemeType() == LexemeType.ELSE) {
            if (!elseifOrElseStack.isEmpty()) {
                postfixList.setAddress(elseifOrElseStack.pop(), postfixList.getNextIndex());
            }
            checkStatement();
        } else if (currentLexeme.lexemeType() != LexemeType.END) {
            throw new InvalidLexemeException("Ожидался else или end", currentLexeme.startIndex());
        }
        goNextIfPossibleEndAfterStart();
    }

    private void checkEnd() {
        if (lexemeIterator.hasNext()) {
            throw new InvalidLexemeException("end должно быть концом программы", currentLexeme.startIndex());
        }
        while (!endIndexStack.isEmpty()) {
            postfixList.setAddress(endIndexStack.pop(), postfixList.getNextIndex());
        }
    }

    private void checkStatement() {
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
            checkASSIGMENTStatement();
            if (lexemeIterator.hasNext()) {
                currentLexeme = lexemeIterator.next();
                if (currentLexeme.lexemeType() != LexemeType.DELIMITER || !currentLexeme.value().equals(";")) {
                    throw new InvalidLexemeException("Ожидалась точка с запятой", currentLexeme.startIndex());
                }
            } else {
                throw new InvalidLexemeException("Ожидалась точка с запятой в конце оператора", currentLexeme.startIndex());
            }
        } else {
            throw new InvalidLexemeException("Ожидался оператор", currentLexeme.startIndex());
        }
    }


    private void checkASSIGMENTStatement() {
        if (currentLexeme.lexemeCategory() != LexemeCategory.IDENTIFIER) {
            throw new InvalidLexemeException(LexemeCategory.IDENTIFIER, "Ожидался идентификатор в присваивании", currentLexeme.startIndex());
        }
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
            if (currentLexeme.lexemeType() != LexemeType.ASSIGMENT) {
                throw new InvalidLexemeException(LexemeType.ASSIGMENT, "Ожидался оператор присваивания", currentLexeme.startIndex());
            }
            checkArithmeticExpression();
            postfixList.writeCommand(Command.SET);
        } else {
            throw new InvalidLexemeException(LexemeType.ASSIGMENT, "Ожидался оператор присваивания", currentLexeme.startIndex());
        }
    }

    private void checkArithmeticExpression() {
        checkOperand();
        postfixList.writeVar(currentLexeme.value());
        checkNextArithmeticOperation();
    }

    private void checkNextArithmeticOperation() {
        if (!lexemeIterator.hasNext()) {
            return;
        }

        currentLexeme = lexemeIterator.next();
        String operation = "";
        if (currentLexeme.lexemeType() == LexemeType.ARITHMETIC_LOW || currentLexeme.lexemeType() == LexemeType.ARITHMETIC_HIGH) {
            operation = currentLexeme.value();
            checkOperand();
            checkNextArithmeticOperation();
        } else if (currentLexeme.lexemeType() == LexemeType.DELIMITER && currentLexeme.value().equals(";")) {
            lexemeIterator.previous();
        } else {
            throw new InvalidLexemeException("Ожидался символ арифметической операции на позиции: " + currentLexeme.startIndex(), currentLexeme.startIndex());
        }
        if (operation.equals("+")) postfixList.writeCommand(Command.ADD);
        else if (operation.equals("-")) postfixList.writeCommand(Command.SUB);
        else if (operation.equals("*")) postfixList.writeCommand(Command.MUL);
        else if (operation.equals("/")) postfixList.writeCommand(Command.DIV);
    }


    private void checkCondition() {
        checkRelationExpression();
        checkCondition2();
    }

    private void checkCondition2() {
        if (!lexemeIterator.hasNext()) {
            return;
        }
        currentLexeme = lexemeIterator.next();
        if (currentLexeme.lexemeType() == LexemeType.OR || currentLexeme.lexemeType() == LexemeType.AND) {
            checkRelationExpression();
            Command command = switch (currentLexeme.lexemeType()) {
                case OR -> Command.OR;
                case AND -> Command.AND;
                default ->
                        throw new InvalidLexemeException("Неизвестная логическая операция", currentLexeme.startIndex());
            };
            postfixList.writeCommand(command);
            checkCondition();
        } else {
            lexemeIterator.previous();
        }
    }


    private void checkRelationExpression() {
        checkOperand();
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
            if (currentLexeme.lexemeType() == LexemeType.RELATION) {
                Command command;
                switch (currentLexeme.value()) {
                    case ">" -> command = Command.CMPG;
                    case "<" -> command = Command.CMPL;
                    case "==" -> command = Command.CMPE;
                    case "<>" -> command = Command.CMPNE;
                    default -> throw new
                            InvalidLexemeException(LexemeType.RELATION.toString(),
                            currentLexeme.startIndex());
                }
                checkOperand();
                postfixList.writeCommand(command);
            } else {
                lexemeIterator.previous();
            }
        }
    }

    private void checkOperand() {
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
            if (currentLexeme.lexemeCategory() != LexemeCategory.CONSTANT && currentLexeme.lexemeCategory() != LexemeCategory.IDENTIFIER) {
                throw new InvalidLexemeException(LexemeCategory.IDENTIFIER, LexemeCategory.CONSTANT, currentLexeme.startIndex());
            }
            if (currentLexeme.lexemeCategory() == LexemeCategory.CONSTANT) {
                postfixList.writeConst(Integer.parseInt(currentLexeme.value()));
            } else {
                postfixList.writeVar(currentLexeme.value());
            }
        } else {
            throw new InvalidLexemeException(LexemeCategory.IDENTIFIER, LexemeCategory.CONSTANT, currentLexeme.startIndex()
            );
        }
    }

    private void checkThen() {
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
            if (currentLexeme.lexemeType() != LexemeType.THEN) {
                throw new InvalidLexemeException("Ожидался then", currentLexeme.startIndex());
            }
        } else {
            throw new InvalidLexemeException("Ожидался then", currentLexeme.startIndex());
        }
    }


    private void goNextIfPossibleStart() {
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
        } else {
            throw new InvalidLexemeException("Ожидался if в начале программы", 0);
        }
    }

    private void goNextIfPossibleEndAfterStart() {
        if (lexemeIterator.hasNext()) {
            currentLexeme = lexemeIterator.next();
        }else if(currentLexeme.lexemeType() == LexemeType.END){}
        else {
            throw new InvalidLexemeException("Ожидалось ключевое слово end для завершения программы", currentLexeme.endIndex() + 1);
        }
    }
}
