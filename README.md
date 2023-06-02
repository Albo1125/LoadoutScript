# LoadoutScript
LoadoutScript is a resource for FiveM by Albo1125 that allows players to easily equip loadouts read from a json file, rather than having to set a bunch of component and prop options manually every time. [FMS](https://albo1125.com/fms) uniforms are also supported which is useful if you have certain custom components for every user, e.g. a name badge. Screenshots can be found at the bottom.

## Installation & Usage
1. Download the latest [release](https://github.com/Albo1125/LoadoutScript/releases).
2. Unzip the LoadoutScript folder into your resources folder on your FiveM server.
3. Create a loadouts.json file in the LoadoutScript folder (or rename the example if you prefer).
4. Add the following to your server.cfg file:
```text
start LoadoutScript
```
5. Type /lo in chat to access the loadouts menu.
6. Type /fmsuniforms in chat to access the FMS uniforms menu, showing all FMS user uniforms.

## Customising your loadouts
Customise your loadouts in the loadouts.json file. You can add as many loadouts as you like. An example loadouts.json file is included.
JSON reference is as follows.

### Loadout
Every entry in the root array is a Loadout.
* "Division" string indicating the main category of this loadout.
* "Name" string indicating the (unique?) name of this loadout for that division.
* "TaserOption" true or false depending on whether you want a Taser option checkbox.
* "PedModelName" string representing the model name of the ped.
* "WeaponHashes" array of [CitizenFX.Core.WeaponHash](https://github.com/citizenfx/fivem/blob/c00ddbfa320b4909e4caf9363c963948864aaa83/code/client/clrcore/External/WeaponHash.cs) enum values (string representation).
* "DefaultCustomisables" array of either PedsComponent, PedsProp or FMSCustomisable objects (see below).
* "LoadoutOptions" array of LoadoutOption objects

### LoadoutOption
* "Name"string indicating the name of this option.
* "NoneOption" true or false, if true adds a None option to the list of selectable values that doesn't change the ped's components/props.
* "OptionNameToCustomisable" Dictionary mapping a name string (descriptive of the component or prop) to either a PedsComponent, PedsProp or FMSCustomisable object (see below).

### PedsComponent (must specify type in JSON)
* "$type":"LoadoutScript.PedCustomisables.PedsComponent, LoadoutScript.net"
* "componentId" one of the following: Head, Beard, Hair, Upper, Lower, Hands, Shoes, Acc1, Acc2, Badges, AuxTorso
* "drawableId" the drawable ID to set this component to. For simple trainer users: first number as listed on the clothes menu, minus 1.
* "textureId" the texture ID to set this component to. For simple trainer users: third number as listed on the clothes menu, minus 1.

### PedsProp (must specify type in JSON)
* "$type":"LoadoutScript.PedCustomisables.PedsProp, LoadoutScript.net"
* "propId" one of the following: Hats, Glasses, Ears, Watches
* "drawableId" the drawable ID to set this component to. For simple trainer users: first number as listed on the clothes menu, minus 1.
* "textureId" the texture ID to set this component to. For simple trainer users: third number as listed on the clothes menu, minus 1.

### FMSCustomisable (must specify type in JSON)
* "$type":"LoadoutScript.PedCustomisables.FMSCustomisable, LoadoutScript.net"
* "uniformTypeName" the uniform type name for the FMS user uniform to load the component, drawable and texture IDs from. If the user has no FMS uniform with this uniformTypeName, it is skipped.

## Improvements & Licensing
Please view LICENSE.md. Improvements and new feature additions are very welcome, please feel free to create a pull request. Proper credit is always required if you release modified versions of my work.

## Libraries used (many thanks to their authors)
* [NativeUI FiveM Port](https://github.com/citizenfx/NativeUI)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [CitizenFX.Core](https://github.com/citizenfx/fivem)

## Screenshots
![LoadoutScript](https://image.prntscr.com/image/E-Mgy1z8RK6HFO1_Io-KLA.png)
![LoadoutScript](https://image.prntscr.com/image/NFHdFqkuQEOUjCoilb0Hdg.png)
![LoadoutScript](https://image.prntscr.com/image/OzshB0r3TZ_HHntxfHpAbg.png)
