package org.example;

import java.io.IOException;
import java.io.InputStream;
import java.math.BigInteger;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Stegocontainer {

    public static byte[] decodeMessageFromStegocontainer(List<String> args) throws IOException {
        if(getStegoKeywordIndex(args)!=-1) {
            List<String> lines = getAllLinesFromStegocontainer(Path.of(args.get(getStegoKeywordIndex(args) + 1)));
            StringBuilder message = new StringBuilder();
            for (int i = lines.size()-1; !lines.get(i).endsWith("  ") ; i--)  {
                if(lines.get(i).isEmpty()){
                    message.insert(0,"0");
                }else {
                    if (lines.get(i).endsWith(" ")) {
                        message.insert(0,"1");
                    } else {
                        message.insert(0,"0");
                    }
                }
            }
            if (message.charAt(0) == '1') {
                message.insert(0, '-');
            }
            byte[] bytes = new BigInteger(message.toString(), 2).toByteArray();;
            return bytes;
        }else{
            return new byte[0];
        }
    }

    public static List<String> getAllLinesFromStegocontainer(Path container) throws IOException {
        return Files.readAllLines(container);
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
