using LoadoutScript.PedCustomisables;
using System.Collections.Generic;

namespace LoadoutScript
{
    /// <summary>
    /// Class that represents a loadout option the player can configure.
    /// </summary>
    internal class LoadoutOption
    {
        public string Name;
        //If NoneOption is true, adds a "None" option to the menu that doesn't change any of the ped components/props.
        public bool NoneOption;
        public Dictionary<string, IPedCustomisable> OptionNameToCustomisable;

        public LoadoutOption(string name, bool noneOption, Dictionary<string, IPedCustomisable> optionNameToCustomisable)
        {
            Name = name;
            NoneOption = noneOption;
            OptionNameToCustomisable = optionNameToCustomisable;
        }
    }
}
