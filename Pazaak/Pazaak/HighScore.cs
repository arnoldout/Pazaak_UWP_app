using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class HighScore
    {
        int usr;
        int en;
        public HighScore(int usr, int en)
        {
            this.usr = usr;
            this.en = en;
        }
        public void readScores()
        { 
           /// Uri i = (new Uri(base.BaseUri, "~/Resources/CardPos.png"));
            String[] values = File.ReadAllText(@"~/Scores.csv").Split(',');
            this.usr += Convert.ToInt32(values[0]);
            this.en += Convert.ToInt32(values[0]);
        }
        public void writeScores()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.usr+","+this.en);
            File.WriteAllText("Scores.csv",sb.ToString());
        }
    }
}
