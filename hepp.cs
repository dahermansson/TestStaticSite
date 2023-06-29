using System.Net;
using HtmlAgilityPack;
using System.Text.Json;

var httpClient = new HttpClient(new HttpClientHandler() { DefaultProxyCredentials = CredentialCache.DefaultCredentials});

var request = await httpClient.GetAsync("https://gitwidgets.golf.se/scorecard");
var html = await request.Content.ReadAsStringAsync();

var htmlDocument = new HtmlDocument();
htmlDocument.LoadHtml(html);

var klubbar = htmlDocument.DocumentNode.SelectSingleNode("//select[@id='club-list']").ChildNodes.Where(t => t.HasAttributes).Select(t => new {id = t.GetAttributeValue("value", ""), name = t.InnerText });
foreach (var klubb in klubbar)
{
    Console.WriteLine($"id:{klubb.id}, namn: {WebUtility.HtmlDecode(klubb.name)}");

    var courseRequest = await httpClient.GetAsync($"https://gitwidgets.golf.se/scorecard/courses?guid={klubb.id}");
    var jsonCourses = await courseRequest.Content.ReadAsStringAsync();

    var courses = JsonSerializer.Deserialize<List<Course>>(jsonCourses);

    foreach (var course in courses)
    {
        Console.WriteLine(course);
    }
}


public record Course(string label, string value);
