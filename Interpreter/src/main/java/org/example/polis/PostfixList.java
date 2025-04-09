package org.example.polis;

import lombok.Getter;

import java.util.ArrayList;
import java.util.List;

@Getter
public class PostfixList implements PostfixCollection {
    private final List<PostfixEntry<?>> postfixList;
    public PostfixList() {
        this.postfixList = new ArrayList<>();
    }
    @Override
    public int writeCommand(Command command) {
        PostfixEntry<Command> postfixEntry = new PostfixEntry<>(EntryType.COMMAND, command);
        return addToList(postfixEntry);
    }
    @Override
    public int writeVar(String name) {
        PostfixEntry<String> postfixEntry = new PostfixEntry<>(EntryType.VAR, name);
        return addToList(postfixEntry);
    }
    @Override
    public int writeConst(Integer value) {
        PostfixEntry<Integer> postfixEntry = new PostfixEntry<>(EntryType.CONST, value);
        return addToList(postfixEntry);
    }
    @Override
    public int writeAddress(Integer address) {
        PostfixEntry<Integer> postfixEntry = new PostfixEntry<>(EntryType.ADDRESS, address);
        return addToList(postfixEntry);
    }
    public int writeEmpty() {
        PostfixEntry<String> postfixEntry = new PostfixEntry<>(EntryType.EMPTY, " ");
        return addToList(postfixEntry);
    }
    @Override
    public void setAddress(int index, Integer address) {
        PostfixEntry<Integer> postfixEntry = (PostfixEntry<Integer>)
                postfixList.get(index);
        postfixEntry.setValue(address);
    }
    public int getNextIndex() {
        return postfixList.size();
    }
    private int addToList(PostfixEntry<?> postfixEntry) {
        postfixList.add(postfixEntry);
        return postfixList.size()- 1;
    }
}