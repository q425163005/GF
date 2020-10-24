using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using UnityEngine;

namespace Fuse
{
    public class ConfigUtility
    {
        const char split_default = ';';
        const char split_item = '_';
        const char split_item_multi = '#';
        public static T GetValue<T>(string str)
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }


        public static int GetValue_int(string str)
        {
            return int.Parse(str);
        }

        public static List<int[]> GetListArray(string str)
        {
            List<int[]> rtn = new List<int[]>();
            string[] list = GetArray<string>(str);
            for (int i = 0; i < list.Length; i++)
            {
                int[] value = GetArray<int>(list[i], split_item);
                rtn.Add(value);
            }
            return rtn;
        }

        public static List<List<int[]>> GetListListArray(string str)
        {
            List<List<int[]>> rtn = new List<List<int[]>>();
            string[] list = GetArray<string>(str, split_item_multi);
            List<int[]> lt2;
            for (int i = 0; i < list.Length; i++)
            {
                string[] list2 = GetArray<string>(list[i], split_default);
                lt2 = new List<int[]>();
                rtn.Add(lt2);
                for (int j = 0; j < list2.Length; j++)
                {
                    int[] value = GetArray<int>(list2[i], split_item);
                    lt2.Add(value);
                }
            }
            return rtn;
        }
        #region 获取数组 T[]
        /// <summary>通用方法</summary>
        public static T[] GetArray<T>(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
            {
                return new T[0];
            }
            string[] objarr = str.Split(separator);
            T[] rtn = new T[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = GetValue<T>(objarr[i]);
            return rtn;
        }

        public static byte[] GetArray_byte(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new byte[0];
            string[] objarr = str.Split(separator);
            byte[] rtn = new byte[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = byte.Parse(objarr[i]);
            return rtn;
        }

        public static short[] GetArray_short(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new short[0];
            string[] objarr = str.Split(separator);
            short[] rtn = new short[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = short.Parse(objarr[i]);
            return rtn;
        }

        public static int[] GetArray_int(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new int[0];
            string[] objarr = str.Split(separator);
            int[] rtn = new int[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = int.Parse(objarr[i]);
            return rtn;
        }
        public static long[] GetArray_long(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new long[0];
            string[] objarr = str.Split(separator);
            long[] rtn = new long[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = long.Parse(objarr[i]);
            return rtn;
        }
        public static float[] GetArray_float(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new float[0];
            string[] objarr = str.Split(separator);
            float[] rtn = new float[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = float.Parse(objarr[i]);
            return rtn;
        }
        public static double[] GetArray_double(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new double[0];
            string[] objarr = str.Split(separator);
            double[] rtn = new double[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = double.Parse(objarr[i]);
            return rtn;
        }

        public static bool[] GetArray_bool(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new bool[0];
            string[] objarr = str.Split(separator);
            bool[] rtn = new bool[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = bool.Parse(objarr[i]);
            return rtn;
        }

        public static string[] GetArray_string(string str, char separator = split_default)
        {
            if (str == string.Empty || str == null)
                return new string[0];
            string[] objarr = str.Split(separator);
            string[] rtn = new string[objarr.Length];
            for (int i = 0; i < objarr.Length; i++)
                rtn[i] = objarr[i];
            return rtn;
        }
        #endregion

        static ILRuntime.Runtime.Enviorment.AppDomain appDomain;
        public unsafe static void RegisterILRuntimeCLRRedirection(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            appDomain = appdomain;
            foreach (var i in typeof(ConfigUtility).GetMethods())
            {
                if (i.Name == "ToObject" && i.IsGenericMethodDefinition)
                {
                    //var param = i.GetParameters();
                    appdomain.RegisterCLRMethodRedirection(i, ILToObject);
                }
            }
        }


        public unsafe static StackObject* ILToObject(ILIntepreter intp, StackObject* esp, IList<object> mStack, CLRMethod method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(esp, 1);
            string[] args = (string[])typeof(string[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, mStack));
            intp.Free(ptr_of_this_method);
            var type = method.GenericArguments[0].ReflectionType;
            var result_of_this_method = ReadValue(type, args, __domain);
            return ILIntepreter.PushObject(__ret, mStack, result_of_this_method);
        }

        public static object ToObject(Type type, string[] args)
        {
            return ReadValue(type, args, appDomain);
        }

        public static T ToObject<T>(string[] args)
        {
            return (T)ReadValue(typeof(T), args);
        }

        public static T ToObjectWithEditor<T>(string[] args)
        {
            return (T)ReadValue(typeof(T), args, null, true);
        }


        private static IDictionary<Type, ObjectMetadata> object_metadata = new Dictionary<Type, ObjectMetadata>();
        private static object ReadValue(Type type, string[] args, ILRuntime.Runtime.Enviorment.AppDomain domain = null, bool isEditor = false)
        {
            ObjectMetadata info;
            if (!object_metadata.TryGetValue(type, out info))
            {
                info = new ObjectMetadata(type);
                object_metadata.Add(type, info);
            }
            object instance;
            if (domain != null)
                instance = domain.Instantiate(type.FullName);
            else
                instance = Activator.CreateInstance(type);


            ObjectFieldMetadata field;
            object value;
            for (int i = 0; i < info.FieldInfo.Length; i++)
            {
                value = null;
                field = info.FieldInfo[i];
                if (field.IsListListArray)
                    value = GetListListArray(args[i]);
                else if (field.IsListArray)
                    value = GetListArray(args[i]);
                else if (field.IsLang)
                {
                    if (domain != null)
                        value = domain.Instantiate(field.TypeName, new object[] { args[i] });
                    else
                    {
                        if (isEditor)
                        {
                            //value = Activator.CreateInstance(typeof(MapEditor.Lang), new object[] { args[i] });
                        }
                        else
                        {
#if REFLECT
                            value = Activator.CreateInstance(Mgr.ILR.GetAssemblyType(field.TypeName), new object[] { args[i] });
#endif
                        }
                    }
                }
                else if (field.IsArray)
                {
                    switch (field.TypeName)
                    {
                        case "Int32[]": value = GetArray_int(args[i]); break;
                        case "Byte[]": value = GetArray_byte(args[i]); break;
                        case "Int16[]": value = GetArray_short(args[i]); break;
                        case "Int64[]": value = GetArray_long(args[i]); break;
                        case "Single[]": value = GetArray_float(args[i]); break;
                        case "Double[]": value = GetArray_double(args[i]); break;
                        case "Boolean[]": value = GetArray_bool(args[i]); break;
                        case "String[]": value = GetArray_string(args[i]); break;
                    }
                }
                else
                {
                    switch (field.TypeName)
                    {
                        case "Int32": value = Int32.Parse(args[i]); break;
                        case "Single": value = Single.Parse(args[i]); break;
                        case "String": value = args[i]; break;
                        case "Boolean": value = Boolean.Parse(args[i]); break;
                        case "Double": value = Double.Parse(args[i]); break;
                        case "Byte": value = Byte.Parse(args[i]); break;
                        case "Int16": value = Int16.Parse(args[i]); break;
                        case "Int64": value = Int64.Parse(args[i]); break;
                        case "DateTime": value = DateTime.Parse(args[i]); break;
                    }

                }
                field.FieldInfo.SetValue(instance, value);
            }
            return instance;
        }
    }

    internal struct ObjectMetadata
    {
        public ObjectFieldMetadata[] FieldInfo;
        public ObjectMetadata(Type type)
        {
            var fields = type.GetFields();
            FieldInfo = new ObjectFieldMetadata[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                FieldInfo[i] = new ObjectFieldMetadata(fields[i]);

        }
    }

    internal struct ObjectFieldMetadata
    {
        public FieldInfo FieldInfo;
        public bool IsArray;         //object[]
        public bool IsListArray;    //List<int[]>
        public bool IsListListArray;//List<List<int[]>>
        public string TypeName;
        public bool IsLang;
        public ObjectFieldMetadata(FieldInfo field)
        {
            FieldInfo = field;
            IsListListArray = field.FieldType.UnderlyingSystemType == typeof(List<List<int[]>>);
            IsListArray = field.FieldType.UnderlyingSystemType == typeof(List<int[]>);
            IsArray = field.FieldType.IsArray;
            TypeName = field.FieldType.UnderlyingSystemType.Name;

            //语言特殊处理
            IsLang = field.FieldType.Name == "Lang";
            if (IsLang)
                TypeName = field.FieldType.FullName;
        }
    }
}
