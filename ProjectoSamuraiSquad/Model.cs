using System;
using System.Collections.Generic;

namespace ProjectoSamuraiSquad
{
	public class Model
	{
		private View view;

		private bool dadosUtilizador = false;
        private bool dadosReparacao = false;

        public List<Telemovel> telemoveis = new List<Telemovel>();
        public List<Reparacao> reparacoes = new List<Reparacao>();

        public delegate void dadosAtualizados();
		public event dadosAtualizados dadosForamAtualizados;
		public delegate void dadosOrcamento(ref int valor);
		public event dadosOrcamento enviarDadosOrcamento;
		public delegate void PdfGerado();
		public event PdfGerado pdfGerado;
        public delegate void enviarDadosTelemovel(ref List<Telemovel> t);
        public event enviarDadosTelemovel enviarListaTelemovel;
        public delegate void enviarDadosReparacao(ref List<Reparacao> r);
        public event enviarDadosReparacao enviarListaReparacao;

        public Model(View v) {

			/* Fica a saber quem é a view e inicializa as listas
			 * de Telemóveis e reparações */

			view = v;

			telemoveis.Add(new Telemovel() { marca = "Samsung", modelo = "Galaxy S23" });
            telemoveis.Add(new Telemovel() { marca = "Samsung", modelo = "Galaxy Z Fold4" });
            telemoveis.Add(new Telemovel() { marca = "Huawei", modelo = "Mate 50" });
            telemoveis.Add(new Telemovel() { marca = "Huawei", modelo = "P30" });
            telemoveis.Add(new Telemovel() { marca = "Apple", modelo = "Iphone XPTO" });
            telemoveis.Add(new Telemovel() { marca = "Apple", modelo = "Iphone XPTO mini" });

			reparacoes.Add(new Reparacao() { nome = "Troca de Ecra", valor = 120 });
            reparacoes.Add(new Reparacao() { nome = "Troca de modulo memoria", valor = 180 });
            reparacoes.Add(new Reparacao() { nome = "Troca de sistema operativo", valor = 60 });

			view.solicitarDadosReparacao += EnviarDadosReparacao;
            view.solicitarDadosTelemovel += EnviarDadosTelemovel;
        }

        public class Telemovel
        {
            public string marca { get; set; }
            public string modelo { get; set; }
        }

        public class Reparacao
        {
            public string nome { get; set; }
            public int valor;
        }

        public class Utilizador
        {
            public string nome { get; set; }
            public string morada { get; set; }
            public int nif;
        }

        public void UpdateInfoReparacao(ref string dados)
		{
			/* Fazer update aos dados da reparação e actualizar dadosReparacao
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
			/* Fazer update aos dados do utilizador e actualizar dadosUtilizador
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

        public void EnviarDadosReparacao()
        {
            /*Envia a lista de reparações possiveis para serem apresentadas ao
             * utilizador. */

            enviarListaReparacao(ref reparacoes);
        }

        public void EnviarDadosTelemovel()
        {
            /*Envia a lista de reparações possiveis para serem apresentadas ao
             * utilizador. */

            enviarListaTelemovel(ref telemoveis);
        }

        public void CriarPdf()
		{
			/* Código de implementação da API Pdf Sharp para criar o Pdf com as
			 * informações necessárias. */

			pdfGerado();
		}
    }
}

