using System.Globalization;

namespace ZachStreamApp
{
    /// <summary>
    /// Internal class used to hold utility functions
    /// </summary>
    internal class Utils
    {
        /// <summary>
        /// Check if the string input is an integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static bool IsInputInteger(string input)
        {
            int num;
            return int.TryParse(input.Trim(), NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.CurrentUICulture, out num);
        }

        /// <summary>
        /// Parse string as integer.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static int ParseInputInteger(string input)
        {
            return int.Parse(input, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Returns a full file path from a partial path and file name
        /// </summary>
        /// <param name="filePath">The directory the file is in</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>Full path of the file</returns>
        internal static string GetFullFilePath(string filePath, string fileName)
        {
            return string.Format("{0}\\{1}.txt", filePath, fileName);
        }
    }
}
