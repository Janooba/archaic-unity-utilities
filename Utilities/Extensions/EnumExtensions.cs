using System;

namespace Archaic.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the count int value of inputed enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetEnumCount<T>()
        {
            int v = Enum.GetValues(typeof(T)).Length;
            return v;
        }

        /// <summary>
        /// Get a random enum value from inputed enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        /// <summary>
        /// Input int get enum./n
        /// EX: _num = 0;/n
        /// MyEnum myEnum = EnumFromInt<MyEnum>(_num);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_i"></param>
        /// <returns></returns>
        public static T EnumFromInt<T>(this int _i)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_i);
        }

        /// <summary>
        /// Gets a random from inputed values./n
        /// EX: MyEnum mE = MyEnum.Defualt/n
        /// mE = RandomValueFrom(mE.A, mE.B, mE.C etc....)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_randomVal"></param>
        /// <returns></returns>
        public static T RandomValueFrom<T>(params T[] _randomVal)
        {
            return (T)_randomVal.GetValue(new System.Random().Next(_randomVal.Length));
            //return (T)_randomVal.GetValue(RandomNumber(0, _randomVal.Length));
        }

        /// <summary>
        /// Gets all values from enums/n
        /// List<MyEnum> myEnumList = new List<MyEnum>();/n
        /// myEnumList.AddRange(GetEnumValues<MyEnum>());
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetEnumValues<T>() where T : struct
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Returns the string name of an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(this object value) where T : struct
        {
            return Enum.GetName(typeof(T), value);
        }

        /// <summary>
        /// Returns array of all string names of an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetEnumNames<T>() where T : struct
        {
            return Enum.GetNames(typeof(T));
        }
    }
}