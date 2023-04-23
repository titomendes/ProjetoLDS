using System;
using System.Collections.Generic;

namespace ProjectoSamuraiSquad
{
	public class View
	{
		private Model model;
		private bool clienteQuerPdf = false;

		private List<Model.Telemovel> telemoveis;
        private List<Model.Reparacao> reparacoes;

        public delegate void DadosReparacao(ref string dados);
		public event DadosReparacao enviarDadosReparacao;
		public event DadosReparacao enviarDadosUtilizador;
		public delegate void PedirOrcamento();
		public event PedirOrcamento solicitarOrcamento;
		public delegate void GerarPdf();
		public event GerarPdf pedirPdf;
		public delegate void Encerrar();
		public event Encerrar ordemEncerrar;
		public delegate void SolicitarListaTelemovel();
		public event SolicitarListaTelemovel solicitarDadosTelemovel;
        public delegate void SolicitarListaReparacao();
        public event SolicitarListaReparacao solicitarDadosReparacao;


        public View(Model m) {
			model = m;
		}

		public void AtivarInterface()
		{

			Console.WriteLine("Bem vindo. ");

			string dadosReparacao = "", dadosUtilizador = "";

			solicitarDadosTelemovel();
			solicitarDadosReparacao();

            model.enviarListaTelemovel += IniciarListaTelemovel;
			model.enviarListaReparacao += IniciarListaReparacao;

			Console.WriteLine("Escolha uma marca e modelo.");

			foreach (var telemovel in telemoveis)
			{
				Console.WriteLine("Marca: {0} Modelo: {1}", telemovel.marca, telemovel.modelo);
			}


            enviarDadosReparacao(ref dadosReparacao);
			enviarDadosUtilizador(ref dadosUtilizador);

			model.dadosForamAtualizados += SolicitarOrcamento;
		}

		public void IniciarListaTelemovel (ref List<Model.Telemovel> t)
		{
			telemoveis = t;
		}

        public void IniciarListaReparacao(ref List<Model.Reparacao> r)
        {
            reparacoes = r;
        }

        public void SolicitarOrcamento()
		{
			solicitarOrcamento();
		}

        public void ApresentarOrcamento(ref int valor)
        {
			/* Apresentar o valor ao cliente e perguntar se quer gerar pdf
			 se não quiser mandar encerrar o programa.*/

			if (clienteQuerPdf) {
				pedirPdf();
			} else
			{
				ordemEncerrar();
			}
        }

		public void MostrarMSGFinal()
		{
			Console.WriteLine("Adeus!");
		}
    }
}

