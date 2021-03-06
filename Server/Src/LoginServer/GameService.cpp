﻿#include "stdafx.h"
#include "GameService.h"
#include "CommandDef.h"
#include "Utility/Log/Log.h"
#include "Utility/CommonFunc.h"
#include "Utility/CommonEvent.h"
#include "Utility/CommonThreadFunc.h"
#include "TimerManager.h"

CGameService::CGameService(void)
{

}

CGameService::~CGameService(void)
{
}

CGameService* CGameService::GetInstancePtr()
{
	static CGameService _GameService;

	return &_GameService;
}


BOOL CGameService::Init()
{
	CommonFunc::SetCurrentWorkPath("");

	if(!CLog::GetInstancePtr()->StartLog("LoginServer","log"))
	{
		ASSERT_FAIELD;
		return FALSE;
	}

	CLog::GetInstancePtr()->AddLog("---------服务器开始启动-----------");

	if(!CConfigFile::GetInstancePtr()->Load("servercfg.ini"))
	{
		ASSERT_FAIELD;
		CLog::GetInstancePtr()->AddLog("配制文件加载失败!");
		return FALSE;
	}

	UINT16 nPort = CConfigFile::GetInstancePtr()->GetIntValue("login_svr_port");
	INT32  nMaxConn = CConfigFile::GetInstancePtr()->GetIntValue("login_svr_max_con");
    if(!ServiceBase::GetInstancePtr()->StartNetwork(nPort, nMaxConn,this))
    {
        ASSERT_FAIELD;
        CLog::GetInstancePtr()->AddLog("启动服务失败!");

		return FALSE;
	}

	//ConnectToLogServer();

	ConnectToAccountSvr();

	m_LoginMsgHandler.Init();

	TimerManager::GetInstancePtr()->AddDiffTimer(1, 1, &CGameService::OnTimer, this);

	return TRUE;
}


BOOL CGameService::Uninit()
{
	ServiceBase::GetInstancePtr()->StopNetwork();
	return TRUE;
}

BOOL CGameService::Run()
{
	while(TRUE)
	{
		ServiceBase::GetInstancePtr()->Update();

		CommonThreadFunc::Sleep(1);
	}

	return TRUE;
}


BOOL CGameService::SendCmdToAccountConnection(UINT32 dwMsgID, UINT64 u64TargetID, UINT32 dwUserData,const google::protobuf::Message& pdata)
{
	if(m_dwAccountConnID == 0)
	{
		ASSERT_FAIELD;
		return FALSE;
	}

	ServiceBase::GetInstancePtr()->SendMsgProtoBuf(m_dwAccountConnID, dwMsgID, u64TargetID, dwUserData,pdata);

	return TRUE;
}

BOOL CGameService::OnTimer(UINT32 dwUserData)
{
	if(m_dwAccountConnID == 0)
	{
		ConnectToAccountSvr();
	}

	if(m_dwLogSvrConnID == 0)
	{
		//ConnectToLogServer();
	}

	return TRUE;
}

BOOL CGameService::ConnectToLogServer()
{
	UINT32 nStatPort = CConfigFile::GetInstancePtr()->GetIntValue("stat_svr_port");
	std::string strStatIp = CConfigFile::GetInstancePtr()->GetStringValue("stat_svr_ip");
	CConnection *pConnection = ServiceBase::GetInstancePtr()->ConnectToOtherSvr(strStatIp, nStatPort);
	if(pConnection == NULL)
	{
		ASSERT_FAIELD;
		return FALSE;
	}

	m_dwLogSvrConnID = pConnection->GetConnectionID();
	return TRUE;
}

BOOL CGameService::ConnectToAccountSvr()
{
	UINT32 nAccountPort = CConfigFile::GetInstancePtr()->GetIntValue("account_svr_port");
	std::string strAccountIp = CConfigFile::GetInstancePtr()->GetStringValue("account_svr_ip");
	CConnection *pConnection = ServiceBase::GetInstancePtr()->ConnectToOtherSvr(strAccountIp, nAccountPort);
	if(pConnection == NULL)
	{
		ASSERT_FAIELD;
		return FALSE;
	}

	m_dwAccountConnID = pConnection->GetConnectionID();
	return TRUE;
}

BOOL CGameService::OnNewConnect(CConnection *pConn)
{
	CLog::GetInstancePtr()->AddLog("新连接来到!");


	return TRUE;
}

BOOL CGameService::OnCloseConnect(CConnection *pConn)
{
	CLog::GetInstancePtr()->AddLog("断开连接!");

	if(pConn->GetConnectionID() == m_dwAccountConnID)
	{
		m_dwAccountConnID = 0;
		ConnectToAccountSvr();
	}

	if(pConn->GetConnectionID() == m_dwLogSvrConnID)
	{
		m_dwLogSvrConnID = 0;
		//ConnectToLogServer();
	}

	return TRUE;
}

BOOL CGameService::DispatchPacket(NetPacket *pNetPacket)
{
	switch(pNetPacket->m_dwMsgID)
	{
	default:
		{
			m_LoginMsgHandler.DispatchPacket(pNetPacket);
		}
		break;
	}

	return TRUE;
}