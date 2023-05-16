using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using ExemploEventosDelegados.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using MimeKit;
using MailKit.Net.Smtp;
using ExemploEventosDelegados.Exceptions;

namespace ExemploEventosDelegados.Views
{
    public class TelefoneView
    {
        private TelefoneModel model;

        private List<IMarca> listaTelefones;
        private Reparacao reparacao;   //reparação
        private Cliente cliente;   //cliente

        private int marcaIndex = -1;
        private int modeloIndex = -1;

        private IMarca marcaSelecionada;
        private IMarcaPdf marcaPdf;

        private IModelo modeloSelecionado;
        private IModeloPdf modeloPdf;    //modelo pdf a enviar para o pdf

        public delegate void PedirInfo(ref List<IMarca> lista);
        public event PedirInfo PedirListaTelefones;

        public delegate bool VerificaInputMarca(int n);
        public event VerificaInputMarca VerificaMarca;
        public delegate bool VerificaInputModelo(int indexMarca, int indexModelo);
        public event VerificaInputModelo VerificaModelo;

        public delegate Reparacao SelecionarReparacao(int n, Dictionary<string, decimal> listaReparacoes);     //delegado e evento para selecionar a reparacao
        public event SelecionarReparacao ReparacaoSelecionada;


        public delegate void UtilizadorSai();
        public event UtilizadorSai UtilizadorQuerSair;

        public void Iniciar(TelefoneModel model)
        {
            this.model = model;

        }

        public void InicarInterface()
        {
            PedirListaTelefones(ref listaTelefones);

            if (listaTelefones != null) MostrarMarcas(listaTelefones);

        }

        public void MostrarMarcas(List<IMarca> marcas)
        {
            Console.WriteLine("Selecione uma marca:");

            for (int i = 0; i < marcas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {marcas[i].nome}");
            }

            // Aguarda a escolha do usuário
            ConsoleKeyInfo input = Console.ReadKey();

            // Converte o input em um índice de marca
            if (int.TryParse(input.KeyChar.ToString(), out marcaIndex))
            {
                marcaIndex--;
            }
            else
            {
                marcaIndex = -1;
            }

            if (VerificaMarca(marcaIndex)) marcaSelecionada = marcas[marcaIndex];

            

            Console.Clear();
            MostrarModelos(marcaSelecionada.nome, marcaSelecionada.Modelos);
        }

        public void MostrarModelos(string nomeMarca, List<IModelo> modelos)
        {
            Console.WriteLine($"Modelos disponíveis da marca {nomeMarca}:");
            for (int i = 0; i < modelos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {modelos[i].nome}");
            }

            // Aguarda a escolha do usuário
            ConsoleKeyInfo input = Console.ReadKey();

            // Converte o input em um índice de modelo
            if (int.TryParse(input.KeyChar.ToString(), out modeloIndex))
            {
                modeloIndex--;
            }
            else
            {
                modeloIndex = -1;
            }

            Console.Clear();

            if (VerificaModelo(marcaIndex, modeloIndex))
            {
                modeloSelecionado = marcaSelecionada.Modelos[modeloIndex];
                marcaPdf = (IMarcaPdf) marcaSelecionada;
            }
           


            MostrarTiposDeReparacao(modeloSelecionado);
        }

        public void MostrarTiposDeReparacao(IModelo modeloSelecionado)
        {
      

            int i = 0;
            int selecao;  // selecionar a reparacao
            Console.WriteLine($"Selecione um tipo de reparação para o modelo {modeloSelecionado.nome}:");
            foreach (var tipoReparacao in modeloSelecionado.PrecosDeReparacao)
            {
                Console.WriteLine($"{i}{tipoReparacao.Key}. Preço: {tipoReparacao.Value}");
                i++;
            }
            ConsoleKeyInfo input = Console.ReadKey();


            if (!(int.TryParse(input.KeyChar.ToString(), out selecao)) || selecao < 0 || selecao > i-1)
                throw new ExceptionInputInvalido();  //so verifica o input

            reparacao = ReparacaoSelecionada(selecao, modeloSelecionado.PrecosDeReparacao);  //reparação criada
            modeloPdf =(IModeloPdf) modeloSelecionado;    //coinverte o modelo para modelo pdf pois só precisamos do nome
            Console.Clear();

        //    insereDadosCliente();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            GerarRelatorioPDF();  //tenho de enviar a marca,modelo,reparação e cliente
            


        }

        public void insereDadosCliente()
        {
            int contacto;
            ITexto texto = new Texto();     //iniciar um objeto texto que implementa a interface que verifica o nome
            Console.WriteLine("Insira o seu nome");
            string nome = Console.ReadLine();
            Console.WriteLine("Insira o seu email");
            string email = Console.ReadLine();
            Console.WriteLine("Insira o seu contacto");
            string contactoString = Console.ReadLine();
            while (!(texto.verificaTexto(nome) || texto.verificaTextoEmail(email) || texto.VerificaContato(contactoString)) || string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contactoString)) ;
            {
                Console.WriteLine("Dados invalidos, Insira novamente");
                insereDadosCliente();
            }
            if (!(int.TryParse(contactoString, out contacto)))
                throw new ExceptionInputInvalido();


            cliente = new Cliente(nome, email, contacto);   //cliente criado   

        }


        public void GerarRelatorioPDF()
            {
            
            
            var nomeArquivo = "Relatorio.pdf";
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12);

                gfx.DrawString($"Modelo selecionado: {modeloPdf.nome}", font, XBrushes.Black, new XRect(10, 10, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString("Tipo de reparação selecionado:", font, XBrushes.Black, new XRect(10, 40, page.Width, 20), XStringFormats.TopLeft);
                gfx.DrawString("Preço:", font, XBrushes.Black, new XRect(150, 40, page.Width, 20), XStringFormats.TopLeft);

                int yOffset = 60;
                
                     //coloca a reparação no pdf
                    gfx.DrawString(reparacao.descricao, font, XBrushes.Black, new XRect(10, yOffset, page.Width, 20), XStringFormats.TopLeft);
                    gfx.DrawString(reparacao.preco.ToString("C"), font, XBrushes.Black, new XRect(150, yOffset, page.Width, 20), XStringFormats.TopLeft);
                    yOffset += 20;
                
             

                document.Save(nomeArquivo);
         
}






    Console.WriteLine($"Relatório gerado em {nomeArquivo}.");
            
            EnviarEmailComRelatorio("titodalt@gmail.com", nomeArquivo);
        }


        public void EnviarEmailComRelatorio(string emailCliente, string nomeArquivo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Remetente", "csharpsamuraisquad@gmail.com"));
            message.To.Add(new MailboxAddress("Destinatário", emailCliente));
            message.Subject = "Relatório do telefone";

            var builder = new BodyBuilder();
            builder.TextBody = "Segue em anexo o relatório do telefone.";

            // Anexa o relatório em PDF ao email
            builder.Attachments.Add(nomeArquivo);

            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("csharpsamuraisquad@gmail.com", "unmmljomqozbfuxs");
                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine($"Relatório enviado para o email {emailCliente}.");
        }


        public void EcraErro()
        {
            Console.Clear();
            Console.WriteLine("O input introduzido é inválido.\n" +
                "Que pretende fazer?\n" +
                "1 - Introduzir novamente\n" +
                "2 - Sair\n");

            ConsoleKeyInfo input = Console.ReadKey();
            int val;
            int.TryParse(input.KeyChar.ToString(), out val);

            if (val == 2) UtilizadorQuerSair();
            else Console.Clear();
        }
    }
}
