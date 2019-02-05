using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AzureBlobStorageSampleApp
{
    public static class DebugServices
    {
        [Conditional("DEBUG")]
        public static void Log(Exception exception,
                                IDictionary<string, string> properties = null,
                                [CallerMemberName] string callerMemberName = "",
                                [CallerLineNumber] int lineNumber = 0,
                                [CallerFilePath] string filePath = "")
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");

            if (properties != null)
            {
                foreach (var property in properties)
                    Debug.WriteLine($"{property.Key}: {property.Value}");
            }
        }
    }
}
