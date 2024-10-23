using System;
using UnityEngine;

namespace erikssonn {
    public enum LogLevel {
        DEBUG,
        WARNING,
        ERROR,
        FATAL
    }

    public class Logger {
        private const string prefix = "[erikssonn]: ";

        public static void Print(string message, LogLevel logLevel = LogLevel.DEBUG) {
            switch (logLevel) {
                case LogLevel.DEBUG:
                    Debug.Log(prefix + message);
                    break;
                case LogLevel.WARNING:
                    Debug.LogWarning(prefix + message);
                    break;
                case LogLevel.ERROR:
                    Debug.LogError(prefix + message);
                    break;
                case LogLevel.FATAL:
                    Debug.LogException(new Exception(message));
                    Debug.Break();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, prefix + "default enum throw");
            }
        }
        
        public static void Throw(string message) {
            throw new Exception("[erikssonn]: EXCEPTION THROWN! " + message);
        }
    }
}