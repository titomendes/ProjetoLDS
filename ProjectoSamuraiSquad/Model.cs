using System.Collections.Generic;
using ExemploEventosDelegados.Views;
using ExemploEventosDelegados.Exceptions;

namespace ExemploEventosDelegados.Models
{
    public class TelefoneMarca
    {
        public string Nome { get; set; }
        public List<TelefoneModelo> Modelos { get; set; }

        public TelefoneMarca(string nome, List<TelefoneModelo> modelos)
        {
            Nome = nome;
            Modelos = modelos;
        }


        public static List<TelefoneMarca> ObterListaDeMarcasDeTelefone()
        {
            var modelosApple = new List<TelefoneModelo>
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

            var modelosSamsung = new List<TelefoneModelo>
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

            var marcas = new List<TelefoneMarca>
            {
                new TelefoneMarca("Apple", modelosApple),
                new TelefoneMarca("Samsung", modelosSamsung)
            };

            return marcas;
        }


    }

    public class TelefoneModelo
    {
        public string Nome { get; set; }
        public Dictionary<string, decimal> PrecosDeReparacao { get; set; }

        public TelefoneModelo(string nome)
        {
            Nome = nome;
            PrecosDeReparacao = new Dictionary<string, decimal>();
        }

        public void AdicionarTipoDeReparacao(string tipoReparacao, decimal preco)
        {
            PrecosDeReparacao[tipoReparacao] = preco;
        }
    }

    public class TelefoneModel {

        private TelefoneView view;
        public List<TelefoneMarca>? marcasDeTelefone;

        public void Iniciar (TelefoneView view)
        {
            this.view = view;
            marcasDeTelefone = TelefoneMarca.ObterListaDeMarcasDeTelefone();
            
        }

        public void EnviarLista(ref List<TelefoneMarca> lista)
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
}

