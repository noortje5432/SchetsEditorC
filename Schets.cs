using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

public class Schets
{
    private Bitmap bitmap;
    public List<Figuren> elementen = new List<Figuren>(); //
    public List<string> nieuw = new List<string>();
    public List<Figuren> figuren = new List<Figuren>();

    public Schets()
    {
        bitmap = new Bitmap(1, 1);
    }
    public Graphics BitmapGraphics
    {
        get { return Graphics.FromImage(bitmap); }
    }
    public void VeranderAfmeting(Size sz)
    {
        if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
        {
            Bitmap nieuw = new Bitmap(Math.Max(sz.Width, bitmap.Size.Width)
                                     , Math.Max(sz.Height, bitmap.Size.Height)
                                     );
            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
            gr.DrawImage(bitmap, 0, 0);
            bitmap = nieuw;
        }
    }
    public void Teken(Graphics gr)
    {
        gr.DrawImage(bitmap, 0, 0);
    }
    public void Schoon()
    {
        Graphics gr = Graphics.FromImage(bitmap);
        gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        elementen.Clear();
    }
    public void Roteer()
    {
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
    }

    public void schrijfNaarFile(string Text)                      //nieuw
    {
        bitmap.Save(Text, ImageFormat.Png);
    }

    public void LeesVanFile(string bestandNaam)
    {
        Image image1 = Image.FromFile(bestandNaam);
        Bitmap bpm = new Bitmap(image1);
        bitmap = bpm;
    }

    public void ExporteerNaarTekst(string bestandNaam)
    {
        TextWriter tw = new StreamWriter(bestandNaam);

        foreach (Figuren i in figuren)
            tw.WriteLine(string.Format("soort: {0} - beginpunt: {1} - eindpunt: {2} - kleur: {3}", i.soort, i.beginpunt.ToString(), i.eindpunt.ToString(), i.kleur));

        tw.Close();

    }

    public void ImporteerVanTekst(string bestandNaam)
    {
        string BestandInhoud = File.ReadAllText(bestandNaam);
        String[] seperator = { "soort: ", " - beginpunt: {X=", ",Y=", "} - eindpunt: {X=", ",Y=", "} - kleur: " };
        List<string> element = BestandInhoud.Split(seperator, StringSplitOptions.RemoveEmptyEntries).ToList();
        for (int i = 0; i < element.Count; i += 6) 
        {
            int xb = int.Parse(element[i + 1]);
            int yb = int.Parse(element[i + 2]);
            int xe = int.Parse(element[i + 3]);
            int ye = int.Parse(element[i + 4]);
            Point begin = new Point(xb, yb);
            Point eind = new Point(xe, ye);
            Color kleur = (Color)Color.FromName(element[i + 5]);
            figuren.Add(new Figuren(element[i], begin, eind, kleur));
        }
        
    }
}