package org.example;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;

public class Container {

    public static Path getPathToContainerContainer(List<String> args) throws IOException {
        return Path.of(args.get(getContainerKeywordIndex(args)+1));
    }

    public static int getContainerKeywordIndex(List<String> args) {
        if(args.contains("--container")) {
            return args.indexOf("--container");
        } else if (args.contains("-c")) {
            return args.indexOf("-c");
        }
        return -1;
    }

}
