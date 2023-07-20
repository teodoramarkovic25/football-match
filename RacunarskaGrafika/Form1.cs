using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RacunarskaGrafika
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.White;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int dimenzija = (int)(vratiManjuDimenziju() * 0.9);
            Point centar = new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
            nacrtajTeren(g, centar, dimenzija);

            Bitmap bitmap = new Bitmap(1000, 600);

            Graphics g1 = Graphics.FromImage(bitmap);
            g1.SmoothingMode = SmoothingMode.AntiAlias;

            centar = new Point(500, 300);
            nacrtajTeren(g1, centar, 1000);
            bitmap.Save(@"bitmapPNG.png", ImageFormat.Png);
        }
        public void nacrtajTeren(Graphics g, Point centar, int duzina)
        {
            int sirinaTerena = (int)(0.6 * duzina);
            Point centarForme = centar;
            Point pocetakOvira = new Point(centarForme.X - duzina / 2, (int)(centarForme.Y - (sirinaTerena / 2)));
            Rectangle okvir = new Rectangle(pocetakOvira, new Size(duzina, (int)(0.6 * duzina)));

            for (int i = 0; i < 10; i++)
            {
                Rectangle trava;

                trava = new Rectangle(new Point(pocetakOvira.X + i * (int)(duzina / 10), pocetakOvira.Y), new Size((int)(duzina / 10)+2, (int)(0.6 * duzina)));

                Brush cetka;
                if (i % 2 == 0)
                    cetka = new SolidBrush(Color.Green);
                else
                    cetka = new SolidBrush(Color.DarkGreen);
                g.FillRectangle(cetka, trava);
            }
            g.DrawRectangle(new Pen(Color.Black, 2.5f), okvir);

            nacrtajLinijeTerena(g, pocetakOvira, duzina, sirinaTerena, new Pen(Color.White, 2.5f));

            Point centarTerena = centar;
            nacrtajLoptu(g, centarTerena, sirinaTerena / 15);
            nacrtajIgrace(g, pocetakOvira, duzina, sirinaTerena, sirinaTerena / 10);
        }

        public int vratiManjuDimenziju()
        {
            int sirina = this.ClientRectangle.Width;
            int visina = this.ClientRectangle.Height;
            if (sirina > visina)
                return visina;
            else
                return sirina;
        }

        public void nacrtajIgraca(Graphics g, Point centar, int precnik, Pen olovka, Color bojaIgraca, int broj, Color bojaBroja)
        {
            Rectangle kvadrat = new Rectangle(centar.X - (int)(precnik / 2), centar.Y - (int)(precnik / 2), precnik, precnik);
            g.FillEllipse(new SolidBrush(bojaIgraca), kvadrat);
            g.DrawEllipse(olovka, kvadrat);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(broj + "", new Font("Comic Sans MS", precnik / 2), new SolidBrush(bojaBroja), centar, format);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void nacrtajLinijeTerena(Graphics g, Point pocetakTerena, int duzinaTerena, int sirinaTerena, Pen olovka)
        {
            int margina = (int)(sirinaTerena * 0.07);
            Rectangle autLinija = new Rectangle(pocetakTerena.X + margina, pocetakTerena.Y + margina, duzinaTerena - 2 * margina, sirinaTerena - 2 * margina);
            g.DrawRectangle(olovka, autLinija);

            g.DrawLine(olovka, new Point(pocetakTerena.X + duzinaTerena / 2, autLinija.Y), new Point(pocetakTerena.X + duzinaTerena / 2, pocetakTerena.Y+sirinaTerena-margina));

            int precnikKruga = (int)(0.25 * sirinaTerena);
            g.DrawEllipse(olovka, new Rectangle(pocetakTerena.X + duzinaTerena / 2 - precnikKruga / 2, pocetakTerena.Y + sirinaTerena / 2 - precnikKruga / 2, precnikKruga, precnikKruga));

            int sirinaLinijeGola = (int)(0.35*sirinaTerena);
            int duzinaLinijeGola = (int)(0.15 * duzinaTerena);
            Rectangle linijaGola = new Rectangle(autLinija.X, pocetakTerena.Y+sirinaTerena/2-sirinaLinijeGola/2, duzinaLinijeGola, sirinaLinijeGola);
            g.DrawRectangle(olovka, linijaGola);
            linijaGola = new Rectangle(autLinija.X+autLinija.Width-duzinaLinijeGola, pocetakTerena.Y + sirinaTerena / 2 - sirinaLinijeGola / 2, duzinaLinijeGola, sirinaLinijeGola);
            g.DrawRectangle(olovka, linijaGola);

            Point pocetakPotpisa = new Point(pocetakTerena.X + duzinaTerena - margina, pocetakTerena.Y + margina);
            nacrtajPotpis(g, pocetakPotpisa, sirinaTerena/30, Color.White);

            Point pocetakNaziva = new Point(pocetakTerena.X + margina, pocetakTerena.Y + margina);
            nacrtajNaziv(g, pocetakNaziva, sirinaTerena / 30, Color.White);
        }

        public void nacrtajLoptu(Graphics g, Point centarLopte, int precnik)
        {
            Image lopta = Properties.Resources.lopta;
            g.DrawImage(lopta, new Rectangle(centarLopte.X - precnik / 2, centarLopte.Y - precnik / 2, precnik, precnik), new Rectangle(0, 0, lopta.Width, lopta.Height), GraphicsUnit.Pixel);
        }

        public void nacrtajIgrace(Graphics g, Point pocetakTerena, int duzinaTerena, int sirinaTerena, int precnikIgraca)
        {
            Point pozicijaIgraca = new Point(pocetakTerena.X+duzinaTerena/3, pocetakTerena.Y+sirinaTerena/2);
            Pen olovka = new Pen(Color.Black, 2.5f);
            Color bojaBroja = Color.White;
            Color plavi = Color.DeepSkyBlue;
            Color crveni = Color.OrangeRed;

            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, plavi, 10, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 3.5), pocetakTerena.Y + (int)(sirinaTerena / 1.5));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, plavi, 2, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 3.3));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, plavi, 7, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 1.2));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, plavi, 6, bojaBroja);



            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 1.5));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, crveni, 11, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.7), pocetakTerena.Y + (int)(sirinaTerena / 3.3));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, crveni, 8, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 1.9));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, crveni, 3, bojaBroja);

            pozicijaIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.8), pocetakTerena.Y + (int)(sirinaTerena / 1.5));
            nacrtajIgraca(g, pozicijaIgraca, precnikIgraca, olovka, crveni, 5, bojaBroja);
        }

        public void nacrtajPotpis(Graphics g, Point tacka, float velicinaFonta, Color boja)
        {
            String potpis = "by Petar Peterovic, 1234";
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            format.LineAlignment = StringAlignment.Far;

            Font font = new Font("Chiller", velicinaFonta, FontStyle.Bold | FontStyle.Italic);
            
            g.DrawString(potpis, font, new SolidBrush(Color.Black), tacka, format);
            tacka.X -= 1;
            tacka.Y -= 1;
            g.DrawString(potpis, font, new SolidBrush(boja), tacka, format);
        }
      public void nacrtajNaziv(Graphics g, Point tacka, float velicinaFonta, Color boja)
        {
            String potpis = "Fudbalski teren, 26.04.2018.";
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Far;

            Font font = new Font("Chiller", velicinaFonta, FontStyle.Bold | FontStyle.Italic);

            g.DrawString(potpis, font, new SolidBrush(Color.Black), tacka, format);
            tacka.X -= 1;
            tacka.Y -= 1;
            g.DrawString(potpis, font, new SolidBrush(boja), tacka, format);
        }
    }
}
