// pch.cpp: файл исходного кода, соответствующий предварительно скомпилированному заголовочному файлу

#include "pch.h"


bool IsFileExist(LPCSTR filePath){
	if (filePath == 0)
		return false;
	DWORD attr = GetFileAttributesA(filePath);
	return (attr != INVALID_FILE_ATTRIBUTES) && !(attr & FILE_ATTRIBUTE_DIRECTORY);
}

bool IsSysFile(LPCSTR filePath) {
	DWORD attr = GetFileAttributesA(filePath);
	return (attr != INVALID_FILE_ATTRIBUTES) && (attr & FILE_ATTRIBUTE_SYSTEM);
}

// При использовании предварительно скомпилированных заголовочных файлов необходим следующий файл исходного кода для выполнения сборки.
