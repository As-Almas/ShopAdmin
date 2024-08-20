#include "pch.h"
#include "DB_create.h"
#include "AppParams.h"

#include <string>

enum DB_types
{
	ASDB,
	JSON,
};

enum ARCH_types {
	NO,
	ASPACK,
};

enum KEY_types {
	NOKEY,
};

int  CreateDBFile(const char* path, int type, int encode, int keyType, int archType, const char* key, const char* password, const char* date)
{
	DWORD fFlags = CREATE_ALWAYS;
	if (IsFileExist(path)) {

		int MBresult = MessageBoxA(NULL, "Файл уже существует. Все данные файла будут утеряны при продолжении операции. \r\n Вы уверены?", "INFO !@#!", MB_OKCANCEL | MB_ICONINFORMATION);
		if (MBresult != IDOK) {
			MessageBoxA(NULL, "Операция успешно отменена!", "INFO @@@@", MB_OK);
			return 0;
		}
		else
			fFlags = TRUNCATE_EXISTING;
	}
	using namespace std;
	unsigned int configTable[5] = { type, encode, keyType, archType, 0};
	
	switch (keyType)
	{
	case NOKEY: break;
	}

	string db_name = path;
	db_name = db_name.substr(db_name.find_last_of("\\")+1);
	db_name = db_name.substr(0, db_name.find_last_of("."));
	configTable[4] = (unsigned int) db_name.length();
	HANDLE file = CreateFileA(
		path,
		GENERIC_WRITE,
		FILE_SHARE_READ,
		0, fFlags, 0, 0
	);

	if(file == INVALID_HANDLE_VALUE)
		return 0;

	WriteFile(file, configTable, 7 * sizeof(unsigned int), 0, 0);
	WriteFile(file, db_name.c_str(), db_name.length() * sizeof(char), 0, 0);

	CloseHandle(file);
	return 1;
}

bool CheckDBList(const char* ListPath, const char* libPath) {
	if (!IsFileExist(ListPath))
		return false;
	HANDLE list = OpenDBList(ListPath);
	char** ListArr = NULL;
	unsigned int arrLength = 0;
	if (!GetArrayFromList(list, &ListArr, &arrLength)) {
		CloseList(list);
		return false;
	}

	CloseList(list);
	if (arrLength == 0 || ListArr == 0)
		return false;


	for (unsigned int i = 0; i < arrLength; i++) {
		if (strncmp(libPath, ListArr[i], lstrlenA(ListArr[i])) == 0)
		{
			FreeArray(&ListArr, arrLength);
			return true;
		}
	}

	FreeArray(&ListArr, arrLength);
	return false;
}

int AddDBToListF(const char* ListPath, const char* libPath)
{
	if (CheckDBList(ListPath, libPath))
		return 0;

	HANDLE hFile = CreateFileA(
		ListPath,
		GENERIC_WRITE,
		FILE_SHARE_READ,
		0, OPEN_ALWAYS, 0, 0
	);

	if (hFile == INVALID_HANDLE_VALUE)
		return 0;
	SetFilePointer(hFile, 0, 0, FILE_END);

	using namespace std;
	string str = libPath;
	size_t pathLength = str.length();
	WriteFile(hFile, &pathLength, sizeof(size_t), 0, 0);
	WriteFile(hFile, str.c_str(), str.length() * sizeof(char), 0, 0);

	CloseHandle(hFile);
	
	return 1;
}
