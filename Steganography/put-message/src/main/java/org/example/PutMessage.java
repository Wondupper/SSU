package org.example;

import java.io.IOException;
import java.util.List;

public class PutMessage {
    public static void main(String[] args) throws IOException {
        analyseInputArgs(args);
    }

    private static void analyseInputArgs(String[] args) throws IOException {
        List<String> argsList = List.of(args);
        if(argsList.contains("-h") || argsList.contains("--help")) {
            OutHelpMessage.outHelpMessage();
        }else{
            Stegocontainer.encodeMessageInStegocontainer(argsList);
        }
    }

}