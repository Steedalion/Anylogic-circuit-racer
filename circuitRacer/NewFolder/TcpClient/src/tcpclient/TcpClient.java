/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package tcpclient;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 *
 * @author csteed
 */
public class TcpClient {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {
        // TODO code application logic here
        
        
        try (Socket clientSocket = new Socket("127.0.0.1", 9999)) {
            System.out.println("Connected to"+clientSocket.toString());
 
            InputStream input = clientSocket.getInputStream();
            InputStreamReader reader = new InputStreamReader(input);
            BufferedReader inputBuff = new BufferedReader(reader);
            
            OutputStream output = clientSocket.getOutputStream();
            OutputStreamWriter writer = new OutputStreamWriter(output);
            BufferedWriter writeBufferedWriter = new BufferedWriter(writer);
 
//            int character;
//            StringBuilder data = new StringBuilder();
            String str;
            
 
            while(true)
            {
                str = inputBuff.readLine();
                System.out.println(str);
            }
//            while ((character = reader.read()) != -1) {
//                data.append((char) character);
//            }
 
//            System.out.println(data);
 
 
        } catch (UnknownHostException ex) {
 
            System.out.println("Server not found: " + ex.getMessage());
 
        } catch (IOException ex) {
 
            System.out.println("I/O error: " + ex.getMessage());
        }
        
        
    }
    
}
