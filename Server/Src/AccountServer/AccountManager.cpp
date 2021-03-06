﻿#include "stdafx.h"
#include "AccountManager.h"
#include "Utility\CommonFunc.h"

CAccountObjectMgr::CAccountObjectMgr()
{

}

CAccountObjectMgr::~CAccountObjectMgr()
{

}

BOOL CAccountObjectMgr::InitManager()
{
	return TRUE;
}

CAccountObject* CAccountObjectMgr::GetAccountObjectByID( UINT64 AccountID )
{
	return GetByKey(AccountID);
}

CAccountObject* CAccountObjectMgr::CreateAccountObject(std::string strName, std::string strPwd, UINT32 dwChannel)
{
	m_u64MaxID += 1;

	CAccountObject *pObj = InsertAlloc(m_u64MaxID);
	if(pObj == NULL)
	{
		ASSERT_FAIELD;
		return NULL;
	}

	pObj->m_bEnabled = TRUE;
	pObj->m_strName = strName;
	pObj->m_strPassword = strPwd;
	pObj->m_ID = m_u64MaxID;
	pObj->m_dwChannel = dwChannel;
	pObj->m_dwCreateTime = CommonFunc::GetCurrTime();


	return pObj;
}

BOOL CAccountObjectMgr::ReleaseAccountObject(UINT64 AccountID )
{
	return Delete(AccountID);
}

BOOL CAccountObjectMgr::AddAccountObject(UINT64 u64ID, std::string strName, std::string strPwd, UINT32 dwChannel)
{
	CAccountObject *pObj = InsertAlloc(u64ID);
	if(pObj == NULL)
	{
		ASSERT_FAIELD;
		return NULL;
	}

	pObj->m_bEnabled = TRUE;
	pObj->m_strName = strName;
	pObj->m_strPassword = strPwd;
	pObj->m_ID = u64ID;
	pObj->m_dwChannel = dwChannel;

	m_mapNameObj.insert(std::make_pair(strName, pObj));

	return TRUE;
}

CAccountObject* CAccountObjectMgr::GetAccountObjectByName(std::string name)
{
	std::map<std::string, CAccountObject*>::iterator itor = m_mapNameObj.find(name);
	if(itor != m_mapNameObj.end())
	{
		return itor->second;
	}

	return NULL;
}
