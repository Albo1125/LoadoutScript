using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadoutScript
{
    /// <summary>
    /// Main class that inherits from CitizenFx.Core's BaseScript.
    /// </summary>
    public class LoadoutScript : BaseScript
    {
        public static MenuPool _menuPool = new MenuPool();
        private static UIMenu mainMenu;
        private static List<LoadoutSubMenu> loadoutsubmenus = new List<LoadoutSubMenu>();
        protected bool initialized = false;

        public LoadoutScript()
        {
            Debug.Write("LoadoutScript by Albo1125.");

            mainMenu = new UIMenu("Loadouts", "");
            _menuPool.Add(mainMenu);

            //Deserialize the loadouts.json file.
            string resourceName = API.GetCurrentResourceName();
            string loadouts = API.LoadResourceFile(resourceName, "loadouts.json");
            Loadout[] AllLoadouts = JsonConvert.DeserializeObject<Loadout[]>(loadouts, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            //Group the loadouts by division and add a separate submenu for every division.
            IEnumerable<IGrouping<string, Loadout>> groupedLoadouts = AllLoadouts.GroupBy(x => x.Division);
            foreach (IGrouping<string, Loadout> div in groupedLoadouts)
            {
                loadoutsubmenus.Add(new LoadoutSubMenu(div.Key, div));
            }

            //Add every division's submenu to the main loadout menu.
            foreach (LoadoutSubMenu submenu in loadoutsubmenus)
            {
                mainMenu.AddItem(submenu.MainMenuDivisionItem);
                mainMenu.BindMenuToItem(submenu.DivisionMenu, submenu.MainMenuDivisionItem);

                _menuPool.Add(submenu.DivisionMenu);
                submenu.DivisionMenu.RefreshIndex();
            }

            //Disable mouse controls and refresh the indexes.
            mainMenu.MouseControlsEnabled = false;
            mainMenu.MouseEdgeEnabled = true;
            mainMenu.RefreshIndex();
            _menuPool.RefreshIndex();

            //Add an event handler for the /lo command.
            EventHandlers["Loadouts:ShowMenu"] += new Action<dynamic>((dynamic) =>
            {
                mainMenu.Visible = true;
            });

            Main();

        }

        /// <summary>
        /// Re-add all menus to a new instance of the menupool to prevent NativeUI issues.
        /// </summary>
        public static void RefreshMenuPool()
        {
            _menuPool = new MenuPool();
            _menuPool.Add(mainMenu);
            foreach (LoadoutSubMenu submenu in loadoutsubmenus)
            {
                _menuPool.Add(submenu.DivisionMenu);
            }
            _menuPool.RefreshIndex();
        }

        private async Task Main()
        {
            while (true)
            {
                await Delay(0);
                _menuPool.ProcessMenus();
            }
        }
    }
}
