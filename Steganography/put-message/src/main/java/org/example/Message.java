package org.example;

import java.io.IOException;
import java.math.BigInteger;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;

public class Message {

    public static StringBuilder getMessageAsBitsString(List<String> args) throws IOException {
        int messageKeywordIndex = getMessageKeywordIndex(args);
        if(messageKeywordIndex != -1) {
            byte [] bytes = Files.readAllBytes(Path.of(args.get(messageKeywordIndex+1)));
            return new StringBuilder(new BigInteger(bytes).toString(2).replace("-",""));
        }else{
            return new StringBuilder();
        }
    }

    public static List<String> writeBitesToFile(StringBuilder bites, List<String> args) throws IOException {
        List<String> lines = new ArrayList<>();
        lines.add("%n  ".formatted());
        for (int i = 0; i < bites.length(); i++) {
            if(bites.charAt(i) == '1'){
                lines.add(" ");
            }else{
                lines.add("");
            }
        }
        return lines;
    }
    
    public static int getMessageKeywordIndex(List<String> args) {
        if(args.contains("--message")) {
            return args.indexOf("--message");
        } else if (args.contains("-m")) {
            return args.indexOf("-m");
        }
        return -1;
    }
}
