using System;
using System.Collections.Generic;

namespace ProjectoSamuraiSquad
{
	public class Model
	{
		private View view;

		private bool dadosUtilizador = false;
        private bool dadosReparacao = false;

        public delegate void dadosAtualizados();
		public event dadosAtualizados dadosForamAtualizados;
		public delegate void dadosOrcamento(ref int valor);
		public event dadosOrcamento enviarDadosOrcamento;
		public delegate void PdfGerado();
		public event PdfGerado pdfGerado;

        public Model(View v) {
			view = v;
		}
		
		public void UpdateInfoReparacao(ref string dados)
		{
			/* Fazer update aos dados da reparação e actualizar dados Reparacao
			para True e se dados Utilizar também já for True enviar evento a
			avisar */

			dadosReparacao = true;

			if (dadosUtilizador)
			{
				dadosForamAtualizados();
			}

		}

        public void updateInfoUtilizador(ref string dados)
        {
			/* Fazer update aos dados da reparação e actualizar dados Reparacao
			para True e se dados Utilizar também já for True enviar evento a
			avisar */

			dadosUtilizador = true;

            if (dadosReparacao)
            {
                dadosForamAtualizados();
            }

        }

		public void EnviarDadosOrcamento()
		{
			/*Processar os dados de utilizador e de reparação e enviar
			 o valor do orcamento*/

			int valorOrcamento = 0;

			enviarDadosOrcamento(ref valorOrcamento);
		}

		public void CriarPdf()
		{
			/* Código de implementação da API Pdf Sharp para criar o Pdf com as
			 * informações necessárias. */

			pdfGerado();
		}
    }
}

