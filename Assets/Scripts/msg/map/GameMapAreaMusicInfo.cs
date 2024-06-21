using System;
using Net;

namespace map
{
    public class GameMapAreaMusicInfo : StructData
    {
        public override OctetsStream WriteData(OctetsStream ot)
        {
            ot.marshal_int(this.width);
            ot.marshal_int(this.height);
            ot.marshal_boolean(this.isSame);
            if (this.nodes != null)
            {
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    ot.marshal_byte(this.nodes[i]);
                }
            }
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.width = ot.unmarshal_int();
            this.height = ot.unmarshal_int();
            this.isSame = ot.unmarshal_boolean();
            this.nodes = new byte[this.width * this.height];
            for (int i = 0; i < this.width * this.height; i++)
            {
                this.nodes[i] = ot.unmarshal_byte();
            }
            return ot;
        }

        public byte GetValueByIndex(int w, int h)
        {
            if (w > this.width || h > this.height)
            {
                return 0;
            }
            return this.nodes[h * this.width + w];
        }

        public int width = 1;

        public int height = 1;

        public bool isSame;

        public byte[] nodes;
    }
}
