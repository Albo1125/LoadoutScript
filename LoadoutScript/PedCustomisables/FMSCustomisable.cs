using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace LoadoutScript.PedCustomisables
{
    /// <summary>
    /// Class that represents a FMSCustomisable which gets data from the FMS.
    /// </summary>
    internal class FMSCustomisable : IPedCustomisable
    {
        public string uniformTypeName;

        public FMSCustomisable(string uniformTypeName)
        {
            this.uniformTypeName = uniformTypeName;
        }

        public void SetPedCustomisable(Ped p)
        {
            FMSUniform uniform = LoadoutScript.FMSUniforms.FirstOrDefault(x => x.uniformname == this.uniformTypeName);
            if (uniform == null)
            {
                Debug.WriteLine("Skipping - Could not find FMS uniform for user with type name " + this.uniformTypeName);
                return;
            }
            uniform.GetPedCustomisable().SetPedCustomisable(p);
        }
    }
}
