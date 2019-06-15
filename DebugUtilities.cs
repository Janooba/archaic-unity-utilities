using System;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    ///<summary>Used to time how long a section of code takes to run in real time.</summary>
    public class Timer : IDisposable
    {
        System.Diagnostics.Stopwatch _watch;
        private object[] _args;
        private string _formattedString;

        public Timer(string str, params object[] arguments)
        {
            _args = arguments;
            _formattedString = str;
            _watch = new System.Diagnostics.Stopwatch();
            _watch.Start();
        }

        void IDisposable.Dispose()
        {
            _watch.Stop();
            string message = string.Format(_formattedString, _args);
            Debug.Log($"{_watch.Elapsed.TotalSeconds} seconds: {message}");
        }
    }
}
