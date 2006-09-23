using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using SnCore.Tools.Web;
using System.IO;
using System.Text;
using SnCore.Tools.Web.Html;
using System.Text.RegularExpressions;

public class ContentPage
{
    public static string GetContent(Uri uri, Uri baseuri)
    {
        return GetContent(uri, baseuri, string.Empty);
    }

    public static string GetHttpContent(Uri uri)
    {
        string content = string.Empty;
        
        WebRequest request = HttpWebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            content = sr.ReadToEnd();
            sr.Close();
        }

        return content;
    }

    public static string GetCss(Uri baseuri)
    {
        string css = GetHttpContent(new Uri(baseuri, "Style.css"));
        return CssAbsoluteLinksWriter.Rewrite(css, baseuri);
    }

    public static string GetContent(Uri uri, Uri baseuri, string note)
    {
        string content = GetHttpContent(uri);

        string[] expressions = 
        { 
            @"\<!-- NOEMAIL-START --\>.*?\<!-- NOEMAIL-END --\>",
            @"\<script.*?\<\/script\>",
            @"\<style.*?\<\/style\>",
            @"\<link.*?\<\/link\>",
            @"\<link.*?\/\>",
            @"href=""javascript:[0-9a-zA-Z$\._\';,\ =\(\)\[\]]*""",
            @".?onclick=""[0-9a-zA-Z\._\';,\ =\(\)\[\]]*""",
        };

        foreach (string r in expressions)
        {
            content = Regex.Replace(content, r, string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        content = HtmlAbsoluteLinksWriter.Rewrite(content, baseuri);

        StringBuilder scontent = new StringBuilder(content); 
        scontent.Insert(0, string.Format("<p style=\"margin: 10px;\"><a href=\"{0}\">can't see message? &#187;&#187; online version</a></p>\n", uri.ToString()));

        // insert additional note
        if (!string.IsNullOrEmpty(note))
        {
            scontent.Insert(0, string.Format("<p style=\"margin: 10px;\">{0}</p>\n", Renderer.Render(note)));
        }

        // hack: insert stylesheet
        scontent.Insert(0, string.Format("<style>\n{0}\n</style>\n", GetCss(baseuri)));
        return scontent.ToString();
    }
}
