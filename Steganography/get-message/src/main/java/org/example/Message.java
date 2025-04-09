package org.example;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.List;

public class Message {

    public static void writeBytes(List<String> args) throws IOException {
        if(getMessageKeywordIndex(args) != -1) {
            Files.write(Path.of(args.get(getMessageKeywordIndex(args)+1)),Stegocontainer.decodeMessageFromStegocontainer(args), StandardOpenOption.CREATE);
        }
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
