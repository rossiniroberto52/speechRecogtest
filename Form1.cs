using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace speechRecogtest
{
    public partial class Form1 : Form
    {
        static CultureInfo ci = new CultureInfo("pt-br");
        static SpeechRecognitionEngine reconecedor;
        SpeechSynthesizer resposta = new SpeechSynthesizer();

        public string[] listaPalavaras = { "oi", "quem é você", "como você está" };

        public Form1()
        {
            InitializeComponent();

            init();
        }

        public void Gramatica()
        {
            try
            {
                reconecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("erro ao integrar lang. escolhida " + ex.Message);
            }

            var gramatica = new Choices();
            gramatica.Add(listaPalavaras);

            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            try
            {
                var g = new Grammar(gb);

                try
                {
                    reconecedor.RequestRecognizerUpdate();
                    reconecedor.LoadGrammarAsync(g);
                    reconecedor.SpeechRecognized += Sre_Reconhecimento;
                    reconecedor.SetInputToDefaultAudioDevice();
                    resposta.SetOutputToDefaultAudioDevice();
                    reconecedor.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("erro do reconhecimento de voz: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("erro ao criar a gramatica" + ex.Message);
            }
        }

        public void init() 
        {
            resposta.Volume = 100;
            resposta.Rate = 3;

            Gramatica();
        }

        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;

            if (frase.Equals("oi"))
            {
                resposta.SpeakAsync("olá, como você está");
            }
            else if(frase.Equals("quem é você"))
            {
                resposta.SpeakAsync("eu sou uma assistente virtual ainda em desenvolvimento");
            }
            else if (frase.Equals("como você está"))
            {
                resposta.SpeakAsync("bem, e você");
            }
        }


    }
}
