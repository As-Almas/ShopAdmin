#pragma once

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

	__declspec(dllexport)  char* GetDBSizeStr(const char*pathDB);
	__declspec(dllexport)  char* GetDBCreateDate(const char* pathDB);
	__declspec(dllexport)  char* GetDBChangeDate(const char* pathDB);
#ifdef __cplusplus
};
#endif // __cplusplus