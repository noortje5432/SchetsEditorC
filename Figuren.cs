using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*Figuren is een nieuwe klasse met een constructormethode.
Deze constructor maakt "Figuren". Dit is een object
met wat voor soort tekening het is, het begin- en eindpunt 
en de kleur. Deze objecten worden in de lijsten gebruikt.*/

public class Figuren
    {
        public string soort { get; set; }
        public Point beginpunt { get; set; }
        public Point eindpunt { get; set; }
        public Color kleur { get; set; }
        public Figuren(string soort, Point beginbunt, Point eindpunt, Color kleur)
        {
            
        }
    }


