#include "pch.h"
#include "ReadDB.h"
#include <sstream>
#include <string>

std::string sizeUnits[] = {
    "byte", 
    "KB",
    "MB",
    "GB"
};

char* GetDBSizeStr(const char* pathDB)
{
    if(!IsFileExist(pathDB))
        return nullptr;
    HANDLE hFile = CreateFileA(
        pathDB,
        GENERIC_READ,
        FILE_SHARE_READ,
        0, OPEN_EXISTING, 0, 0
    );

    if (hFile == INVALID_HANDLE_VALUE)
        return nullptr;

    LARGE_INTEGER fSize;
    if (!GetFileSizeEx(hFile, &fSize)) {
        CloseHandle(hFile);
        return nullptr;
    }
    CloseHandle(hFile);
    double divideModule = 0;
    int sizeUnitID = 0;
    if (fSize.QuadPart >= 1024) {
        sizeUnitID = 1;
        divideModule = (fSize.QuadPart % 1024) / 1024.0;
        fSize.QuadPart = fSize.QuadPart / 1024ll;
    }
    if (fSize.QuadPart >= 1024) {
        sizeUnitID = 2;
        divideModule = (fSize.QuadPart % 1024) / 1024.0;
        fSize.QuadPart = fSize.QuadPart / 1024ll;
    }
    if (fSize.QuadPart >= 1024) {
        sizeUnitID = 3;
        divideModule = (fSize.QuadPart % 1024) / 1024.0;
        fSize.QuadPart = fSize.QuadPart / 1024ll;
    }

    using namespace std;
    stringstream sstring, doubleStr;
    sstring << fSize.QuadPart;
    doubleStr << divideModule;

    string str = doubleStr.str();
    str = str.substr(1);
    if (str.length() > 3) {
        str = str.substr(0, 3);
    }
    doubleStr.clear();

    sstring << str;
    sstring << " " << sizeUnits[sizeUnitID];

    str = sstring.str();

    
    char* strResult = new char[str.length()+1];
    unsigned int i = 0;
    for (char s : str) {
        strResult[i] = s;
        i++;
    }
    strResult[str.length()] = '\0';
    str.clear();
    sstring.clear();

    return strResult;
}

char* GetDBCreateDate(const char* pathDB)
{
    if (!IsFileExist(pathDB))
        return nullptr;

    HANDLE hFile = CreateFileA(
        pathDB,
        GENERIC_READ,
        FILE_SHARE_READ,
        0, OPEN_EXISTING, 0, 0
    );
    if (hFile == INVALID_HANDLE_VALUE)
        return nullptr;
    FILETIME fTime;

    if (!GetFileTime(hFile, &fTime, 0, 0))
        return nullptr;
    SYSTEMTIME sTime;
    FileTimeToSystemTime(&fTime, &sTime);

    using namespace std;
    stringstream timeString;
    if (sTime.wDay < 10)
        timeString << "0";
    timeString << sTime.wDay << ".";
    if (sTime.wMonth < 10)
        timeString << "0";
    timeString << sTime.wMonth << "." << sTime.wYear << " ";

    if (sTime.wHour < 10)
        timeString << "0";
    timeString << sTime.wHour << ":";
    if (sTime.wMinute < 10)
        timeString << "0";
    timeString << sTime.wMinute;


    char* result = new char[timeString.str().length() + 1];
    unsigned int i = 0;
    for (char s : timeString.str()) {
        result[i] = s;
        i++;
    }
    result[timeString.str().length()] = '\0';
    return result;
}

char* GetDBChangeDate(const char* pathDB)
{
    if (!IsFileExist(pathDB))
        return nullptr;

    HANDLE hFile = CreateFileA(
        pathDB,
        GENERIC_READ,
        FILE_SHARE_READ,
        0, OPEN_EXISTING, 0, 0
    );
    if (hFile == INVALID_HANDLE_VALUE)
        return nullptr;
    FILETIME fTime;

    if (!GetFileTime(hFile, 0, 0, &fTime))
        return nullptr;
    SYSTEMTIME sTime;
    FileTimeToSystemTime(&fTime, &sTime);

    using namespace std;
    stringstream timeString;
    if (sTime.wDay < 10)
        timeString << "0";
    timeString << sTime.wDay << ".";
    if (sTime.wMonth < 10)
        timeString << "0";
    timeString << sTime.wMonth << "." << sTime.wYear << " ";

    if (sTime.wHour < 10)
        timeString << "0";
    timeString << sTime.wHour << ":";
    if (sTime.wMinute < 10)
        timeString << "0";
    timeString << sTime.wMinute;

    char* result = new char[timeString.str().length() + 1];
    unsigned int i = 0;
    for (char s : timeString.str()) {
        result[i] = s;
        i++;
    }
    result[timeString.str().length()] = '\0';
    return result;
}
