using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using ExemploEventosDelegados.Models;

using MimeKit;
using MailKit.Net.Smtp;
using ExemploEventosDelegados.Exceptions;
using ExemploEventosDelegados.Interfaces;

namespace ExemploEventosDelegados.Views
{
    public class TelefoneView
    {
        private List<IMarca> listaTelefones;
        private Reparacao reparacao;   //reparação
        private Cliente cliente;   //cliente

        private int marcaIndex = -1;
        private int modeloIndex = -1;

        private IMarca marcaSelecionada;
        private IMarcaPdf marcaPdf;

        private IModelo modeloSelecionado;
        private IModeloPdf modeloPdf;    //modelo pdf a enviar para o pdf

        private OrcamentoPdf orcamentoPdf; //objeto para receber os dados do equipamento
        private Equipamento equipamento;  //vai receber a marca modelo reapração e cliente

        public delegate void PedirInfo(ref List<IMarca> lista);
        public event PedirInfo PedirListaTelefones;

        public delegate bool VerificaInputMarca(int n);
        public event VerificaInputMarca VerificaMarca;
        public delegate bool VerificaInputModelo(int indexMarca, int indexModelo);
        public event VerificaInputModelo VerificaModelo;

        public delegate Reparacao SelecionarReparacao(int n, Dictionary<string, decimal> listaReparacoes);     //delegado e evento para selecionar a reparacao
        public event SelecionarReparacao ReparacaoSelecionada;

        public delegate bool GeraRelatorio(Equipamento equipamento,string nomeArquivo);   //delegado e evento para quando o equipamento estiver completo para o orçamento
        public event GeraRelatorio EquipamentoGerado;

        public delegate void UtilizadorSai();
        public event UtilizadorSai UtilizadorQuerSair;

        public delegate void EnviarOrcamento(string emailCliente, string nomeArquivo);
        public event EnviarOrcamento OrcamentoPronto;

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
                marcaPdf = new TelefoneMarca();
                marcaPdf  = (IMarcaPdf) marcaSelecionada; 
              
                
            }         

            MostrarTiposDeReparacao(modeloSelecionado);
        }

        public void MostrarTiposDeReparacao(IModelo modeloSelecionado)
        {
      

            int i = 1;
            int selecao;  // selecionar a reparacao
            Console.WriteLine($"Selecione um tipo de reparação para o modelo {modeloSelecionado.nome}:");
            foreach (var tipoReparacao in modeloSelecionado.PrecosDeReparacao)
            {
                Console.WriteLine($"{i}. {tipoReparacao.Key}. Preço: {tipoReparacao.Value}");
                i++;
            }
           ConsoleKeyInfo input = Console.ReadKey();


            if (!(int.TryParse(input.KeyChar.ToString(), out selecao)) || selecao < 1 || selecao > i - 1)
                throw new ExceptionInputInvalido();  //so verifica o input

            reparacao = new Reparacao();
            reparacao = ReparacaoSelecionada(selecao - 1, modeloSelecionado.PrecosDeReparacao);  //reparação criada
            modeloPdf = new TelefoneModelo();
            modeloPdf =(IModeloPdf) modeloSelecionado;    //coinverte o modelo para modelo pdf pois só precisamos do nome
            Console.Clear();

            insereDadosCliente();

       

        
        }


         public void insereDadosCliente()
         {

             Texto texto = new Texto();     
             Console.WriteLine("Insira o seu nome");
             string nome = Console.ReadLine();
             Console.WriteLine("Insira o seu email");
             string email = Console.ReadLine();
             Console.WriteLine("Insira o seu contacto");
             string contacto = Console.ReadLine();
             while (!(texto.verificaTexto(nome)) || !(texto.verificaTextoEmail(email)) || !(texto.VerificaContato(contacto)) || string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contacto)) 
             {
                 Console.WriteLine("Dados invalidos, Insira novamente");
                 insereDadosCliente();
             }
            


             cliente = new Cliente(nome, email, contacto);   //cliente criado   



             equipamento = new Equipamento(marcaPdf, modeloPdf, reparacao, cliente);
             Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string nomeArquivo = "Relatorio.pdf";
            if (EquipamentoGerado(equipamento,nomeArquivo) == true)
            {        
                EnviarEmailComRelatorio(email, nomeArquivo);
            }

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

            UtilizadorQuerSair();
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
