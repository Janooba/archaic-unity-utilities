using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Archaic.Core.Utilities
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Add to inputed list from inputed elements/n
        /// EX: AddToList(myList, a, b, c, etc...);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_elements"></param>
        public static void AddToList<T>(this List<T> _list, params T[] _elements)
        {
            _list.AddRange(_elements);
        }

        // Redo this as a scheduler service
        #region Timer Extensions (Coroutines)
        /*
        /// <summary>
        /// Is a dictionary of strings and bools for the IETimerDic 
        /// </summary>
        public static Dictionary<String, bool> TimerDicD;

        /// <summary>
        /// Exectute stated method after time has passed in game
        /// EX: StartCoroutine(TimerTest(Method, 5f));
        /// </summary>
        /// <param name="_method">The method you want to run</param>
        /// <param name="_time">The time from </param>
        /// <returns></returns>
        public static IEnumerator TimerDicIE(this Action _method, float _time, string ID)
        {
            bool _bool = true;
            TimerDicD.Add(ID, _bool);
            _time += Time.time;
            while (Time.time < _time)
            {
                yield return true;
            }
            TimerDicD.TryGetValue(ID, out _bool);
            if (_bool)
            {
                _method();
            }
            TimerDicD.Remove(ID);
        }

        /// <summary>
        /// Exectute stated method after time has passed in game
        /// EX: StartCoroutine(TimerTest(Method, 5f));
        /// </summary>
        /// <param name="_method">The method you want to run</param>
        /// <param name="_time">The time from </param>
        /// <returns></returns>
        public static IEnumerator ForcedTimer(this Action _method, float _time)
        {
            _time += Time.time;
            while (Time.time < _time)
            {
                yield return new WaitForEndOfFrame();
            }
            Debug.Log(_method.Method.ToString());
            _method();
            //yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// Execute stated method after the numer of stated frames
        /// EX: StartCoroutine(TimerTest(Method, 5f));
        /// </summary>
        /// <param name="_method">The method you want to run</param>
        /// <param name="_frame">The numer of frames. </param>
        /// <returns></returns>
        public static IEnumerator ForcedTimer(this Action _method, int _frame = 1)
        {
            int _counter = 0;
            while (_counter < _frame + 1)
            {
                yield return new WaitForEndOfFrame();
                _counter++;
            }
            _method();
        }

        public static IEnumerator ForcedTimer<T>(this Action<T> _method, T _var, int _frame = 1)
        {
            int _counter = 0;
            while (_counter < _frame)
            {
                _counter++;
                yield return new WaitForEndOfFrame();
            }
            _method(_var);
        }
        */
        #endregion
    }
}