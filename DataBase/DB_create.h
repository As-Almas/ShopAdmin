#pragma once


#ifdef __cplusplus
extern "C" {
#endif // __cplusplus
	__declspec(dllexport)  int CreateDBFile(const char*path, int type, int encode, int keyType, int archType, const char*key, const char*password, const char*date);
	__declspec(dllexport)  int AddDBToListF(const char* ListPath, const char* libPath);
#ifdef __cplusplus
};
#endif // __cplusplus



