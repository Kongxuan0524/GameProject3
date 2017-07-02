using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using Protocol;
using ProtoBuf;

public class NetworkManager : GTSingleton<NetworkManager>
{
    private NetworkClient                                mClient;
    private bool                                         mAlone = true;//单机版
    private static Dictionary<MessageID, NetworkHandler> mMessageDispatchs = new Dictionary<MessageID, NetworkHandler>();
    private static List<MessageRecv>                     mRecvPackets = new List<MessageRecv>();
    private static List<MessageSend>                     mSendPackets = new List<MessageSend>();
    private MessageCenter                                mMessageCenter  = new MessageCenter();

    public override void Init()
    {
        mMessageCenter.AddHandlers();
    }

    public void Execute()
    {
        lock (mRecvPackets)
        {
            if (mRecvPackets.Count > 0)
            {
                for (int i = 0; i < mRecvPackets.Count; i++)
                {
                    DispatchPacket(mRecvPackets[i]);
                }
                mRecvPackets.Clear();
            }
        }
        lock (mSendPackets)
        {
            if (mSendPackets.Count > 0)
            {
                for (int i = 0; i < mSendPackets.Count; i++)
                {
                    Send(mSendPackets[i]);
                }
                mSendPackets.Clear();
            }
        }
    }

    public void Connect(string ip, int port)
    {
        mClient = new NetworkClient(ip, port);
        mClient.Connect();
    }

    public void Send<T>(MessageID messageID, T obj)
    {
        if(mAlone)
        {
            byte[] bytes = null;
            Pack(messageID, obj, ref bytes);
            Recv(mClient, bytes);
        }
        else
        {
            if (mClient == null)
            {
                return;
            }
            Send(mClient, messageID, obj);
        }
    }

    public void Close()
    {
        if (mClient != null)
        {
            mClient.Close();
            mClient = null;
        }   
    }

    public bool Alone
    {
        get { return mAlone; }
        set { mAlone = value; }
    }

    public static void Recv(NetworkClient client, byte[] bytes)
    {
        MemoryStream ms = new MemoryStream(bytes);
        MessagePacket pack = Serializer.Deserialize<MessagePacket>(ms);
        MessageRecv evet = new MessageRecv();
        evet.Packet = pack;
        evet.Client = client;
        lock (mRecvPackets)
        {
            mRecvPackets.Add(evet);
        }
    }

    public static void AddListener(MessageID id, NetworkHandler handle)
    {
        NetworkHandler h = null;
        mMessageDispatchs.TryGetValue(id, out h);
        if (h == null)
        {
            mMessageDispatchs.Add(id, handle);
        }
        else
        {
            mMessageDispatchs[id] += handle;
        }
    }

    public static void DelListener(MessageID id, NetworkHandler handle)
    {
        NetworkHandler h = null;
        mMessageDispatchs.TryGetValue(id, out h);
        if (h != null)
        {
            mMessageDispatchs[id] -= handle;
        }
    }

    static void DispatchPacket(MessageRecv recv)
    {
        NetworkHandler handle = null;
        mMessageDispatchs.TryGetValue(recv.Packet.ID, out handle);
        if (handle != null)
        {
            handle.Invoke(recv, recv.Packet.RetCode);
        }
    }

    static void Send(MessageSend msg)
    {
        msg.Client.Send(msg.Bytes);
    }

    static void Send<T>(NetworkClient client, MessageID messageID, T obj)
    {
        byte[] bytes = null;
        Pack<T>(messageID, obj, ref bytes);
        MessageSend tab = new MessageSend(client, bytes);
        mSendPackets.Add(tab);
    }

    static void Pack<T>(MessageID messageID, T obj, ref byte[] bytes)
    {
        MemoryStream byteMs = new MemoryStream();
        Serializer.Serialize<T>(byteMs, obj);
        byteMs.Seek(0, SeekOrigin.Begin);

        MessagePacket pack = new MessagePacket();
        pack.ID = messageID;
        pack.Data = byteMs.ToArray();
        pack.Auth = string.Empty;
        pack.RetCode = 0;
        MemoryStream ms = new MemoryStream();
        Serializer.Serialize(ms, pack);

        bytes = ms.ToArray();
    }
}