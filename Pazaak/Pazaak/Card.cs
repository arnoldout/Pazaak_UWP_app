using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    public class Card
    {
        //variables of a card object
        //i chose int16 as the value of a card will only ever be at most 2 digits
        private Int16 val;
        //imgSrc will hold the corresponding source for the card
        //i.e if card is positive it will have a different image to that of a negative card
        private String imgSrc;
        private Random rnd;
        private Boolean isUsed;

        //getters and setters for variables
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

        public bool IsUsed
        {
            get
            {
                return isUsed;
            }

            set
            {
                isUsed = value;
            }
        }

        //constructors for card

        //this constructor allows a card to be created based off of the provided integer value
        public Card(Int16 val)
        {
            this.val = val;
            this.isUsed = false;
        }
        //this constructor randomly generates a value for the card
        public Card()
        {
            this.rnd = new Random();
            Int16 value = (Int16)this.rnd.Next(1, 11);
            this.val = value;
            this.isUsed = false;
        }
        //this constructor creates a card and uses an already established seed as the random object
        public Card(Random r)
        {
            this.rnd = r;
            Int16 value = (Int16)this.rnd.Next(1, 11);
            this.val = value;
            this.isUsed = false;
        }
    }
}