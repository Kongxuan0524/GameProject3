#ifndef __WS_PLAYER_MANAGER_OBJECT_H__
#define __WS_PLAYER_MANAGER_OBJECT_H__
#include "GameDefine.h"
#include "Utility/AVLTree.h"
#include "Utility/Position.h"
#include "PlayerObject.h"

class CPlayerManager : public AVLTree<UINT64, CPlayerObject>
{
public:
	CPlayerManager()
	{

	}

	~CPlayerManager()
	{

	}



public:
	static CPlayerManager* GetInstancePtr();

public:
	CPlayerObject*		CreatePlayer(UINT64 u64RoleID, UINT32 RoleType, string RoleName);

	CPlayerObject*		GetPlayer(UINT64 u64RoleID);

	CPlayerObject*		CreatePlayerByID(UINT64 u64RoleID);

	BOOL			    ReleasePlayer(UINT64 u64RoleID);

	BOOL				DeletePlayer(UINT32 u64RoleID);



public:
};

#endif //__WS_PLAYER_OBJECT_H__