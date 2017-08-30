using System;

namespace AzureBlobStorageSampleApp
{
    public static class DebugServices
    {
        public static void Log(Exception exception)
        {
            var exceptionType = exception.GetType().ToString();
            var message = exception.Message;

            System.Diagnostics.Debug.WriteLine(exceptionType);
            System.Diagnostics.Debug.WriteLine($"Error: {message}");
        }
    }
}
