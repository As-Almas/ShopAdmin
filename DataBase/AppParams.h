#pragma once


#ifdef __cplusplus
extern "C" {
#endif // __cplusplus
	__declspec(dllexport)  void*		  OpenDBList(const char* listPath);	
	__declspec(dllexport)  bool			  GetArrayFromList(void* list, void* arr, unsigned int* arrLength);

	__declspec(dllexport)  void*		  GetStructPointerFromList(void*List);
	__declspec(dllexport)  char*		  GetNextStructElement(void* pointer);
	__declspec(dllexport)  char*		  GetCurrStructElement(void* pointer);
	__declspec(dllexport)  char*		  GetPrevStructElement(void* pointer);
	__declspec(dllexport)  char*		  GetNStructElement(void* pointer, unsigned int elID);
	__declspec(dllexport)  unsigned int   GetStructLength(void* pointer);
	__declspec(dllexport)  void			  FreeStruct(void*pointer);

	__declspec(dllexport)  void			  FreeArray(void* arr, unsigned int arrLength);
	__declspec(dllexport)  void			  CloseList(void*list);
#ifdef __cplusplus
};
#endif // __cplusplus

