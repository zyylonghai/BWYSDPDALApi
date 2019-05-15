using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SDPCRL.CORE
{
    public sealed class LibSysUtils
    {
        public static Int32 ToInt32(object obj)
        {
            Int32 result = 0;

            if (Int32.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                if (IsNULLOrEmpty(obj))
                {
                    return 0;
                }
                else
                {
                    return Int32.MinValue;
                }
            }
            return result;


        }

        public static bool IsNULLOrEmpty(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else
            {
                if (string.Compare(obj.GetType().Name, typeof(string).Name, false) == 0 && obj.ToString() == string.Empty)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static bool ToBooLean(string value)
        {
            string trueValue = ReSourceManage.GetResource(BooleanValue.True);
            if (string.Compare(trueValue, value, true) == 0)
            {
                return true;
            }
            return false;
        }

        public static string BooleanToText(bool value)
        {
            if (value)
            {
                return ReSourceManage.GetResource(BooleanValue.True);
            }
            return ReSourceManage.GetResource(BooleanValue.False);
        }

        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }

        public static T ConvertToEnumType<T>(string enumNm)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (string.Compare(item.ToString(), enumNm) == 0)
                {
                    return item;
                }
            }
            return default(T);
        }

        public static bool isNumberic(string val, out int result)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            result = -1;
            if (rex.IsMatch(val))
            {
                result = Convert.ToInt32(val);
                return true;
            }
            else
                return false;
        }

        //public static object ConvertToEnumType(string enumNm,Type enumType)
        //{

        //}

        private enum BooleanValue
        {
            [LibReSource("是")]
            True = 1,
            [LibReSource("否")]
            False = 0
        }
    }

    public sealed class ReSourceManage
    {
        public static string GetResource(object enumObj)
        {
            Type t = enumObj.GetType();
            FieldInfo field = t.GetField(t.GetEnumName(enumObj));
            object[] attrArray = field.GetCustomAttributes(typeof(LibReSourceAttribute), true);
            LibReSourceAttribute resource = attrArray[0] as LibReSourceAttribute;
            return resource.Resource;
        }
    }
}
