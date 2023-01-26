using Microsoft.VisualBasic.ApplicationServices;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

public interface ISchetsTool
{
    void MuisVast(SchetsControl s, Point p);
    void MuisDrag(SchetsControl s, Point p);
    void MuisLos(SchetsControl s, Point p);
    void Letter(SchetsControl s, char c);
}

public abstract class StartpuntTool : ISchetsTool
{
    protected Point startpunt;
    protected Brush kwast;

    public virtual void MuisVast(SchetsControl s, Point p)
    {   startpunt = p;
    }
    public virtual void MuisLos(SchetsControl s, Point p)
    {   
        kwast = new SolidBrush(s.PenKleur);
    }
    public abstract void MuisDrag(SchetsControl s, Point p);
    public abstract void Letter(SchetsControl s, char c);
}

public class VlakGumTool : StartpuntTool
{
    public override string ToString() { return "gum"; }

    public void bovensteElement(SchetsControl s, Point p)
    {
        List<int> mogelijkheden = new List<int>();
        int getal = s.schets.elementen.Count;
        Point klikpunt = p;
        int i = 0;
        while (i < getal)
        {
            if (s.schets.elementen[i].beginpunt.X <= klikpunt.X && s.schets.elementen[i].eindpunt.X <= klikpunt.X)
            {
                if (s.schets.elementen[i].beginpunt.Y <= klikpunt.Y && s.schets.elementen[i].eindpunt.Y <= klikpunt.Y)
                {
                    mogelijkheden.Add(i);
                }
            }
        }

        if (mogelijkheden.Count != 0)
        {
            s.schets.elementen.RemoveAt(mogelijkheden[mogelijkheden.Count - 1]);
            opnieuwTekenen(s.schets.elementen, s);
        }

    }

    public void opnieuwTekenen(List<Figuren> elementen, SchetsControl s)
    {
        Graphics gr = s.MaakBitmapGraphics();
        gr.FillRectangle(Brushes.White, 0, 0, s.Width, s.Height);

        foreach (Figuren element in elementen)
        {
            if (element.soort == "kader")
            {
                gr.DrawRectangle(TweepuntTool.MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(element.beginpunt, element.eindpunt));
            }
            if (element.soort == "vlak")
            {
                gr.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(element.beginpunt, element.eindpunt));
            }
            if (element.soort == "ellips")
            {
                gr.DrawEllipse(TweepuntTool.MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(element.beginpunt, element.eindpunt));
            }
            if (element.soort == "bol")
            {
                gr.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(element.beginpunt, element.eindpunt));
            }
            if (element.soort == "lijn")
            {
                gr.DrawLine(TweepuntTool.MaakPen(this.kwast, 3), element.beginpunt, element.eindpunt);
            }
            if (element.soort == "pen")
            {

            }
        }
        s.Invalidate();
    }

    public override void Letter(SchetsControl s, char c) { }

    public override void MuisDrag(SchetsControl s, Point p)
    {
        bovensteElement(s, p);
    }
}

public class TekstTool : StartpuntTool
{
    public override string ToString() { return "tekst"; }

    public override void MuisDrag(SchetsControl s, Point p) { }

    public override void Letter(SchetsControl s, char c)
    {
        if (c >= 32)
        {
            Graphics gr = s.MaakBitmapGraphics();
            Font font = new Font("Tahoma", 40);
            string tekst = c.ToString();
            SizeF sz =
            gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
            gr.DrawString(tekst, font, kwast,
                                            this.startpunt, StringFormat.GenericTypographic);
            // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
            Point beginpunt = startpunt;
            startpunt.X += (int)sz.Width;
            Point eindpunt = startpunt;
            Figuren letter = new figuur(soort, beginpunt, eindpunt, kleur) { soort = "tekst", beginpunt = beginpunt, eindpunt = eindpunt, kleur = "zwart" };
            s.schets.elementen.Add(letter);
            s.Invalidate();
        }
    }
}


public abstract class TweepuntTool : StartpuntTool
{
    public static Rectangle Punten2Rechthoek(Point p1, Point p2)
    {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                            , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                            );
    }
    public static Pen MaakPen(Brush b, int dikte)
    {   Pen pen = new Pen(b, dikte);
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;
        return pen;
    }
    public override void MuisVast(SchetsControl s, Point p)
    {   base.MuisVast(s, p);
        kwast = Brushes.Gray;
    }
    public override void MuisDrag(SchetsControl s, Point p)
    {   s.Refresh();
        this.Bezig(s.CreateGraphics(), this.startpunt, p);
    }
    public override void MuisLos(SchetsControl s, Point p)
    {   base.MuisLos(s, p);
        this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
        s.schets.elementen.Add(null);
        s.Invalidate();
    }
    public override void Letter(SchetsControl s, char c)
    {
    }
    public abstract void Bezig(Graphics g, Point p1, Point p2);
        
    public virtual void Compleet(Graphics g, Point p1, Point p2)
    {   this.Bezig(g, p1, p2);
    }
}

public class RechthoekTool : TweepuntTool
{
    public override string ToString() { return "kader"; }


    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }
    public override void MuisLos(SchetsControl s, Point p)
    {
        base.MuisLos(s, p);
        Figuren kader = new figuur() { soort = "kader", beginpunt = p1, eindpunt = p2, kleur = "zwart" };
        s.schets.elementen.Add(kader);
        s.Invalidate();
    }

}

public class VolRechthoekTool : RechthoekTool
{
    public override string ToString() { return "vlak"; }

    public override void Compleet(Graphics g, Point p1, Point p2)
    {   g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        figuur vlak = new figuur() { soort = "vlak", beginpunt = p1, eindpunt = p2, kleur = "zwart" };
        s.schets.elementen.Add(vlak);
    }
}

//Nieuwe methodes cirkel en volcirkel

public class EllipsTool : TweepuntTool
{
    public override string ToString() { return "ellips"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1,p2));
    }

    public override void MuisLos(SchetsControl s, Point p)
    {
        base.MuisLos(s, p);
        Figuren kader = new figuur() { soort = "ellips", beginpunt = p1, eindpunt = p2, kleur = "zwart" };
        s.schets.elementen.Add(kader);
        s.Invalidate();
    }
}

public class VolEllipsTool : EllipsTool
{
    public override string ToString() { return "bol"; }

    public override void Compleet(Graphics g, Point p1, Point p2)
    {
        g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        figuur bol = new figuur() { soort = "bol", beginpunt = p1, eindpunt = p2, kleur = "zwart" };
        elementen.Add(bol);
    }
}

public class LijnTool : TweepuntTool
{
    public override string ToString() { return "lijn"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawLine(MaakPen(this.kwast,3), p1, p2);
        figuur lijn = new figuur() { soort = "lijn", beginpunt = p1, eindpunt = p2, kleur = "zwart" };
        elementen.Add(lijn);
    }
}

public class PenTool : LijnTool
{
    public override string ToString() { return "pen"; }

    public override void MuisDrag(SchetsControl s, Point p)
    {   this.MuisLos(s, p);
        this.MuisVast(s, p);
        figuur pen = new figuur() { soort = "pen", beginpunt = p, eindpunt = p, kleur = "zwart" };
        elementen.Add(pen);
    }
}
    
public class GumTool : PenTool
{
    public override string ToString() { return "gum"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawLine(MaakPen(Brushes.White, 7), p1, p2);
    }
}