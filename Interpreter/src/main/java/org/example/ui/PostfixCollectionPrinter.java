package org.example.ui;

import com.jakewharton.fliptables.FlipTable;
import org.example.polis.PostfixEntry;
import org.example.polis.PostfixList;

import java.util.ArrayList;
import java.util.List;

public class PostfixCollectionPrinter {
    public static void printPostfixEntryTable(PostfixList postfixList) {
        List<String> headers = new ArrayList<>(List.of("ПОЛИЗ", "Индекс"));
        List<PostfixEntry<?>> postfixEntries = postfixList.getPostfixList();
        List<String[]> data = new ArrayList<>(postfixEntries.size());
        for (int i = 0; i < postfixEntries.size(); i++) {
            String[] row = new String[2];
            PostfixEntry<?> entry = postfixEntries.get(i);
            row[0] = entry.getValue() != null ? entry.getValue().toString() : "null";
            row[1] = Integer.toString(i);
            data.add(row);
        }
        System.out.println(FlipTable.of(headers.toArray(new String[0]), data.toArray(new String[0][])));
    }
}


