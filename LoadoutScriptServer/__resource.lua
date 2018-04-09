resource_manifest_version '44febabe-d386-4d18-afbe-5e627f4af937'
name 'LoadoutScript'
description 'Allows you to specify easily accessible custom loadouts in a menu using NativeUI.'
author 'Albo1125 (https://www.youtube.com/albo1125)'
version 'v1.7.0'
url 'https://github.com/Albo1125/LoadoutScript'
files {
	'Newtonsoft.Json.dll',
	'loadouts.json'
}
server_script 'sv_LoadoutScript.lua'
client_script {
	'NativeUI.net.dll',
	'LoadoutScript.net.dll'
}