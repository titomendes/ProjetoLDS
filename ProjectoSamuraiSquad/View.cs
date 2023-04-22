using System;
using System.Collections.Generic;

namespace ProjectoSamuraiSquad
{
	public class View
	{
		private Model model;
		private bool clienteQuerPdf = false;

		public delegate void DadosReparacao(ref string dados);
		public event DadosReparacao enviarDadosReparacao;
		public event DadosReparacao enviarDadosUtilizador;
		public delegate void PedirOrcamento();
		public event PedirOrcamento solicitarOrcamento;
		public delegate void GerarPdf();
		public event GerarPdf pedirPdf;
		public delegate void Encerrar();
		public event Encerrar ordemEncerrar;


		public View(Model m) {
			model = m;
		}

		public void AtivarInterface()
		{

			Console.WriteLine("Bem vindo. ");

			string dadosReparação = "", dadosUtilizador = "";

			/* Pedir marca, modelo, reparação ao cliente
			 * Pedir dados pessoais ao cliente
			 * Enviar informação ao Controller
			 */

			enviarDadosReparacao(ref dadosReparação);
			enviarDadosUtilizador(ref dadosUtilizador);

			model.dadosForamAtualizados += SolicitarOrcamento;
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

