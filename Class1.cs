using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OpnieuwTekenen
{
    public void opnieuwTekenen(List<Figuren> elementen, Bitmap bitmap, Graphics gr)
    {
        gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

        foreach (Figuren element in elementen)
        {
            Brush kwast = new SolidBrush(element.kleur);
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
                gr.DrawLine(TweepuntTool.MaakPen(kwast, 3), element.beginpunt, element.eindpunt);
            }
            if (element.soort == "pen")
            {
                gr.DrawLine(TweepuntTool.MaakPen(kwast, 3), element.beginpunt, element.eindpunt);
            }
            if (element.soort.Length == 1)
            {
                Font font = new Font("Tahoma", 40);
                SizeF sz =
                gr.MeasureString(element.soort, font, element.beginpunt, StringFormat.GenericTypographic);
                gr.DrawString(element.soort, font, kwast,
                                                element.beginpunt, StringFormat.GenericTypographic);
            }

        }
        bitmap.Invalidate();
    }
}
