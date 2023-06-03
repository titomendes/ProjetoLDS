using System;
using System.Collections.Generic;
using ExemploEventosDelegados.Models;
using ExemploEventosDelegados.Views;
using ExemploEventosDelegados.Exceptions;
using System.Linq.Expressions;
using ExemploEventosDelegados.Interfaces;

namespace ExemploEventosDelegados.Controllers
{
    public class TelefoneController
    {
        private List<IMarca> telefones;
        private TelefoneView view;
        private TelefoneModel model;
        private bool sair = false;

        private IModelo modelo;    // objeto modelo do tipo IModelo
        private Texto texto;
        private OrcamentoPdf orcamento;  //objeto orcamento para ligar o evento

        public delegate void Interface ();
        public event Interface AtivarInterface;


        public TelefoneController()
        {
            this.view =  new TelefoneView();
            this.model = new TelefoneModel();
            this.modelo = new TelefoneModelo();    //inicializar o modelo para aceder a função de selecionar a reparacao
            this.orcamento= new OrcamentoPdf();
        }

        public void Iniciar()
        {
            model.Iniciar();
     
            AtivarInterface += view.InicarInterface;
            
            view.PedirListaTelefones += model.EnviarLista;

            view.VerificaMarca += model.VerificaMarca;
            view.VerificaModelo += model.VerificaModelo;


            view.ReparacaoSelecionada += modelo.selecionaReparacao;   //evento de reparacao selecionada

            view.EquipamentoGerado += orcamento.GerarRelatorioPDF;  //evento de equipamento pronto para gerar o orçamento

          

            view.UtilizadorQuerSair += SairPrograma;

            do
            {
                try
                {
                    // Inicia a Interface
                    AtivarInterface();
                }
                catch (ExceptionInputInvalido)
                {
                    view.EcraErro();
                }

            } while (!sair);
          
        }

        public void SairPrograma()
        {
            sair = true;
        }
    }
}
