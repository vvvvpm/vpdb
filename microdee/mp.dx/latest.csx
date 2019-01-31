/* mppm 2.0 {
	name: mp.dx
	version: latest
	author: microdee
	license:
		'''
		MIT License
		Copyright (C) 2018 David Mórász
		'''
	dependencies: [
		mp.essentials
		mp.fxh
		dx11
	]
} */

using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

try
{
	// params:
	var ghuser = "microdee";
	var ghrepo = "mp.dx";
	var baseurl = "https://api.github.com";
	var filename = "mp.dx.zip";

	
	var initreguest = string.Format(
		"/repos/{0}/{1}/releases?client_id={2}&amp;client_secret={3}",
		ghuser,
		ghrepo,
		Encoding.ASCII.GetString(Convert.FromBase64String("ZDM4YjY2ZDc3ZGQ4MGVlZDEyNjg=")),
		Encoding.ASCII.GetString(Convert.FromBase64String("MWFjM2JjYTRmNTY4ZmQ1MmI5MmUyNTAyOWQxZGI5MDRlZDZiMDdkYw=="))
	);
	// Console.WriteLine(baseurl + initreguest);

	var client = new WebClient();
	client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
	Console.WriteLine("Fetching releases from github");

	var stream = client.OpenRead(baseurl + initreguest);
	var reader = new StreamReader(stream).ReadToEnd();
	var jsondata = JArray.Parse(reader);
	var jsonres = jsondata.Children().ToArray()[0];
	var asset = jsonres["assets"].Where(a => a["name"].ToString() == filename).ToArray()[0];

	Console.WriteLine("Downloading {0} from {1}", filename, jsonres["tag_name"]);

	Download(
		asset["browser_download_url"].ToString(),
		Path.Combine(VPM.TempDir, filename)
	);
	Extract(Path.Combine(VPM.TempDir, filename), Pack.TempDir);
	CopyDir(
		Pack.TempDir,
		Path.Combine(VVVV.Dir, "packs"),
		ignore: new [] {"obsolete", "wip"}
	);
}
catch (Exception e)
{
	ThrowException(e);
}