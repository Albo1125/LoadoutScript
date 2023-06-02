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
        public static MenuPool fmsmenuPool = new MenuPool();
        private static UIMenu mainMenu, fmsUniformMenu;
        private static List<LoadoutSubMenu> loadoutsubmenus = new List<LoadoutSubMenu>();
        private bool awaitingFmsUniforms = false;
        protected bool initialized = false;
        internal static FMSUniform[] FMSUniforms = new FMSUniform[0];

        public LoadoutScript()
        {
            Debug.Write("LoadoutScript by Albo1125.");

            mainMenu = new UIMenu("Loadouts", "");
            _menuPool.Add(mainMenu);

            fmsUniformMenu = new UIMenu("FMS Uniforms", "");
            fmsmenuPool.Add(fmsUniformMenu);

            //Deserialize the loadouts.json file.
            string resourceName = API.GetCurrentResourceName();
            string loadouts = API.LoadResourceFile(resourceName, "loadouts.json");
            if (string.IsNullOrWhiteSpace(loadouts))
            {
                loadouts = "[]";
            }
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
                TriggerServerEvent("fms:fetchuniforms");
            });

            EventHandlers["Loadouts:FMSUniforms"] += new Action<dynamic>((dynamic) =>
            {
                awaitingFmsUniforms = true;
                TriggerServerEvent("fms:fetchuniforms");
            });

            EventHandlers["fms:pushuniforms"] += new Action<List<dynamic>>((List<dynamic> uniformslist) =>
            {
                FMSUniform[] uniformsarr = uniformslist.Select(x => new FMSUniform(x.uniformname, x.gametype, x.componentid, x.drawableid, x.textureid)).ToArray();
                if (!uniformsarr.SequenceEqual(FMSUniforms))
                {
                    FMSUniforms = uniformsarr;
                    populateFmsUniformsMenu();
                }

                if (!awaitingFmsUniforms)
                {
                    return;
                }
                awaitingFmsUniforms = false;
                fmsUniformMenu.Visible = true;
            });

            Main();

        }

        public void populateFmsUniformsMenu()
        {
            Debug.WriteLine("Refreshing FMS Uniforms menu");
            fmsmenuPool.CloseAllMenus();
            fmsUniformMenu = new UIMenu("FMS Uniforms", "");
            fmsmenuPool = new MenuPool();

            foreach (FMSUniform f in FMSUniforms)
            {
                UIMenuItem i = new UIMenuItem(f.uniformname, f.gametype + ". Component " + f.componentid + " Drawable " + f.drawableid + " Texture " + f.textureid);
                fmsUniformMenu.AddItem(i);
            }

            fmsUniformMenu.OnItemSelect += (sender, selectedItem, index) =>
            {
                FMSUniform f = FMSUniforms[index];
                var c = f.GetPedCustomisable();
                if (c == null)
                {
                    Debug.WriteLine("Could not get PedCustomisable");
                    return;
                }

                c.SetPedCustomisable(this.LocalPlayer.Character);
            };

            
            fmsUniformMenu.MouseControlsEnabled = false;
            fmsUniformMenu.MouseEdgeEnabled = true;
            fmsmenuPool.Add(fmsUniformMenu);
            fmsUniformMenu.RefreshIndex();
            fmsmenuPool.RefreshIndex();
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
                fmsmenuPool.ProcessMenus();
            }
        }
    }
}
