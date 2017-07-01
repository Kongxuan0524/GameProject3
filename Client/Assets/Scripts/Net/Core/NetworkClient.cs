using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;
using Protocol;

public class NetworkClient
{
    private TcpClient       mTcp;
    private string          mIP;
    private int             mPort;
    private bool            mIsConnect = false;
    private Thread          mRecvThread;
    private byte[]          mByteBuffer = new byte[4096];
    private EventWaitHandle mAllDone;

    public NetworkClient(string ip, int port)
    {
        mTcp    = new TcpClient();
        mIP     = ip;
        mPort   = port;
    }

    public void Connect()
    {
        mTcp.BeginConnect(IPAddress.Parse(mIP), mPort, OnConnect, mTcp);
    }

    public void Send(byte [] bytes)
    {
        if(mIsConnect==false)
        {
            OnError(MessageRetCode.TYPE_DISCONNECT);
            return;
        }
        try
        {
            mTcp.Client.Send(bytes);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    public void Close()
    {
        OnClose();
    }

    void OnConnect(IAsyncResult ar)
    {
        if (mTcp.Client == null)
        {
            OnError(MessageRetCode.TYPE_DISCONNECT);
            return;
        }
        if (!mTcp.Client.Connected)
        {
            OnError(MessageRetCode.TYPE_DISCONNECT);
            return;
        }
        mIsConnect = true;
        mAllDone = new EventWaitHandle(false, EventResetMode.AutoReset);
        mRecvThread = new Thread(OnReceive);
        mRecvThread.Start();
    }

    void OnReceive()
    {
        while (true)
        {
            Thread.Sleep(20);
            if (mTcp == null || mTcp.Connected == false)
            {
                OnError(MessageRetCode.TYPE_DISCONNECT);
                return;
            }
            NetworkStream stream = mTcp.GetStream();
            if (stream.CanRead)
            {
                stream.BeginRead(mByteBuffer, 0, mByteBuffer.Length, new AsyncCallback(OnAsyncRead), stream);
                mAllDone.WaitOne();
            }
        }
    }

    void OnAsyncRead(IAsyncResult ar)
    {
        NetworkStream stream = (NetworkStream)ar.AsyncState;
        int len = stream.EndRead(ar);
        byte[] bytes = new byte[len];
        Array.Copy(mByteBuffer, bytes, len);
        NetworkManager.Recv(this, bytes);
        mAllDone.Set();
    }

    void OnError(MessageRetCode retCode)
    {
        if(retCode == MessageRetCode.TYPE_OK)
        {
            return;
        }
        Debug.LogError(retCode.ToString());
        OnClose();
    }

    void OnClose()
    {
        mIsConnect = false;
        if (mRecvThread != null)
        {
            mRecvThread.Abort();
            mRecvThread = null;
        }
        if (mTcp != null && mTcp.Connected)
        {
            mTcp.Close();
        }
        mTcp = null;
    }

}
