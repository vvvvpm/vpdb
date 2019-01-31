/* mppm 2.0 {
    name: vvvv
    version: 38
    author: vvvv group
    license:
        '''
        vvvv is free for evaluation and non-commercial use.
        For all commercial applications, you need to buy a license.
        See more info at https://vvvv.org/documentation/licensing
        '''
} */

using System;
using System.Net;
using System.IO;
using System.Diagnostics;

var version = "50beta38";

var vvvvurl = $"https://vvvv.org/sites/default/files/vvvv_{version}_{VVVV.Architecture}.zip";
var addonsurl = $"https://vvvv.org/sites/default/files/addons_{version}_{VVVV.Architecture}.zip";

Console.WriteLine("Downloading and extracting vvvv");

var locvvvvzip = Path.Combine(Pack.TempDir, $"vvvv_{version}_{VVVV.Architecture}.zip");
Download(vvvvurl, locvvvvzip);
Extract(locvvvvzip, Pack.TempDir);
CopyDir(Path.Combine(Pack.TempDir, Path.GetFileNameWithoutExtension(locvvvvzip)), VVVV.Dir);

Console.WriteLine("Downloading and extracting the Addonpack");
var locaddonszip = Path.Combine(Pack.TempDir, "addonpack.zip");
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
