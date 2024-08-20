#include "pch.h"
#include "AppParams.h"
#include <memory>
#include <vector>

struct ListArray {
	unsigned int arrLength = 0;
	unsigned int currElement = 0;
	char** Array;
};

void* OpenDBList(const char* listPath)
{
	if (!IsFileExist(listPath))
		return 0;
	HANDLE hFile = CreateFileA(
		listPath,
		GENERIC_READ,
		FILE_SHARE_READ,
		0, OPEN_EXISTING, 0, 0
	);

	if (hFile == INVALID_HANDLE_VALUE)
		return 0;

	return hFile;
}

bool GetArrayFromList(void* list, void* arr, unsigned int* arrLen)
{
	if (list == 0)
		return false;
	unsigned int length = 0u;
	DWORD readLength = 0;
	void* chArr = 0;
	SetFilePointer(list, 0, 0, FILE_BEGIN);
	while (true)
	{
		size_t pLength = 0;

		ReadFile(list, &pLength, sizeof(size_t), &readLength, 0);
		if (readLength == 0u)
			break;
		SetFilePointer(list, pLength, 0, FILE_CURRENT);
		length++;
	}
	if (length == 0)
		return false;
	chArr = (char**)calloc(length, sizeof(void*));
	if (chArr == NULL) {
		list = 0;
		arr = 0;
		return false;
	}
	ZeroMemory(chArr, sizeof(void*) * length);
	*arrLen = length;
	SetFilePointer(list, 0, 0, FILE_BEGIN);
	while (true)
	{
		if (length <= 0)
			break;
		size_t pLength = 0;

		ReadFile(list, &pLength, sizeof(size_t), &readLength, 0);
		if (readLength == 0)
			break;
		((char**)chArr)[length - 1] = (char*)calloc(pLength + 1, sizeof(char));
		ReadFile(list, ((char**)chArr)[length-1], pLength, &readLength, 0);
		((char*) ((char**)chArr)[length-1])[pLength] = '\0';
		if (readLength == 0)
			break;
		length--;
	}
	(*(char***)arr) = (char**)chArr;

	return true;
}

void* GetStructPointerFromList(void* List)
{
	ListArray* struc = new ListArray;
	struc->currElement = 0;
	if (!GetArrayFromList(List, &struc->Array, &struc->arrLength)) {
		delete struc;
		return NULL;
	}

	return struc;
}

char* GetNextStructElement(void* pointer)
{
	if (pointer == 0)
		return 0;
	ListArray* struc = (ListArray*)pointer;
	struc->currElement++;
	if (struc->currElement >= struc->arrLength)
		struc->currElement = 0;

	return struc->Array[struc->currElement];
}

char* GetCurrStructElement(void* pointer)
{

	if (pointer == 0)
		return 0;
	ListArray* struc = (ListArray*)pointer;
	if (struc->currElement >= struc->arrLength)
		struc->currElement = 0;

	return struc->Array[struc->currElement];
}

char* GetPrevStructElement(void* pointer)
{
	if (pointer == 0)
		return 0;
	ListArray* struc = (ListArray*)pointer;
	struc->currElement--;
	if (struc->currElement >= struc->arrLength || struc->currElement < 0)
		struc->currElement = struc->arrLength - 1;

	return struc->Array[struc->currElement];
}

char* GetNStructElement(void* pointer, unsigned int elID)
{
	if (pointer == 0)
		return 0;
	ListArray* struc = (ListArray*)pointer;
	if (elID >= struc->arrLength)
		return 0;
	return struc->Array[elID];
}

unsigned int GetStructLength(void* pointer)
{
	if (pointer == 0)
		return 0;

	ListArray* struc = (ListArray*)pointer;
	return struc->arrLength;
}

void FreeStruct(void* struc)
{
	ListArray* StrList = (ListArray*)struc;

	FreeArray(&StrList->Array, StrList->arrLength);
	delete StrList;
}

void FreeArray(void* arr, unsigned int arrLength)
{
	if (arr == 0 || arrLength == 0)
		return;
	char** chArr = *((char***)arr);
	
	for (unsigned int i = 0; i < arrLength; i++) {
		free(chArr[i]);
	}
	free(chArr);
	(*(char***)arr) = 0;
}

void CloseList(void* list)
{
	CloseHandle(list);
}
