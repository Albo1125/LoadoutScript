using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoadoutScript.PedCustomisables
{
    /// <summary>
    /// Class that represents a custom configuration of one of the PedProps.
    /// </summary>
    internal class PedsProp : IPedCustomisable
    {
        public enum PedProps { Hats, Glasses, Ears, Watches = 6, Bracelets = 7 };

        [JsonConverter(typeof(StringEnumConverter))]
        public PedProps propId;

        //Simple trainer: first number as listed on the clothes menu, minus 1.
        public int drawableId;

        //Simple trainer: third number as listed on the clothes menu, minus 1.
        public int textureId;

        public PedsProp(PedProps propId, int drawableId, int textureId)
        {
            this.propId = propId;
            this.drawableId = drawableId;
            this.textureId = textureId;
        }

        public void SetPedCustomisable(Ped p)
            => API.SetPedPropIndex(p.Handle, (int)propId, drawableId, textureId, true);
    }
}
