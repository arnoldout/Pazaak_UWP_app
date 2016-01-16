using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class Player
    {
        private Card[] crdPool;
        private Card[] hand;
        private String name;
        private int rndsWn;
        private int gmsWn;
        private int currScr;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int RndsWn
        {
            get
            {
                return rndsWn;
            }

            set
            {
                rndsWn = value;
            }
        }

        public int GmsWn
        {
            get
            {
                return gmsWn;
            }

            set
            {
                gmsWn = value;
            }
        }

        public int CurrScr
        {
            get
            {
                return currScr;
            }

            set
            {
                currScr = value;
            }
        }

        public Player(Card[] crdPool, String name) {
            this.crdPool = crdPool;
            this.hand = genHnd(crdPool);
            this.rndsWn = 0;
            this.gmsWn = 0;
            this.name = name;
            this.currScr = 0;   
        }
        public Card[] genHnd(Card [] crdPool)
        {
            Card[] hand = new Card[4];
            for(int genLoop = 0; genLoop<4; genLoop++)
            {
                Random r = new Random();
                int card = r.Next(0, crdPool.Length);
                hand[genLoop] = crdPool[card]; 
            }
            return hand;
        }
    }
}

