using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace juego_vida
{
    public partial class Form1 : Form
    {
        private int longitud = 25;
        private int largoPixel = 20;
        int[,] celulas;
        private int vueltasVida = 0;
        List<string> lista = new List<string>();
        
        

        public Form1()
        {
            InitializeComponent();
            celulas = new int[longitud, longitud];
            var fileContent = string.Empty;
            var filePath = string.Empty;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void PintarMatriz()
        {   
            Bitmap bmp = new Bitmap(picMatriz.Width, picMatriz.Height);
            for (int x = 0; x < longitud; x++)
            {
                for (int y = 0; y < longitud; y++)
                {
                    if (celulas[x, y] == 0)
                        pintarPixel(bmp, x, y, Color.White);
                    else
                        pintarPixel(bmp, x, y, Color.Black);
                }

            }
            
            picMatriz.Image = bmp;
        }

        private void pintarPixel(Bitmap bmp, int x, int y, Color color)
        {
            for (int i = 0; i < largoPixel; i++)
                for (int j = 0; j < largoPixel; j++)
                    bmp.SetPixel(i + ((x * largoPixel)), j + ((y * largoPixel)), color);

        }

        private void JuegoDeLaVida()
        {
            int[,] celulasTemporales = new int[longitud, longitud];
            for (int x = 0; x < longitud; x++)
            {
                for (int y = 0; y < longitud; y++)
                {
                    if (celulas[x, y] == 0)
                        celulasTemporales[x, y] = Reglas(x, y, false);
                    else
                        celulasTemporales[x, y] = Reglas(x, y, true);
                }
            }
            celulas = celulasTemporales;
        }
        private int Reglas(int x, int y, bool viva)
        {
            int VecinasVivas = 0;

            //vecina 1
            if (x > 0 && y > 0)
                if (celulas[x - 1, y - 1] == 1)
                    VecinasVivas++;

            //vecina 2
            if (y > 0)
                if (celulas[x, y - 1] == 1)
                    VecinasVivas++;

            //vecina 3
            if (x < longitud - 1 && y > 0)
                if (celulas[x + 1, y - 1] == 1)
                    VecinasVivas++;

            //vecina 4
            if (x > 0)
                if (celulas[x - 1, y] == 1)
                    VecinasVivas++;

            //vecina 5
            if (x < longitud - 1)
                if (celulas[x + 1, y] == 1)
                    VecinasVivas++;

            //vecina 6
            if (x > 0 && y < longitud - 1)
                if (celulas[x - 1, y + 1] == 1)
                    VecinasVivas++;

            //vecina 7
            if (y < longitud - 1)
                if (celulas[x, y + 1] == 1)
                    VecinasVivas++;


            //vecina 8
            if (x < longitud - 1 && y < longitud - 1)
                if (celulas[x + 1, y + 1] == 1)
                    VecinasVivas++;

            if (viva)
                return (VecinasVivas == 2 || VecinasVivas == 3) ? 1 : 0;
            else
                return VecinasVivas == 3 ? 1 : 0;
        }

        private void tiempo_Tick(object sender, EventArgs e)
        {
            JuegoDeLaVida();
            PintarMatriz();
            vueltasVida++;
            lblNumeroCiclos.Text = "" + vueltasVida;

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            LeerArchivo();
            




            
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            PintarMatriz();
            tiempo.Enabled = true;
            if (btnIniciar.Text == "continuar")
            { btnIniciar.Text = "Iniciar";
            }
            
        }

        private void btnDetener_Click(object sender, EventArgs e)
        {
            tiempo.Enabled = false;
            btnIniciar.Text = "continuar";
        }

        private void CargarMatriz(int valor)
        {
            for (int f = 0; f < celulas.Length; f++)
            {
                for (int c = 0; c < celulas.Length; c++)
                {

                    celulas[f, c] = valor;
                }
            }
        }
        //boton de prueba
        private void button1_Click(object sender, EventArgs e)
        {
            
            for (int f = 0; f < longitud; f++)
            {
                for (int c = 0; c < longitud; c++)
                {
                   Console.Write(celulas[f, c] + " ");
                }
                Console.WriteLine();
            }
            
            ////reinicio
            //for (int i = 0; i < longitud; i++)
            //    for (int j = 0; j < longitud; j++)
            //       celulas[i, j] = 0;
            //vueltasVida = 0;

            ////llenado
            //Random r = new Random();

            //for (int i = 0; i < longitud; i++)
            //    for (int j = 0; j < longitud; j++)
            //        celulas[i, j] = r.Next(0, 2);
            //PintarMatriz();
            

        }
        public void LeerArchivo()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;



            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        String val = "";
                        int f = 0;
                        int c = 0;

                        foreach (char item in fileContent)
                        {
                            val = Convert.ToString(item);
                            if (val == "1" || val == "0")
                            {
                                if (c == 25) {
                                    break;
                                }
                                if(f < 25)
                                {
                                    celulas[f, c] = Convert.ToInt32(val);
                                    f++;
                                    if (f == 25)
                                    {
                                        f = 0;
                                        c++;

                                    }
                                }
                                
                                
                                
                                
                                
                            }
                        }


                    }

                }
            }
            PintarMatriz();
        }

        private void picMatriz_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);
             
            for (int y = 0; y<= longitud; ++y)
            {
                g.DrawLine(p, 0, y * largoPixel, longitud * largoPixel, y * largoPixel);

            }
            for (int x = 0; x <= longitud; x++)
            {
                g.DrawLine(p, x * largoPixel, 0, x * largoPixel, longitud * largoPixel);
            }
            ////hori
            //for (int i = 500; i >= 0; i = i - longitud)
            //{
            //    g.DrawLine(p, 0, i, 500, i);

            //}
            ////verti
            //for (int j = 500; j >= 0 ; j= j-longitud)
            //{
            //    g.DrawLine(p, j, 0, j, 500);
            //}
        }

        //private void btnReset_Click(object sender, EventArgs e)
        //{
        //    lblNumeroCiclos.Text = "0";
        //}
    }
    
}
