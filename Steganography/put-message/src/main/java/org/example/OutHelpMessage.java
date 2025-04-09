package org.example;

public class OutHelpMessage {

    public static void outHelpMessage() {
        System.out.println("""
                The program embeds the message into the container in the desired manner and creates a stegocontainer.
                
                Message – sequence of bytes
                Container – text in UTF-8 encoding.
                Stegocontainer is UTF-8 encoded text containing an embedded message.
                
                Input parameters:
                -m and --message – path to the file containing the message. If not specified, the message is sent via standard input.
                -s and --stego – the path to which the stego container should be written. If not specified, the result is printed to the standard output stream.
                -c and --container – path to the container file. Required parameter!
                -h and --help – display help about how to use the program and what it does
                
                Launch examples:
                put-message -m message.txt -s stegocontainer.txt -c container.txt
                put-message -c container.txt < message.txt > stegocontainer.txt""");
    }

}
