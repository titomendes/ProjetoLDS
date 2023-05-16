using System.Collections.Generic;
using ExemploEventosDelegados.Views;
using ExemploEventosDelegados.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace ExemploEventosDelegados.Models
{
    public interface IMarca   //cada marca tem de ter um nome e uma lista de modelos
    {
        string nome { get; set; }
        List<IModelo> Modelos { get; set; }
    }

    public interface IMarcaPdf
    {
        string nome { get; set; }
    }

    public class TelefoneMarca : IMarca
    {
        public string nome { get; set; }
        public List<IModelo> Modelos { get; set; }

        public TelefoneMarca(string nome, List<IModelo> modelos)
        {
            this.nome = nome;
            Modelos = modelos;
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

    public interface IModeloPdf  //interface do modelo para enviar parao pdf, só é preciso o nome
    {
        string nome { get; set; }
    }

    public interface IModelo   //cada modelo tem de ter nome e uma lista de reparações
    {                           //com a interface pode se criar outros tipos de modelo, desde que implementem este contrato
                                //coloquei a função de adiconarTipoDeReparação no contrato porque se não para a usar tinha de fazer um cast para cada modelo para poder usar a função
        string nome { get; set; }
        Dictionary<string, decimal> PrecosDeReparacao { get; set; }

        void AdicionarTipoDeReparacao(string tipoReparacao, decimal preco);

        public Reparacao selecionaReparacao(int n, Dictionary<string, decimal> listaReparacoes);

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

        private TelefoneView view;
        public List<IMarca>? marcasDeTelefone;  

        public void Iniciar(TelefoneView view)
        {
            this.view = view;
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
    public interface ITexto
    {

        bool verificaTexto(string texto);
        bool verificaTextoEmail(string texto);
        bool VerificaContato(string contato);
    }




    public class Texto : ITexto  //implementa as duas interfaces caso se queira validar diferentes tipos de texto
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

    public interface ICliente   //interface do cliente, tem de ter nome email e contacto
    {
        string nome { get; set; }
        string email { get; set; }
        int contacto { get; set; }
    }
    public class Cliente : ICliente
    {
        public string nome { get; set; }
        public string email { get; set; }
        public int contacto { get; set; }

        public Cliente(string nome, string email, int contacto)
        {
            this.nome = nome;
            this.email = email;
            this.contacto = contacto;
        }
    }


}



