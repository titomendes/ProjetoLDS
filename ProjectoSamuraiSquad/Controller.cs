using System;
using System.Collections.Generic;

namespace ProjectoSamuraiSquad
{
	public class Controller
	{
		private Model? model;
		private View? view;

		public delegate void AtivacaoInterface();
		public event AtivacaoInterface AtivarInterface;

		//Eventos e delegados para comunicação

		public void IniciarPrograma()
		{
			model = new Model(view);
			view = new View(model);

			AtivarInterface += view.AtivarInterface;

			view.enviarDadosReparacao += model.UpdateInfoReparacao;

			view.solicitarOrcamento += model.EnviarDadosOrcamento;

			model.enviarDadosOrcamento += view.ApresentarOrcamento;

			view.pedirPdf += model.CriarPdf;

			view.ordemEncerrar += Encerrar;
            model.pdfGerado += Encerrar;
        }

		public void Encerrar()
		{
			view.MostrarMSGFinal();
			Environment.Exit(0);
		}
	}
}

