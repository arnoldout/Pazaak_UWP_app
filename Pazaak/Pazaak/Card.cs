using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class Card
    {
        private Int16 val;
        private String imgSrc;

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

        public Card(Int16 val)
        {
            this.val = val;
        }
    }
}
