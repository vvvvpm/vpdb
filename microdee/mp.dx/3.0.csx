/* mppm 2.0 {
	name: mp.dx
	version: 3.0
	author: microdee
	license:
		'''
		MIT License
		Copyright (C) 2018 David Mórász
		'''
	dependencies: [
		"mp.essentials:3.0"
		"mp.fxh"
		"dx11"
	]
} */

#load "uppm-ref:uppm.github:specific"

new
{
	Install = Action(() => FetchGithub(
		"microdee",
		"mp.dx",
		"mp.dx.zip",
		3, 0
	))
}