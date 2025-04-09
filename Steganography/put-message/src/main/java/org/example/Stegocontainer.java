package org.example;

import java.io.*;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.nio.file.StandardOpenOption;
import java.util.List;

import static org.example.Message.getMessageAsBitsString;

public class Stegocontainer {

    public static void encodeMessageInStegocontainer(List<String> args) throws IOException {
        if(getStegoKeywordIndex(args) != -1) {
            Files.copy(Container.getPathToContainerContainer(args), Path.of(args.get(getStegoKeywordIndex(args)+1)), StandardCopyOption.REPLACE_EXISTING);
            Files.write(Path.of(args.get(getStegoKeywordIndex(args)+1)),Message.writeBitesToFile(getMessageAsBitsString(args),args), StandardOpenOption.APPEND);
        }
    }

    public static int getStegoKeywordIndex(List<String> args) {
        if(args.contains("--stego")) {
            return args.indexOf("--stego");
        } else if (args.contains("-s")) {
            return args.indexOf("-s");
        }
        return -1;
    }
}
