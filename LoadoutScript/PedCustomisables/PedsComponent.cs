using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoadoutScript.PedCustomisables
{
    /// <summary>
    /// Class that represents a custom configuration of one of the PedComponents.
    /// </summary>
    internal class PedsComponent : IPedCustomisable
    {
        public enum PedComponents { Head, Beard, Hair, Upper, Lower, Hands, Shoes, Acc1 = 8, Acc2 = 9, Badges = 10, AuxTorso = 11 }

        [JsonConverter(typeof(StringEnumConverter))]
        public PedComponents componentId;

        //Simple trainer: first number as listed on the clothes menu, minus 1.
        public int drawableId;

        //Simple trainer: third number as listed on the clothes menu, minus 1.
        public int textureId;

        public PedsComponent(PedComponents componentId, int drawableId, int textureId)
        {
            this.componentId = componentId;
            this.drawableId = drawableId;
            this.textureId = textureId;
        }

        public void SetPedCustomisable(Ped p)
            => API.SetPedComponentVariation(p.Handle, (int)componentId, drawableId, textureId, 2);
    }
}
