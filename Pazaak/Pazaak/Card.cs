using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    public class Card
    {
        private Int16 val;
        private String imgSrc;
        private Random rnd;

        public string ImgSrc
        {
            get
            {
                return imgSrc;
            }

            set
            {
                imgSrc = value;
            }
        }

        public short Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public Card(Int16 val)
        {
            this.val = val;
        }
        public Card()
        {
            this.rnd = new Random();
            Int16 value = (Int16)this.rnd.Next(1, 11);
            this.val = value;
        }
        public Card(Random r)
        {
            this.rnd = r;
            Int16 value = (Int16)this.rnd.Next(1, 11);
            this.val = value;
        }
    }
}