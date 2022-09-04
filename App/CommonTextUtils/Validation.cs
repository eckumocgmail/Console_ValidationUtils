using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

public class Validation
{
     
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static bool IsValidUrl(string url)
    {
        if(( url.ToLower().StartsWith("http://") ||
                url.ToLower().StartsWith("https://") ||
                    url.ToLower().StartsWith("ftp://") ||
                        url.ToLower().StartsWith("file://") ) &&
            url.Substring(url.IndexOf("://")).Length > 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    

    

    public static bool IsRus(string word)
    {
        return Regex.Match(word, "/^[а-яА-ЯёЁ]+$/", RegexOptions.IgnoreCase).Success;
    }
    public static bool IsEng(string word)
    {
        string alf = "qwertyuiopasdfghjklzxcvbnm" + " " + "qwertyuiopasdfghjklzxcvbnm".ToUpper();
        string text = word;
        for (int i = 0; i < text.Length; i++)
        {
            if (!alf.Contains(text[i]))
            {
                return false;
            }
        }
        return true;
    }



    public static bool IsNumber(string text)
    {
        foreach(char ch in text.ToCharArray())
        {
            if (!"0123456789".Contains(ch))
            {
                return false;
            }
        }
        return true;
    }

   
}

