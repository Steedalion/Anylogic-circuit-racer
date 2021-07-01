/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package tcpclient;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author steed
 */
public class TCPSender {

    public Socket clientSocket;
    BufferedReader inputBuff;
    BufferedWriter outputBuff;
    String hostName;
    int hostPort;

    public TCPSender(String hostName, int hostPort) {
        this.hostName = hostName;
        this.hostPort = hostPort;
    }
    
    public void Connect()
    {
        try {
            clientSocket=new Socket(hostName, hostPort);
            inputBuff = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            outputBuff = new BufferedWriter(new OutputStreamWriter(clientSocket.getOutputStream()));
        } catch (IOException ex) {
            Logger.getLogger(TCPSender.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public String ReadLine() 
    {
        try {
            return inputBuff.readLine();
        } catch (IOException ex) {
            Logger.getLogger(TCPSender.class.getName()).log(Level.SEVERE, null, ex);
        }
        return null;
    }
    
    public void WriteLine(String msg)
    {
        try {
            outputBuff.write(msg+"\n");
            outputBuff.flush();
        } catch (IOException ex) {
            Logger.getLogger(TCPSender.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    

}
