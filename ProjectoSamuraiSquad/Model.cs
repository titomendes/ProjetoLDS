using System.Collections.Generic;
using ExemploEventosDelegados.Views;
using ExemploEventosDelegados.Exceptions;
using System;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ExemploEventosDelegados.Interfaces;

namespace ExemploEventosDelegados.Models
{
   

    public class TelefoneMarca : IMarca, IMarcaPdf
    {
        public string nome { get; set; }
        public List<IModelo> Modelos { get; set; }

        public TelefoneMarca(string nome, List<IModelo> modelos)
        {
            this.nome = nome;
            Modelos = modelos;
        }
        public TelefoneMarca()
        {
            
        }

        public static List<IMarca> ObterListaDeMarcasDeTelefone()  //agora as listas são do tipo IModelo
        {
            var modelosApple = new List<IModelo>
            {
                new TelefoneModelo("iPhone 12"),
                new TelefoneModelo("iPhone SE"),
                new TelefoneModelo("iPhone 11")
            };

            modelosApple[0].AdicionarTipoDeReparacao("Ecra partido", 100.00m);
            modelosApple[0].AdicionarTipoDeReparacao("Bateria", 80.00m);
            modelosApple[1].AdicionarTipoDeReparacao("Ecra partido", 90.00m);
            modelosApple[1].AdicionarTipoDeReparacao("Alto-falante com problema", 70.00m);
            modelosApple[2].AdicionarTipoDeReparacao("Ecra partido", 100.00m);
            modelosApple[2].AdicionarTipoDeReparacao("Bateria", 80.00m);
            modelosApple[2].AdicionarTipoDeReparacao("Botão partido", 50.00m);

            var modelosSamsung = new List<IModelo>
            {
                new TelefoneModelo("Note 20"),
                new TelefoneModelo("A51"),
                new TelefoneModelo("S23")
            };

            modelosSamsung[0].AdicionarTipoDeReparacao("Ecra partido", 100.00m);
            modelosSamsung[0].AdicionarTipoDeReparacao("Bateria", 80.00m);
            modelosSamsung[1].AdicionarTipoDeReparacao("Ecra partido", 90.00m);
            modelosSamsung[1].AdicionarTipoDeReparacao("Alto-falante com problema", 70.00m);
            modelosSamsung[2].AdicionarTipoDeReparacao("Ecra partido", 100.00m);
            modelosSamsung[2].AdicionarTipoDeReparacao("Bateria", 80.00m);
            modelosSamsung[2].AdicionarTipoDeReparacao("Botão partido", 50.00m);

            var marcas = new List<IMarca>
            {
                new TelefoneMarca("Apple", modelosApple),
                new TelefoneMarca("Samsung", modelosSamsung)
            };

            return marcas;
        }


    }

   
    public class TelefoneModelo : IModelo, IModeloPdf   //implementa a interface
    {                                          //vou verificar a reparação aqui, já que a lista está aqui
        public string nome { get; set; }
        public Dictionary<string, decimal> PrecosDeReparacao { get; set; } //valor é o preco, key é a string
        public TelefoneModelo(string nome)
        {
            this.nome = nome;
            PrecosDeReparacao = new Dictionary<string, decimal>();
        }

        public TelefoneModelo() //criei so para aceder as funções
        {

        }
        public Reparacao selecionaReparacao(int n, Dictionary<string, decimal> listaReparacoes)  //devolve a reparacao selecionada
        {
            Reparacao reparacao = new Reparacao();
            int i = 0;
            foreach (var tipoReparacao in listaReparacoes)
            {
                if (i == n)
                {
                    reparacao.descricao = tipoReparacao.Key;
                    reparacao.preco = tipoReparacao.Value;
                    break;
                }

                i++;
            }
            return reparacao;
        }

        public void AdicionarTipoDeReparacao(string tipoReparacao, decimal preco)
        {
            PrecosDeReparacao[tipoReparacao] = preco;
        }

    }

    public class TelefoneModel
    {

        public List<IMarca>? marcasDeTelefone;  

        public void Iniciar()
        {
            marcasDeTelefone = TelefoneMarca.ObterListaDeMarcasDeTelefone();
        }

        public void EnviarLista(ref List<IMarca> lista)   //enviar lista de interfaces de marca
        {
            if (marcasDeTelefone != null) lista = (marcasDeTelefone);
        }

        public bool VerificaMarca(int index)
        {
            try
            {
                if (marcasDeTelefone[index] != null) ;
            }
            catch
            {
                throw new ExceptionInputInvalido();
            }

            return true;
        }

        public bool VerificaModelo(int indexMarca, int indexModelo)
        {
            try
            {
                if (marcasDeTelefone[indexMarca].Modelos[indexModelo] != null) ;
            }
            catch
            {
                throw new ExceptionInputInvalido();
            }

            return true;
        }

    }
 

    public class Texto
    {
        public string texto { get; set; }

        public Texto()
        {

        }
        public bool verificaTexto(string texto)
        {
            string padrao = @"^[a-zA-Z]+$";   //verifica se a string contem apenas letras
            return Regex.IsMatch(texto, padrao);

        }
        public bool verificaTextoEmail(string texto)      //verifica o email
        {
            string padrao = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Verificar correspondência do padrão com o email
            return Regex.IsMatch(texto, padrao);
        }

        public bool VerificaContato(string contato)
        {
            // Padrão de expressão regular para verificar se o contato contém apenas números
            string padrao = @"^\d{9}$"; // Verifica se o contato possui exatamente 9 dígitos

            // Verificar correspondência do padrão com o contato
            return Regex.IsMatch(contato, padrao);
        }
    }

    public class Reparacao
    {
        public string descricao { get; set; }
        public decimal preco { get; set; }
    }

    
    public class Cliente 
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string contacto { get; set; }

        public Cliente(string nome, string email, string contacto)
        {
            this.nome = nome;
            this.email = email;
            this.contacto = contacto;
        }
    }

    public class Equipamento
    {
        public IMarcaPdf marca { get; set; }
        public IModeloPdf modelo { get; set; }
        public Cliente cliente { get; set; }
        public Reparacao reparacao { get; set; }

        public Equipamento(IMarcaPdf marca, IModeloPdf modelo, Reparacao reparacao, Cliente cliente)
        {
            this.marca = marca;
            this.modelo = modelo;
            this.cliente = cliente;
            this.reparacao = reparacao;
        }
    }

    public class OrcamentoPdf  //classe que gera o orçamento em pdf
    {

        //evento e delegado para a avisar que o orçamento esta pronto e chamar a função de enviar o email
       
      
        public bool GerarRelatorioPDF(Equipamento equipamento,string nomeArquivo)
        {

            nomeArquivo = "Relatorio.pdf";
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12);

                int yOffset = 60;

                // Coloca a reparação no PD
                gfx.DrawString("Marca:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.marca.nome, font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;

                gfx.DrawString("Modelo:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.modelo.nome, font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;


                gfx.DrawString("Tipo de reparação selecionado:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.reparacao.descricao, font, XBrushes.Black, new XRect(180, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;

                gfx.DrawString("Preço:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.reparacao.preco.ToString("C"), font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;

                gfx.DrawString("Cliente:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.cliente.nome, font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;

                gfx.DrawString("Contacto:", font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString(equipamento.cliente.contacto, font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                yOffset += 20;



                document.Save(nomeArquivo);

            }

            Console.WriteLine($"Relatório gerado em {nomeArquivo}.");

          

            return true;
        }

       

        public OrcamentoPdf()
        {

        }
    }


}



