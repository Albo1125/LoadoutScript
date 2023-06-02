using CitizenFX.Core;
using LoadoutScript.PedCustomisables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LoadoutScript
{
    /// <summary>
    /// Class that represents a complete loadout and its customisation options.
    /// </summary>
    internal class Loadout
    {
        public string Division, Name, PedModelName;
        public bool TaserOption;

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public WeaponHash[] WeaponHashes;

        public IPedCustomisable[] DefaultCustomisables;
        public LoadoutOption[] LoadoutOptions;

        public Loadout(string Division, string Name, WeaponHash[] WeaponHashes, bool taserOption, string PedModelName, IPedCustomisable[] DefaultCustomisables, LoadoutOption[] LoadoutOptions)
        {

            this.Division = Division;
            this.Name = Name;
            this.TaserOption = taserOption;
            this.PedModelName = PedModelName;
            this.WeaponHashes = WeaponHashes;
            this.DefaultCustomisables = DefaultCustomisables;

            this.LoadoutOptions = LoadoutOptions;
        }

        /// <summary>
        /// Equips this loadout with the specified options.
        /// </summary>
        /// <param name="taser">Whether or not to equip a TASER.</param>
        /// <param name="options">IPedCustomisables to set for the player.</param>
        public async void Equip(bool taser, IEnumerable<IPedCustomisable> options)
        {
            Model model = new Model(PedModelName);
            await model.Request(2000);
            if (model.IsLoaded)
            {
                await Game.Player.ChangeModel(model);

                foreach (IPedCustomisable pc in DefaultCustomisables)
                {
                    pc.SetPedCustomisable(Game.PlayerPed);
                    await BaseScript.Delay(50);
                }
                foreach (IPedCustomisable pc in options)
                {
                    pc.SetPedCustomisable(Game.PlayerPed);
                    await BaseScript.Delay(50);
                }

                Game.PlayerPed.Weapons.RemoveAll();
                foreach (WeaponHash hash in WeaponHashes)
                {
                    Game.PlayerPed.Weapons.Give(hash, 1000, false, true);
                    await BaseScript.Delay(50);
                }
                if (taser)
                {
                    Game.PlayerPed.Weapons.Give(WeaponHash.StunGun, -1, false, true);
                }
            }
        }
    }
}
