﻿#ifndef _GAME_SERVICE_H_
#define _GAME_SERVICE_H_
#include "ServiceBase.h"
#include "LoginMsgHandler.h"

class CGameService  : public IPacketDispatcher
{
private:
	CGameService(void);
	virtual ~CGameService(void);

public:
	static CGameService* GetInstancePtr();

public:

	BOOL		Init();

	BOOL		Uninit();

	BOOL		Run();

	BOOL		OnNewConnect(CConnection *pConn);

	BOOL		OnCloseConnect(CConnection *pConn);

	BOOL		DispatchPacket( NetPacket *pNetPacket);

	BOOL        SendCmdToAccountConnection(UINT32 nMsgID, UINT64 u64TargetID, UINT32 dwUserData, const google::protobuf::Message& pdata);

	BOOL		OnTimer(UINT32 dwUserData);

	BOOL	    ConnectToLogServer();

	BOOL		ConnectToAccountSvr();

public:
	CLoginMsgHandler	m_LoginMsgHandler;


	UINT32              m_dwLogSvrConnID;
	UINT32              m_dwAccountConnID;
};

#endif
