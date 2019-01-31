/* mppm 2.0 {
	name: dx11
	version: appveyor
	author: vux
	aliases: dx11-vvvv, vvvv-dx11
	license:
		'''
		BSD 3-Clause License
		Copyright (c) 2016, Julien Vulliet (mrvux) All rights reserved.
		See more info at https://github.com/mrvux/dx11-vvvv/blob/master/License.md
		'''
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
	var avuser = "mrvux";
	var avproject = "dx11-vvvv";
	var baseurl = "https://ci.appveyor.com";
	var filename = string.Format("vvvv-packs-dx11-{0}.zip", VVVV.Architecture);
	var auxartpath = WebUtility.UrlEncode("Zip");

	var initreguest = string.Format(
		"/api/projects/{0}/{1}",
		avuser,
		avproject
	);

	var client = new WebClient();
	//client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
	client.Headers.Add("Authorization", Encoding.ASCII.GetString(Convert.FromBase64String("QmVhcmVyIDIydGgya28zdjB1N254bDhveGQw")));
	client.Headers.Add("content-type", "application/json");
	Console.WriteLine("Fetching project data from appveyor");

	var stream = client.OpenRead(baseurl + initreguest);
	var reader = new StreamReader(stream).ReadToEnd();
	
	var jsondata = JObject.Parse(reader);
	var jobid = jsondata.SelectToken("$.build.jobs").ToArray()[0]["jobId"].ToString();

	Console.WriteLine("Downloading {0} from job ID {1}", filename, jobid);

	var durl = string.Format(
		"{0}/api/buildjobs/{1}/artifacts/{2}%2F{3}",
		baseurl,
		jobid,
		auxartpath,
		filename
	);

	Download(
		durl,
		Path.Combine(VPM.TempDir, filename)
	);
	Extract(Path.Combine(VPM.TempDir, filename), Pack.TempDir);
	CopyDir(
		Pack.TempDir,
		VVVV.Dir
	);
}
catch (Exception e)
{
	ThrowException(e);
}
