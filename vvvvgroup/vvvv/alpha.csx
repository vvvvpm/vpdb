/* mppm 2.0 {
    name: vvvv
    version: alpha
    author: vvvv group
    license:
        '''
        Don't use for production!
        Patches you save with an alpha version
        may not open anymore
        with the current beta version!

        Please mark problems you report in the forum with the alpha tag
        or visit us in the matrix for a chat over issues you encounter.

        vvvv is free for evaluation and non-commercial use.
        For all commercial applications, you need to buy a license.
        See more info at https://vvvv.org/documentation/licensing
        '''
} */

using System;
using System.Xml;
using System.Net;
using System.IO;

var baseurl = "http://vvvv.org:8111";
var initreguest = "/guestAuth/app/rest/builds/buildType:vvvv45_alpha_" + VVVV.Architecture + ",status:SUCCESS";

var client = new WebClient();
Console.WriteLine("Fetching latest VVVV from TeamCity");

var stream = client.OpenRead(baseurl + initreguest);
var reader = new StreamReader(stream);
var xmltext = reader.ReadToEnd();

Console.WriteLine("Parsing xml data");
var builddoc = new XmlDocument();
builddoc.LoadXml(xmltext);
var artifactnode = builddoc.SelectSingleNode("/build/artifacts");
var artifactrequest = artifactnode.Attributes["href"].InnerText;

Console.WriteLine("Fetching artifacts");
stream = client.OpenRead(baseurl + artifactrequest);
reader = new StreamReader(stream);
xmltext = reader.ReadToEnd();

Console.WriteLine("Parsing artifacts");
var artdoc = new XmlDocument();
artdoc.LoadXml(xmltext);

var nodelist = artdoc.SelectSingleNode("/files").ChildNodes;
var vvvvurl = "";
var vvvvzip = "";
var addonsurl = "";
var addonszip = "";
for (int i = 0; i &lt; nodelist.Count; i++)
{
    var filenode = nodelist.Item(i);
    if (filenode == null) continue;

    if (filenode.Attributes["name"].InnerText.StartsWith("vvvv_"))
    {
        vvvvurl = baseurl + filenode["content"].Attributes["href"].InnerText;
        vvvvzip = filenode.Attributes["name"].InnerText;
    }
    if (filenode.Attributes["name"].InnerText.StartsWith("addons_"))
    {
        addonsurl = baseurl + filenode["content"].Attributes["href"].InnerText;
        addonszip = filenode.Attributes["name"].InnerText;
    }
}

Console.WriteLine("Downloading and extracting VVVV");
var locvvvvzip = Path.Combine(Pack.TempDir, vvvvzip);
Download(vvvvurl, locvvvvzip);
Extract(locvvvvzip, Pack.TempDir);
CopyDir(Path.Combine(Pack.TempDir, Path.GetFileNameWithoutExtension(vvvvzip)), VVVV.Dir);

Console.WriteLine("Downloading and extracting the Addonpack");
var locaddonszip = Path.Combine(Pack.TempDir, addonszip);
Download(addonsurl, locaddonszip);
Extract(locaddonszip, VVVV.Dir);

void Install(string src, string filename, string args)
{
    Console.WriteLine("Installing " + filename);
    var rfn = Path.Combine(Pack.TempDir, filename);
    Download(src, rfn);
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = rfn,
            Arguments = args,
            Verb = "runas"
        }).WaitForExit();
    }
    catch (System.Exception e)
    {
        ThrowException(e);
    }
}
Console.WriteLine("Install vvvv dependencies? (Y)es or (N)o\n(DX9, VC++ redists, setup.exe)\n(if vpm is not running as an admin UAC might pop-up 14 times)");
if(Console.ReadLine()?.Contains("y") ?? false)
{
    Install("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe", "dxwebsetup.exe", "/Q");
    if(VVVV.Architecture == "x64")
    {
        Install("https://download.microsoft.com/download/2/d/6/2d61c766-107b-409d-8fba-c39e61ca08e8/vcredist_x64.exe", "vcr.2008.x64.exe", "/Q");
        Install("https://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe", "vcr.2010.x64.exe", "/passive /norestart");
        Install("https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe", "vcr.2012.x64.exe", "/passive /norestart");
        Install("http://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe", "vcr.2013.x64.exe", "/passive /norestart");
        Install("https://download.microsoft.com/download/9/3/F/93FCF1E7-E6A4-478B-96E7-D4B285925B00/vc_redist.x64.exe", "vcr.2015.x64.exe", "/passive /norestart");
        Install("https://download.visualstudio.microsoft.com/download/pr/11100230/15ccb3f02745c7b206ad10373cbca89b/VC_redist.x64.exe", "vcr.2017.x64.exe", "/passive /norestart");
    }
    Install("https://download.microsoft.com/download/d/d/9/dd9a82d0-52ef-40db-8dab-795376989c03/vcredist_x86.exe", "vcr.2008.x86.exe", "/Q");
    Install("https://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe", "vcr.2010.x86.exe", "/passive /norestart");
    Install("https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x86.exe", "vcr.2012.x86.exe", "/passive /norestart");
    Install("http://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe", "vcr.2013.x86.exe", "/passive /norestart");
    Install("https://download.microsoft.com/download/9/3/F/93FCF1E7-E6A4-478B-96E7-D4B285925B00/vc_redist.x86.exe", "vcr.2015.x86.exe", "/passive /norestart");
    Install("https://download.visualstudio.microsoft.com/download/pr/11100229/78c1e864d806e36f6035d80a0e80399e/VC_redist.x86.exe", "vcr.2017.x86.exe", "/passive /norestart");

    Console.WriteLine("Running Setup.exe");
    Process.Start(new ProcessStartInfo
    {
        FileName = Path.Combine(VVVV.Dir, "setup.exe"),
        Arguments = "/silent",
        Verb = "runas"
    }).WaitForExit();
}
