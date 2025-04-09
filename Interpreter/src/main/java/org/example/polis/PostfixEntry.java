package org.example.polis;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@AllArgsConstructor
public class PostfixEntry<T> {
    private EntryType entryType;
    private T value;
}
