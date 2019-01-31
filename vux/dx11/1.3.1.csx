/* uppm 2.0 {
	name: dx11
	version: 1.3.1
	author: vux
	license:
		'''
		BSD 3-Clause License
		Copyright (c) 2016, Julien Vulliet (mrvux) All rights reserved.
		See more info at https://github.com/mrvux/dx11-vvvv/blob/master/License.md
		'''
} */

new
{
	Install = Action(() => 
	{
		var srcurl = VVVV.Architecture == "x64" ?
			"https://vvvv.org/sites/default/files/uploads/vvvv-packs-dx11-1.3.1.x64.zip" :
			"https://vvvv.org/sites/default/files/vvvv-packs-dx11-x86-1.3.1.zip";
		Download(
			srcurl,
			VPM.TempDir + "\\dx11-vvvv.zip"
		);
		Extract(VPM.TempDir + "\\dx11-vvvv.zip", Pack.TempDir);
		CopyDir(
			Pack.TempDir,
			VVVV.Dir
		);
	})
};