GitClone("https://github.com/velcrome/vvvv-Message.git", Pack.TempDir);

BuildSolution(2015, Pack.TempDir + "\\src\\vvvv-Message.sln", "Release|AnyCPU", true);

CopyDir(
	Pack.TempDir + "\\build\\AnyCPU\\Release",
	VVVV.Dir + "\\packs\\vvvv-Message"
);
