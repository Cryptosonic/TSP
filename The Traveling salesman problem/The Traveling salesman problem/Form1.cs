using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Traveling_salesman_problem
{
    
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();
            lbxRep.SelectedIndex = 0;                                            // Väljer automatiskt alternativ 1 när programet startas 
            lbxAlgo.SelectedIndex = 0;
            tbxPunkter.Text = "2";
            if (String.IsNullOrEmpty(tbxSeed.Text))
            {
                Random rd = new Random();
                int SeedInfo = rd.Next(0, 999999);
                tbxSeed.Text = Convert.ToString(SeedInfo);
            }           
        }
        
        string Tempo;
        string Tempo1;
        string Tempo2;
        int PunkterTotal;                       // Declarerar varibaler jag vill ska vara pubilc
        int RepTotal;
        int Seed;
        int PunkterTotal2;
        bool StatPå;
        double TidÖver500;
        Random rd1;
        Random rd2;
        Point[] Punkt;
        Pen RedPen = new Pen(Color.Red);
        Pen BluePen = new Pen(Color.Blue);
        Pen GreenPen = new Pen(Color.FromArgb(0, 180, 0));
        Pen BrownPen = new Pen(Color.FromArgb(135, 135, 0));
        Point[] PunktK;                                             // Dessa 3 deklareras här pga kan inte använda dem annars
        double totalDistance;                               
        double minDistance;
        
        public void btnStart_Click(object sender, EventArgs e)                  // STARTA PROGRAMMET
        {

            if (btnStart.Text == "Start")                                        // Toggle av eller på. 
            {
                            
                btnStart.Text = ("Stopp");
                

                if (String.IsNullOrEmpty(tbxSeed.Text))
                {
                    Random rd = new Random();
                    int SeedInfo = rd.Next(0, 999999);                          // FAILSAFE SEED
                    tbxSeed.Text = Convert.ToString(SeedInfo);
                }
                
                if (  (String.IsNullOrEmpty(tbxPunkter.Text)) ||  ((Convert.ToInt32(tbxPunkter.Text) > 11) && (lbxAlgo.SelectedIndex == 0))) // FAILSAFE PUNKTER
                {
                    tbxPunkter.Text = Convert.ToString(3);          // om fört stort antal punkter med Brute force metoden 
                }
                if (((Convert.ToInt32(tbxPunkter.Text) > 10) && (lbxAlgo.SelectedIndex == 0)) && (Convert.ToInt32(lbxRep.SelectedIndex) > 0 )) // FAILSAFE REPITIONER
                {
                    tbxPunkter.Text = Convert.ToString(3);          // om för stort antal repititioner med brute force metoden 
                }


                Graphics g = GBDraw.CreateGraphics();              
                Pen SvartPen = new Pen(Color.Gray);
                if (GBDraw.Enabled == true)
                {
                    g.DrawRectangle(SvartPen, 49, 49, 302, 302);
                    g.DrawRectangle(SvartPen, 399, 49, 302, 302);                   // temp Rita boxar att ha prickar i 
                    g.DrawRectangle(SvartPen, 49, 399, 302, 302);
                    g.DrawRectangle(SvartPen, 399, 399, 302, 302);
                }

                Tempo = tbxPunkter.Text.ToString();
                Tempo1 = lbxRep.SelectedItem.ToString();
                Tempo2 = tbxSeed.Text.ToString();              
                PunkterTotal = Convert.ToInt32(Tempo);
                RepTotal = Convert.ToInt32(Tempo1);
                Seed = Convert.ToInt32(Tempo2);
                rd1 = new Random(Seed);
                rd2 = new Random(Seed);
                Punkt = new Point[PunkterTotal];
                double MDBruteForce = 0;
                double MDNärmstagrannenKlassisk = 0;                    // till alla algoritmer 
                double MDSlump = 0;
                double MDMyrkoloni = 0;
                double MTBruteForce = 0;
                double MTNärmstagrannenKlassisk = 0;
                double MTSlump = 0;
                double MTMyrkoloni = 0;

                Graphics g0 = pBPlan1.CreateGraphics();             // Steg 2 start 
                Graphics g1 = pBPlan2.CreateGraphics();
                Graphics g2 = pBPlan3.CreateGraphics();
                Graphics g3 = pBPlan4.CreateGraphics();
                for (int i = 1; i < (RepTotal+1); i++)             // repetera för antalet lbxRep. central loop
                {
                    for (int o = 0; o < PunkterTotal; o++)
                    {
                        
                        Punkt[o].X = rd1.Next(0, 296);                      // generera slump punkter 
                        Seed = ((Seed * 37) + 743);                  
                        Punkt[o].Y = rd1.Next(0, 296);

                        
                        SolidBrush SvartP = new SolidBrush(Color.Black);
                        if (GBDraw.Enabled == true)
                        {
                            g0.FillEllipse(SvartP, Punkt[o].X, Punkt[o].Y, 5, 5);
                            g1.FillEllipse(SvartP, Punkt[o].X, Punkt[o].Y, 5, 5);                   // rita punkterna i olika plan
                            g2.FillEllipse(SvartP, Punkt[o].X, Punkt[o].Y, 5, 5);
                            g3.FillEllipse(SvartP, Punkt[o].X, Punkt[o].Y, 5, 5);
                        }
                    }
                    if (GBDraw.Enabled == true)
                    {
                        g0.DrawEllipse(BluePen, Punkt[0].X - 3, Punkt[0].Y - 3, 11, 11);
                        g1.DrawEllipse(BluePen, Punkt[0].X - 3, Punkt[0].Y - 3, 11, 11);
                        g2.DrawEllipse(BluePen, Punkt[0].X - 3, Punkt[0].Y - 3, 11, 11);
                        g3.DrawEllipse(BluePen, Punkt[0].X - 3, Punkt[0].Y - 3, 11, 11);        // Steg 2 slut
                    }
                    if (lbxAlgo.SelectedIndex == 0)         // Steg 4 Start
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();            // start timer 
                        
                        AlgBruteForce();
                        MDBruteForce = MDBruteForce + (Convert.ToDouble(lblBruteM.Text));
                        lblBruteM.Text = Convert.ToString(MDBruteForce / i); // skapar medelvärdet genom att spara tidiage värden och dividera på i = RepTotal
                        
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;          // stop timer

                        MTBruteForce = MTBruteForce + elapsedMs;
                        lblBruteT.Text = Convert.ToString(MTBruteForce / i);
                    }
                    else if (lbxAlgo.SelectedIndex == 1)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        AlgNärmstagrannenKlassisk();
                        MDNärmstagrannenKlassisk = MDNärmstagrannenKlassisk + (Convert.ToDouble(lblAlgoM.Text));
                        lblAlgoM.Text = Convert.ToString(MDNärmstagrannenKlassisk / i);

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;          

                        MTNärmstagrannenKlassisk = MTNärmstagrannenKlassisk + elapsedMs;
                        lblAlgoT.Text = Convert.ToString(MTNärmstagrannenKlassisk / i);
                    }
                    else if (lbxAlgo.SelectedIndex == 2)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        AlgSlump();
                        MDSlump = MDSlump + (Convert.ToDouble(lblAlgoM.Text));
                        lblAlgoM.Text = Convert.ToString(MDSlump / i);

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;

                        MTSlump = MTSlump + elapsedMs;
                        lblAlgoT.Text = Convert.ToString(MTSlump / i);
                    }
                    else if (lbxAlgo.SelectedIndex == 3)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        AlgMyrkoloni();
                        MDMyrkoloni = MDMyrkoloni + (Convert.ToDouble(lblAlgoM.Text));
                        lblAlgoM.Text = Convert.ToString(MDMyrkoloni / i);

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;

                        MTMyrkoloni = MTMyrkoloni + elapsedMs;
                        lblAlgoT.Text = Convert.ToString(MTMyrkoloni / i);
                    }
                    // steg 4 slut
                    // 1500x800 orgianl storklek föster
                    // 415, 210 storklek statEavP location 38, 456
                    // 415, 210 storklek statSavP loc 38, 236
                }
                if (lbxAlgo.SelectedIndex == 0)         // steg 8 start
                {
                    StatSavP.Series["Brute Force"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblBruteM.Text), 2));
                    if (Math.Round(Convert.ToDouble(lblBruteT.Text), 4) > 500)
                    {
                        TidÖver500 = Math.Round(Convert.ToDouble(lblBruteT.Text), 4) - 500;
                    }
                    else
                    {
                        TidÖver500 = 0;
                    }
                    StatEavP.Series["Brute Force"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblBruteM.Text), 2) * (1 + ((TidÖver500 / 1000) * (TidÖver500 / 500))));
                }
                else if (lbxAlgo.SelectedIndex == 1)
                {
                    StatSavP.Series["Närmsta Grannen"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2));
                    if (Math.Round(Convert.ToDouble(lblAlgoT.Text), 4) > 500)
                    {
                        TidÖver500 = Math.Round(Convert.ToDouble(lblAlgoT.Text), 4) - 500;
                    }
                    else
                    {
                        TidÖver500 = 0;
                    }
                    StatEavP.Series["Närmsta Grannen"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2) * (1 + ((TidÖver500 / 1000) * (TidÖver500 / 500))));
                }
                else if (lbxAlgo.SelectedIndex == 2)
                {
                    StatSavP.Series["Slump"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2));
                    if (Math.Round(Convert.ToDouble(lblAlgoT.Text), 4) > 500)
                    {
                        TidÖver500 = Math.Round(Convert.ToDouble(lblAlgoT.Text), 4) - 500;
                    }
                    else
                    {
                        TidÖver500 = 0;
                    }
                    StatEavP.Series["Slump"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2) * (1 + ((TidÖver500 / 1000) * (TidÖver500 / 500))));
                }
                else if (lbxAlgo.SelectedIndex == 3)
                {
                    StatSavP.Series["Myrkoloni"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2));
                    if (Math.Round(Convert.ToDouble(lblAlgoT.Text), 4) > 500)
                    {
                        TidÖver500 = Math.Round(Convert.ToDouble(lblAlgoT.Text), 4)-500;
                    }
                    else
                    {
                        TidÖver500 = 0;
                    }
                    StatEavP.Series["Myrkoloni"].Points.AddXY(PunkterTotal, Math.Round(Convert.ToDouble(lblAlgoM.Text), 2)*(1+((TidÖver500/1000)*(TidÖver500/500))) );
                }

                    if ((String.IsNullOrEmpty(lblBruteM.Text) == false) && (String.IsNullOrEmpty(lblAlgoM.Text) == false)) 
                    {
                        lblDistansProcent.Text = Convert.ToString( (Convert.ToDouble(lblAlgoM.Text)) / (Convert.ToDouble(lblBruteM.Text)) ); 
                    }
                if ((String.IsNullOrEmpty(lblBruteT.Text) == false) && (String.IsNullOrEmpty(lblAlgoT.Text) == false)) 
                     {
                        lblTidProcent.Text = Convert.ToString((Convert.ToDouble(lblAlgoT.Text)) / (Convert.ToDouble(lblBruteT.Text))); 
                     }

            }               // Steg 8 slut 
            else
            {
                btnStart.Text = ("Start");
            }
            if (btnStart.Text == ("Stopp"))         // efter loop som kör programmet görs detta för att automisera data collection
            { 
                if ((StatPå == true) && (lbxAlgo.SelectedIndex == 3))     
                {
                    lbxAlgo.SelectedIndex = 1;              // byt till 0 om du vill ha med brute force metoden
                    PunkterTotal2 = PunkterTotal + 0;
                    tbxPunkter.Text = Convert.ToString(PunkterTotal2);
                }
                else if (StatPå == true)     
                {
                lbxAlgo.SelectedIndex = Convert.ToInt32(lbxAlgo.SelectedIndex) + 1;
                }  
            }
        }

        public double CalcDistance(Point number1, Point number2)                    // räknar sträckan mellan två punkter 
        {
            int x1 = number1.X;
            int x2 = number2.X;
            int y1 = number1.Y;
            int y2 = number2.Y;
            double Distance = Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            return Distance;
        }

        public void AlgSlump()                                          // pseudo random mellan punkterna 
        {
            Graphics g0 = pBPlan3.CreateGraphics();                     // Steg 3 start (bara denna fnktion)
            double TotalDistanceS = 0;
            for (int p = 0; p < (PunkterTotal - 1); p++)
            {
                TotalDistanceS = TotalDistanceS + CalcDistance(Punkt[p], Punkt[p + 1]);
                if (GBDraw.Enabled == true)
                {
                    g0.DrawLine(GreenPen, Punkt[p], Punkt[p + 1]);                   // ritar från punkt 0-1-2... så vidare
                }
            }
            lblAlgoM.Text = Convert.ToString(TotalDistanceS);
        }
        public void AlgNärmstagrannenKlassisk()                            // närsmta grannen metoden utan optimisering
        {                                                                   // Steg 5 start 
            double DistanceMin = 1000000;
            Point MinPunkt = new Point(0,0);
            Point Temp = new Point(0,0);
            int PunktIndex=0;
            int q = 0;
            double TotalDistanceNGK = 0;
            for (int o = 0; o < (PunkterTotal-1); o++)
            {
                for (int i = (0 + q); i < (PunkterTotal - 1); i++)
                {
                    if (DistanceMin > CalcDistance(Punkt[q], Punkt[i + 1]))             //räknar den kortsate punkten från start 
                    {
                        DistanceMin = CalcDistance(Punkt[q], Punkt[i + 1]);
                        MinPunkt = (Punkt[i + 1]);
                        PunktIndex = (i + 1);
                    }                   
                }              
                Graphics g0 = pBPlan2.CreateGraphics();

                if (GBDraw.Enabled == true)
                {
                    g0.DrawLine(BluePen, Punkt[q], MinPunkt);                 
                }

                Temp = Punkt[q + 1];
                Punkt[q + 1] = MinPunkt;                        // byter plats så att den inte missar att tästa saker som den ska
                Punkt[PunktIndex] = Temp;
                TotalDistanceNGK = (TotalDistanceNGK + DistanceMin);
                DistanceMin = 1000000;
                q++;
            }
            lblAlgoM.Text = Convert.ToString(TotalDistanceNGK);             // Steg 5 slut
        }
        public void AlgBruteForce ()                            // Steg  start 
        {
            Graphics g0 = pBPlan1.CreateGraphics();
            PunktK = new Point[PunkterTotal]; 
            totalDistance=0;
            minDistance = 1000000;
            int q = 0;                                                  
            int[] TempPermutation = new int[PunkterTotal-1];
            for (int i = 1; i < PunkterTotal; i++)
            {
                TempPermutation[i-1] = i;
            }
            GetPer(TempPermutation);

            if (GBDraw.Enabled == true) 
            { 
                g0.DrawLine(RedPen, Punkt[0], PunktK[0]);
                 for (int i = 0; i < (PunkterTotal - 2); i++)
                 {
                     g0.DrawLine(RedPen, PunktK[i], PunktK[i + 1]);
                 }
            }

            lblBruteM.Text = Convert.ToString(minDistance);                             
                               
        }
        private static void Swap(ref int a, ref int b)
        {
            if (a == b) return;

            var temp = a;
            a = b;
            b = temp;
        }

        public void GetPer(int[] list)
        {
            int x = list.Length - 1;
            GetPer(list, 0, x );
        }

        private void GetPer(int[] list, int k, int m)
        {
            if (k == m)
            {
                totalDistance = 0;
                totalDistance = totalDistance + CalcDistance(Punkt[0], Punkt[list[0]]); 
                for (int o = 0; o < (PunkterTotal - 2); o++)
                {
                    totalDistance = totalDistance + CalcDistance(Punkt[list[o]], Punkt[list[o + 1]]);
                }

                if (minDistance > totalDistance)
                {
                    minDistance = totalDistance;
                    for (int p = 0; p < (PunkterTotal-1); p++)
                    {
                        PunktK[p] = Punkt[list[p]];                       // Sparar om det var den kortsate distansen hitils för att sedan rita mellan PunktK[0]....
                    }
                }
            }
        
            else
                for (int i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    GetPer(list, k + 1, m);
                    Swap(ref list[k], ref list[i]);
                }
        }                                                               // Steg 6 slut 

       
        

        public void AlgMyrkoloni()                              // steg 7 start 
        {
            int Myror = 1000;
            double Pheromone = 2;

            Graphics g0 = pBPlan4.CreateGraphics();
            PunktK = new Point[PunkterTotal];
            double DistanceMin=99999999;
            double DistanceTotal=0;
            double DistanceTemp=0;
            int[] PunktTemp = new int[PunkterTotal];
            int[] PunktTemp2 = new int[PunkterTotal];
            List<int> ListItems = new List<int>();
            bool FirstGo = false;
            
            int k = 1;
            int n = 0;
            double Path = 0;
            var PunktS = Tuple.Create(1, 2);
            int PunktNu = 0;
            int PunktNext = 0;
            double test1 = 0;
            double[,] CostMatrix = new double[PunkterTotal, PunkterTotal];
            double[,] CostMatrix2 = new double[PunkterTotal, PunkterTotal];
            double[,] PheromoneMatrix = new double[PunkterTotal, PunkterTotal];
            double[] ProbArray = new double[PunkterTotal];
            double[] ProbArray2 = new double[PunkterTotal];
            double[] SumArray = new double[PunkterTotal];
            double[] SumProbArray = new double[PunkterTotal];

            double[,] ProbMatrix = new double[PunkterTotal, PunkterTotal];  // neh
            double[,] ProbMatrix2 = new double[PunkterTotal, PunkterTotal]; // neh
            
            double[] SumProbMatrix = new double[PunkterTotal];  // neh 
            for (int u = 0; u < Myror; u++)
            {
                DistanceTotal = 0;
                k = 1;
                PunktNu = 0;
                ListItems.Clear();
                
                for (int p = 0; p < PunkterTotal; p++)
                {
                    SumArray[p] = 0;
                }
                for (int i = 0; i < PunkterTotal; i++)
                {

                    for (int o = 0; o < PunkterTotal; o++)
                    {
                        if (i != o)
                        {
                            if (FirstGo == false)
                            {
                                CostMatrix[i, o] = CalcDistance(Punkt[i], Punkt[o]);
                                CostMatrix2[i, o] = CalcDistance(Punkt[i], Punkt[o]);
                                PheromoneMatrix[i, o] = 1;
                                
                            }
                            else
                                CostMatrix[i, o] = CostMatrix2[i, o];
                                
                        }
                    }

                }
                for (int i = 0; i < PunkterTotal-1; i++)    // myran går alla steg 
                {
                    n = 0;
                    for (int o = 0; o < PunkterTotal; o++)
                    {
                        ProbArray[o] = 0;
                        ProbArray2[o] = 0;
                        SumProbArray[o] = 0;
                    }
                    for (int o = 0; o < PunkterTotal; o++)    // sumarray räkna 
                    {
                        if (PunktNu != o && ListItems.Contains(o) == false)
                        { 
                            SumArray[PunktNu] = (SumArray[PunktNu] + ((1 / CostMatrix[PunktNu, o]) * PheromoneMatrix[PunktNu, o])); 
                        }                       
                    }
                    for (int o = 0; o < PunkterTotal; o++)  // prob array räkna 
                    {
                        if (PunktNu == o || ListItems.Contains(o) == true)
                        {
                            ProbArray[o] = 0;
                            ProbArray2[o] = 0;
                        }
                        else
                        {
                            ProbArray[o] = (((1 / CostMatrix[PunktNu, o]) * PheromoneMatrix[PunktNu, o]) / (SumArray[PunktNu]));    // rumarray can vara fel 
                            ProbArray2[o] = ProbArray[o];
                        }
                        
                    }
                    Path = rd2.Next(1, 1000000);
                    Path = Path / 1000000;
                    Seed = (Seed * 37) + 743;
                    Array.Sort(ProbArray);
                    for (int o = 0+k-1; o < PunkterTotal; o++)
                    {
                        for (int p = 0+k-1; p < PunkterTotal-n; p++)
                        {
                            SumProbArray[o] = SumProbArray[o] + ProbArray[p];
                        }
                        n++;
                    }
                    
                    for (int o = 0+k-1; o < PunkterTotal-1; o++)    // myran går ett steg itaget 
                    {
                        if ((SumProbArray[o] >= Path) && (Path > SumProbArray[o+1]))
                        {
                            test1 = SumProbArray[o] - SumProbArray[o + 1];
                            test1 = Math.Truncate(1000000 * test1) / 1000000;
                            for (int p = 0; p < PunkterTotal; p++)
                            {
                                ProbArray2[p] = Math.Truncate(1000000 * ProbArray2[p]) / 1000000;
                            }

                            PunktNext = Array.IndexOf(ProbArray2, test1);
                            PheromoneMatrix[PunktNu, PunktNext] = PheromoneMatrix[PunktNu, PunktNext] + Pheromone;
                            ListItems.Add(PunktNu);
                            DistanceTemp = CostMatrix[PunktNu, PunktNext];
                            PunktTemp2[k] = PunktNext;

                            for (int p = 0; p < PunkterTotal; p++)
                            {
                                CostMatrix[p, PunktNu] = 0;
                                CostMatrix[PunktNu, p] = 0;                              
                            }
                            CostMatrix[PunktNu, PunktNext] = 0;
                            CostMatrix[PunktNext, PunktNu] = 0;


                            


                            PunktNu = PunktNext;    // sista som görs 
                        }
                    }
                    DistanceTotal = DistanceTotal + DistanceTemp;
                    k++;
                }
                if (DistanceTotal < DistanceMin)
                {
                    DistanceMin = DistanceTotal;
                    for (int i = 1; i < PunkterTotal; i++)
                    {
                        PunktTemp[i] = PunktTemp2[i];
                    } 
                }
                FirstGo = true;
            }
            if (GBDraw.Enabled == true)
            {
                g0.DrawLine(BrownPen, Punkt[0], Punkt[PunktTemp[0]]);
                for (int i = 0; i < PunkterTotal - 1; i++)
                {
                    g0.DrawLine(BrownPen, Punkt[PunktTemp[i]], Punkt[PunktTemp[i + 1]]);
                }
            }
            lblAlgoM.Text = Convert.ToString(DistanceMin);                // Steg 7 slut
        }
        

      
        private void btnDraw_Click(object sender, EventArgs e) // Dissable Ritandet
        {
            if (GBDraw.Enabled == true)
            {
                GBDraw.Enabled = false;
                btnDraw.Text = "Rita är av";
            }
            else
            {
                GBDraw.Enabled = true;
                btnDraw.Text = "Rita är på";
            }        
        }                                                       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnResetGB_Click(object sender, EventArgs e)
        {           
                GBDraw.Enabled = false;                           
                GBDraw.Enabled = true;               
        }

        private void btnStetistikAuto_Click(object sender, EventArgs e)
        {
            
            if (btnStetistikAuto.Text == "Auto statistik på")
            {
                btnStetistikAuto.Text = "Auto statistik av";
                StatPå = false;
            }
            else if (btnStetistikAuto.Text == "Auto statistik av")
            {
                btnStetistikAuto.Text = "Auto statistik på";                
                StatPå = true;
            }
        }
    }
}
