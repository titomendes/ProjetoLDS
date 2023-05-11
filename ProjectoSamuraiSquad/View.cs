using System;
using System.Collections.Generic;
using ExemploEventosDelegados.Models;

namespace ExemploEventosDelegados.Views
{
    public class TelefoneView
    {
        private TelefoneModel model;

        private List<TelefoneMarca> listaTelefones;

        private int marcaIndex = -1;
        private int modeloIndex = -1;
        private TelefoneMarca marcaSelecionada;
        private TelefoneModelo modeloSelecionado;

        public delegate void PedirInfo (ref List<TelefoneMarca> lista);
        public event PedirInfo PedirListaTelefones;

        public delegate bool VerificaInputMarca(int n);
        public event VerificaInputMarca VerificaMarca;
        public delegate bool VerificaInputModelo(int indexMarca,int indexModelo);
        public event VerificaInputModelo VerificaModelo;

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
         
        public void MostrarMarcas(List<TelefoneMarca> marcas)
        {
            Console.WriteLine("Selecione uma marca:");

            for (int i = 0; i < marcas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {marcas[i].Nome}");
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
            MostrarModelos(marcaSelecionada.Nome, marcaSelecionada.Modelos); 
        }

        public void MostrarModelos(string nomeMarca, List<TelefoneModelo> modelos)
        {
            Console.WriteLine($"Modelos disponíveis da marca {nomeMarca}:");
            for (int i = 0; i < modelos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {modelos[i].Nome}");
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

            if (VerificaModelo(marcaIndex, modeloIndex)) modeloSelecionado = marcaSelecionada.Modelos[modeloIndex];

            MostrarTiposDeReparacao(modeloSelecionado);
        }

        public void MostrarTiposDeReparacao(TelefoneModelo modeloSelecionado)
        {
            Console.WriteLine($"Selecione um tipo de reparação para o modelo {modeloSelecionado.Nome}:");
            foreach (var tipoReparacao in modeloSelecionado.PrecosDeReparacao)
            {
                Console.WriteLine($"{tipoReparacao.Key}. Preço: {tipoReparacao.Value}");
            }

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
