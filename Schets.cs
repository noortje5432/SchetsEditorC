using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class Schets
{
    private Bitmap bitmap;

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

    public class figuur
    {
        public string soort { get; set; }
        public Point beginpunt { get; set; }
        public Point eindpunt { get; set; }
        public string kleur { get; set; }

    }
    public void LijstMaken()
    {
        List<figuur> elementen = new List<figuur>();
    }
}