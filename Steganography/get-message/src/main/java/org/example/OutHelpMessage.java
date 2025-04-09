package org.example;

public class OutHelpMessage {

    public static void outHelpMessage() {
        System.out.println("""
                The program restores the message from the stego container exactly bit for bit.
                
                Message – sequence of bytes
                Stegocontainer is UTF-8 encoded text containing an embedded message.
                
                Input parameters:
                -m and --message – path to the file in which you want to write the message in the form in which it was before embedding. If not specified, the message is printed to standard output..
                -s and --stego – the path to which the stego container should be written. If not specified, the result is printed to the standard output stream.
                -h and --help – display help about how to use the program and what it does
                
                Launch examples:
                get-message -s stegocontainer.txt --message message.txt
                get-message < stegocontainer.txt > message.txt""");
    }

}
