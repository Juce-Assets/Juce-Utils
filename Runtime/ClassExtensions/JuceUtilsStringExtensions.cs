using System.IO;
using System.Text;

public static class JuceUtilsStringExtensions
{
    public static string FirstCharToUpper(this string str)
    {
        string ret = str;

        if (!string.IsNullOrEmpty(str))
        {
            StringBuilder sb = new StringBuilder(ret);

            sb[0] = char.ToUpper(sb[0]);

            ret = sb.ToString();
        }

        return ret;
    }

    public static Stream ToStream(this string str)
    {
        MemoryStream ret = new MemoryStream();

        StreamWriter writer = new StreamWriter(ret);
        writer.Write(str);
        writer.Flush();

        ret.Position = 0;

        return ret;
    }
}