using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Helper;
public class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templatemodel)
    {
        var templatepath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        var streamreader = new StreamReader(templatepath);
        var body = streamreader.ReadToEnd();
        streamreader.Close();
        foreach (var item in templatemodel)
        {
            body = body.Replace(item.Key, item.Value);
        }
        return body;
    }
}
