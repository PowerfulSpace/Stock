﻿namespace Stock.Models
{
    public static class Helper
    {
        public static string GetTypeName(string fullTypeName)
        {
            string retString = string.Empty;

            try
            {
                int lastIndex = fullTypeName.LastIndexOf('.') + 1;
                retString = fullTypeName.Substring(lastIndex, fullTypeName.Length - lastIndex);
            }
            catch (Exception)
            {
                retString = fullTypeName;
            }
            return retString;
        }
    }
}
