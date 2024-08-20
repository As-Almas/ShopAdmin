using System;
using System.Runtime.InteropServices;

namespace ShopAdmin.DB
{
    class NativeKernel
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
    class DB_dll
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CreateDataBase(string path, int type, int encode, int keyType, int archType, string key, string password, string date);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int AddDBToListF(string ListPath, string libPath);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr OpenDBList(string ListPath);

        /*[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool GetArrayFromList(void* list, void* arr, unsigned int* arrLength);*/

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetStructPointerFromList(IntPtr ListPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetNextStructElement(IntPtr StructPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetCurrStructElement(IntPtr StructPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetPrevStructElement(IntPtr StructPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetNStructElement(IntPtr StructPointer, uint elID);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint GetStructLength(IntPtr StructPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeStruct(IntPtr StructPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CloseList(IntPtr ListPointer);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetDBSizeStr(string pathDB);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetDBCreateDate(string pathDB);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetDBChangeDate(string pathDB);




        private IntPtr lib;

        public CreateDataBase createDB;
        public AddDBToListF DBList_addDB;
        public OpenDBList OpenListOfDB;
        public GetStructPointerFromList GetListStPointer;
        public GetNextStructElement GetStNextEl;
        public GetCurrStructElement GetStCurrEl;
        public GetPrevStructElement GetStPrevEl;
        public GetNStructElement GetStNElement;
        public GetStructLength GetStArrLength;
        public FreeStruct FreeListStruct;
        public CloseList CloseLisOfDB;

        public GetDBSizeStr GetDBSizeAsStr;
        public GetDBCreateDate GetDBCreatedDate;
        public GetDBChangeDate GetDBLastChangeDate;





        public DB_dll()
        {
            lib = NativeKernel.LoadLibrary("DataBase.dll");
            if (lib == IntPtr.Zero)
                throw new Exception("ERROR! DataBase.dll not found!");

            IntPtr pAddr = NativeKernel.GetProcAddress(lib, "CreateDBFile");
            createDB = (CreateDataBase)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(CreateDataBase));

            pAddr = NativeKernel.GetProcAddress(lib, "AddDBToListF");
            DBList_addDB = (AddDBToListF)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(AddDBToListF));

            pAddr = NativeKernel.GetProcAddress(lib, "OpenDBList");
            OpenListOfDB = (OpenDBList)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(OpenDBList));

            pAddr = NativeKernel.GetProcAddress(lib, "GetStructPointerFromList");
            GetListStPointer = (GetStructPointerFromList)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetStructPointerFromList));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetStructPointerFromList");
            GetStNextEl = (GetNextStructElement)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetNextStructElement));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetCurrStructElement");
            GetStCurrEl = (GetCurrStructElement)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetCurrStructElement));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetPrevStructElement");
            GetStPrevEl = (GetPrevStructElement)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetPrevStructElement));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetNStructElement");
            GetStNElement = (GetNStructElement)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetNStructElement));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetStructLength");
            GetStArrLength = (GetStructLength)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetStructLength));
            
            pAddr = NativeKernel.GetProcAddress(lib, "FreeStruct");
            FreeListStruct = (FreeStruct)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(FreeStruct));
            
            pAddr = NativeKernel.GetProcAddress(lib, "CloseList");
            CloseLisOfDB = (CloseList)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(CloseList));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetDBSizeStr");
            GetDBSizeAsStr = (GetDBSizeStr)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetDBSizeStr));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetDBCreateDate");
            GetDBCreatedDate = (GetDBCreateDate)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetDBCreateDate));
            
            pAddr = NativeKernel.GetProcAddress(lib, "GetDBChangeDate");
            GetDBLastChangeDate = (GetDBChangeDate)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(GetDBChangeDate));
            
        }

        public void FreeDLL()
        {
            NativeKernel.FreeLibrary(lib);
            createDB = null;
            DBList_addDB = null;
            OpenListOfDB = null;
            GetListStPointer = null;
            GetStNextEl = null;
            GetStCurrEl = null;
            GetStPrevEl = null;
            GetStNElement = null;
            GetStArrLength = null;
            FreeListStruct = null;
            CloseLisOfDB = null;
        }

        ~DB_dll()
        {
        }
    }
}
