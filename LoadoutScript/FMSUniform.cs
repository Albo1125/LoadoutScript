using CitizenFX.Core;
using LoadoutScript.PedCustomisables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadoutScript
{
    internal class FMSUniform
    {
        public string uniformname, gametype;
        public int componentid, drawableid, textureid;

        public FMSUniform(string uniformname, string gametype, int componentid, int drawableid, int textureid)
        {
            this.uniformname = uniformname;
            this.gametype = gametype;
            this.componentid = componentid;
            this.drawableid = drawableid;
            this.textureid = textureid;
        }

        public IPedCustomisable GetPedCustomisable()
        {
            if (this.gametype == "Prop")
            {
                return new PedsProp((PedsProp.PedProps)this.componentid, this.drawableid, this.textureid);
            } else if (this.gametype == "Clothing")
            {
                return new PedsComponent((PedsComponent.PedComponents)this.componentid, this.drawableid, this.textureid);
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            return obj is FMSUniform uniform &&
                   uniformname == uniform.uniformname &&
                   gametype == uniform.gametype &&
                   componentid == uniform.componentid &&
                   drawableid == uniform.drawableid &&
                   textureid == uniform.textureid;
        }

        public override int GetHashCode()
        {
            int hashCode = -27903177;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(uniformname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(gametype);
            hashCode = hashCode * -1521134295 + componentid.GetHashCode();
            hashCode = hashCode * -1521134295 + drawableid.GetHashCode();
            hashCode = hashCode * -1521134295 + textureid.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"[uniformname={this.uniformname}" + 
                   $",gametype={this.gametype}" +
                   $",componentid={this.componentid}" +
                   $",drawableid={this.drawableid}" + 
                   $",textureid={this.textureid}]";
        }
    }
}
