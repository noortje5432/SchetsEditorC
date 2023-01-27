using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;


public class SchetsWin : Form
{   
    MenuStrip menuStrip;
    SchetsControl schetscontrol;
    ISchetsTool huidigeTool;
    Panel paneel;
    bool vast;

    private void veranderAfmeting(object o, EventArgs ea)
    {
        schetscontrol.Size = new Size ( this.ClientSize.Width  - 70
                                      , this.ClientSize.Height - 50);
        paneel.Location = new Point(64, this.ClientSize.Height - 30);
    }

    private void klikToolMenu(object obj, EventArgs ea)
    {
        this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
    }

    private void klikToolButton(object obj, EventArgs ea)
    {
        this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
    }

    private void opslaan(object o, EventArgs ea)        // In deze methode wordt het dropdownitem opslaan beschreven.
    {                                                   // Wanneer er op opslaan gedrukt wordt zal de code in werking gesteld worden
        if (Text == "")                                 // Als de file al een keer opgeslagen is zal deze opniew opgeslagen en overgeschreven worden.
            opslaanAls(o, ea);                          // Als de file nog niet is opgeslagen, zal het "opslaan als" menu openen.
        else
            schetscontrol.brug(Text);
    }

    public void opslaanAls(object o, EventArgs ea)     //Hier wordt de methode opslaanAls beschreven. Wanneer er op de dropdownitem gedrukt wordt zal er een SaveFileDialog openen.
    {                                                       //Hier kan de bitmap als jpg, bmp, png en als txt file opgeslagen worden.
        SaveFileDialog dialoog = new SaveFileDialog();
        dialoog.Filter = "Afbeelding (*.PNG)|*.PNG|Afbeelding (*.JPG)|*.JPG|Afbeelding (*.BMP)|*.BMP|Tekst file(*.TXT)|*.TXT"; 
        dialoog.Title = "Afbeelding opslaan als...";
        if (dialoog.ShowDialog() == DialogResult.OK)
        {
            Text = dialoog.FileName;
            schetscontrol.brug(Text);   //Hier wordt brug gebruikt als overbrugging naar de klasse SchetsControl
        }
    }

    public void open(object sender, EventArgs e)               //In deze methode kunnen afbeeldingen, en tekeningen die hierboven zijn gemaakt en opgeslagen geopend worden.
    {                                                           //Dit wordt gedaan met de OpenFileDialog.
        OpenFileDialog dialoog = new OpenFileDialog();
        dialoog.Filter = "Afbeeldingen (*.PNG;*.JPG;*.BMP;*.TXT)|*.PNG;*.JPG;*.BMP;*.TXT";  
        dialoog.Title = "Afbeelding openen...";
        if (dialoog.ShowDialog() == DialogResult.OK)
        {                          
            schetscontrol.brugb(dialoog.FileName);  //brugb wordt hier gebruikt als overbrugging naar SchetsControl.
        }
    }

    public void opslaanAlsLijst(object o, EventArgs ea)     //In deze methode wordt de bitmap op een speciale manier opgeslagen.
    {                                                       //Hier worden namelijk dingen die zijn getekend opgeslagen, met de letterlijke code van het "schetsen".
        SaveFileDialog dialoog = new SaveFileDialog();      
        dialoog.Filter = "Tekst file(*.TXT)|*.TXT";         //Dit wordt dan dus ook opgeslagen als tekstfile.
        dialoog.Title = "Afbeelding opslaan als...";
        if (dialoog.ShowDialog() == DialogResult.OK)
        {
            Text = dialoog.FileName;
            schetscontrol.brugc(Text);                      //Hier wordt opnieuw een overbrugging gebruikt om de methode te koppelen aan schetscontrol.
        }
    }

    public void OpenLijst(object o, EventArgs ea)          //In deze methode kunnen bestanden geopend worden die hierboven beschreven zijn. 
    {                                                      //De code van het schetsen wordt hier geopend door middel van een OpenFileDialog.
        OpenFileDialog dialoog = new OpenFileDialog();  
        dialoog.Filter = "Tekst file(*.TXT)|*.TXT";     //Deze bestanden worden geopend als tekstfile.
        dialoog.Title = "Open lijst";
        if (dialoog.ShowDialog() == DialogResult.OK)
        {
            schetscontrol.brugd(dialoog.FileName);         //Hier wordt opnieuw een overbrugging gebruikt om de methode te koppelen aan schetscontrol.
        }
    }

    private void afsluiten(object obj, EventArgs ea)
    {
        this.Close();
    }

    public SchetsWin()
    {
        ISchetsTool[] deTools = { new PenTool()         
                                , new LijnTool()
                                , new RechthoekTool()
                                , new VolRechthoekTool()
                                , new EllipsTool()
                                , new VolEllipsTool()
                                , new TekstTool()
                                , new VlakGumTool()
                                };
        String[] deKleuren = { "Black", "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };

        this.ClientSize = new Size(700, 500);
        huidigeTool = deTools[0];

        schetscontrol = new SchetsControl();
        schetscontrol.Location = new Point(64, 10);
        schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                    {   vast=true;  
                                        huidigeTool.MuisVast(schetscontrol, mea.Location); 
                                    };
        schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                    {   if (vast)
                                        huidigeTool.MuisDrag(schetscontrol, mea.Location); 
                                    };
        schetscontrol.MouseUp   += (object o, MouseEventArgs mea) =>
                                    {   if (vast)
                                        huidigeTool.MuisLos (schetscontrol, mea.Location);
                                        vast = false; 
                                    };
        schetscontrol.KeyPress +=  (object o, KeyPressEventArgs kpea) => 
                                    {   huidigeTool.Letter  (schetscontrol, kpea.KeyChar); 
                                    };
        this.Controls.Add(schetscontrol);

        menuStrip = new MenuStrip();
        menuStrip.Visible = false;
        this.Controls.Add(menuStrip);
        this.maakFileMenu();
        this.maakToolMenu(deTools);
        this.maakActieMenu(deKleuren);
        this.maakToolButtons(deTools);
        this.maakActieButtons(deKleuren);
        this.Resize += this.veranderAfmeting;
        this.veranderAfmeting(null, null);
    }

    private void maakFileMenu()
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("File");
        menu.MergeAction = MergeAction.MatchOnly;
        menu.DropDownItems.Add("Opslaan", null, this.opslaan);                   //Hier worden de verschillende dropdownitems toegevoegd aan het menu.
        menu.DropDownItems.Add("Opslaan als...", null, this.opslaanAls);        //Wij hebben hier het opslaan en openen aan toegevoegd.
        menu.DropDownItems.Add("Open...", null, this.open);                     
        menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
        menu.DropDownItems.Add("OpslaanAlsLijst", null, this.opslaanAlsLijst);
        menu.DropDownItems.Add("OpenLijst", null, this.OpenLijst);
        menuStrip.Items.Add(menu);
    }

    private void maakToolMenu(ICollection<ISchetsTool> tools)
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
        foreach (ISchetsTool tool in tools)
        {   ToolStripItem item = new ToolStripMenuItem();
            item.Tag = tool;
            item.Text = tool.ToString();
            item.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
            item.Click += this.klikToolMenu;
            menu.DropDownItems.Add(item);
        }
        menuStrip.Items.Add(menu);
    }

    private void maakActieMenu(String[] kleuren)
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
        menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
        menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
        ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
        foreach (string k in kleuren)
            submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
        menu.DropDownItems.Add(submenu);
        menuStrip.Items.Add(menu);
    }

    private void maakToolButtons(ICollection<ISchetsTool> tools)
    {
        int t = 0;
        foreach (ISchetsTool tool in tools)
        {
            RadioButton b = new RadioButton();
            b.Appearance = Appearance.Button;
            b.Size = new Size(45, 62);
            b.Location = new Point(10, 10 + t * 62);
            b.Tag = tool;
            b.Text = tool.ToString();
            b.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
            b.TextAlign = ContentAlignment.TopCenter;
            b.ImageAlign = ContentAlignment.BottomCenter;
            b.Click += this.klikToolButton;
            this.Controls.Add(b);
            if (t == 0) b.Select();
            t++;
        }
    }

    private void maakActieButtons(String[] kleuren)
    {   
        paneel = new Panel(); this.Controls.Add(paneel);
        paneel.Size = new Size(600, 24);
            
        Button clear = new Button(); paneel.Controls.Add(clear);
        clear.Text = "Clear";  
        clear.Location = new Point(  0, 0); 
        clear.Click += schetscontrol.Schoon;        
            
        Button rotate = new Button(); paneel.Controls.Add(rotate);
        rotate.Text = "Rotate"; 
        rotate.Location = new Point( 80, 0); 
        rotate.Click += schetscontrol.Roteer; 
           
        Label penkleur = new Label(); paneel.Controls.Add(penkleur);
        penkleur.Text = "Penkleur:"; 
        penkleur.Location = new Point(180, 3); 
        penkleur.AutoSize = true;               
            
        ComboBox cbb = new ComboBox(); paneel.Controls.Add(cbb);
        cbb.Location = new Point(240, 0); 
        cbb.DropDownStyle = ComboBoxStyle.DropDownList; 
        cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
        foreach (string k in kleuren)
            cbb.Items.Add(k);
        cbb.SelectedIndex = 0;
    }
}