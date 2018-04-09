using LoadoutScript.PedCustomisables;
using NativeUI;
using System;
using System.Collections.Generic;

namespace LoadoutScript
{
    /// <summary>
    /// Class that represents a loadout submenu for a division.
    /// </summary>
    internal class LoadoutSubMenu
    {
        public UIMenuItem MainMenuDivisionItem;
        public UIMenu DivisionMenu;
        public string Division;
        public Dictionary<UIMenuItem, Loadout> itemToLoadout = new Dictionary<UIMenuItem, Loadout>();

        public UIMenu LoadoutOptionsMenu = new UIMenu("Options", "");

        /// <summary>
        /// Constructor that creates a NativeUI menu based on the specified parameters.
        /// </summary>
        /// <param name="Division"></param>
        /// <param name="Loadouts"></param>
        public LoadoutSubMenu(string Division, IEnumerable<Loadout> Loadouts)
        {
            this.Division = Division;
            MainMenuDivisionItem = new UIMenuItem(Division);
            DivisionMenu = new UIMenu(Division, "Loadouts");

            //Create a new UIMenuItem for every loadout this division has.
            foreach (Loadout l in Loadouts)
            {
                UIMenuItem item = new UIMenuItem(l.Name);

                DivisionMenu.AddItem(item);
                itemToLoadout[item] = l;
            }

            DivisionMenu.MouseControlsEnabled = false;
            DivisionMenu.MouseEdgeEnabled = true;

            //Add a custom event handler to every loadout's UIMenuItem that creates a custom menu for that loadout and shows it.
            DivisionMenu.OnItemSelect += (sender, selectedItem, index) =>
            {
                sender.Visible = false;
                CreateOptionsMenu(itemToLoadout[selectedItem]);
                LoadoutScript.RefreshMenuPool();
                LoadoutScript._menuPool.Add(LoadoutOptionsMenu);
                LoadoutScript._menuPool.RefreshIndex();
                LoadoutOptionsMenu.Visible = true;
            };

            DivisionMenu.RefreshIndex();

        }

        /// <summary>
        /// Method that creates a custom submenu based on the specified Loadout.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public UIMenu CreateOptionsMenu(Loadout l)
        {
            LoadoutOptionsMenu = new UIMenu(l.Name, "Loadouts");
            LoadoutOptionsMenu.ParentMenu = DivisionMenu;
            List<UIMenuListItem> optionsListItems = new List<UIMenuListItem>();

            //For every option, create a UIMenuListItem with all the available options for that LoadoutOption.
            foreach (LoadoutOption opt in l.LoadoutOptions)
            {
                UIMenuListItem listitem;
                List<dynamic> optionNames = new List<dynamic>(opt.OptionNameToCustomisable.Keys);
                if (opt.NoneOption)
                {
                    optionNames.Insert(0, "None");
                }
                LoadoutOptionsMenu.AddItem(listitem = new UIMenuListItem(opt.Name, optionNames, 0));
                optionsListItems.Add(listitem);
            }

            //Create a taser option if specified.
            UIMenuCheckboxItem taseritem = null;
            if (l.TaserOption)
            {
                LoadoutOptionsMenu.AddItem(taseritem = new UIMenuCheckboxItem("Equip Taser", false));
            }

            UIMenuItem confirmitem = new UIMenuItem("Confirm");
            LoadoutOptionsMenu.AddItem(confirmitem);
            LoadoutOptionsMenu.MouseControlsEnabled = false;
            LoadoutOptionsMenu.MouseEdgeEnabled = true;

            //When confirming selection, check which options have been selected and equip the Loadout with those options.
            LoadoutOptionsMenu.OnItemSelect += (sender, selectedItem, index) =>
            {
                if (selectedItem == confirmitem)
                {
                    List<IPedCustomisable> options = new List<IPedCustomisable>();
                    for (int i = 0; i < optionsListItems.Count; i++)
                    {
                        string optionChosen = optionsListItems[i].IndexToItem(optionsListItems[i].Index);
                        Dictionary<string, IPedCustomisable> dict = l.LoadoutOptions[i].OptionNameToCustomisable;
                        if (dict.ContainsKey(optionChosen))
                        {
                            options.Add(dict[optionChosen]);
                        }
                    }
                    l.Equip(taseritem != null && taseritem.Checked, options);
                    sender.Visible = false;

                }
            };

            LoadoutOptionsMenu.RefreshIndex();
            return LoadoutOptionsMenu;
        }
    }
}
