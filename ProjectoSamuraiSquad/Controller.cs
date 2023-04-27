using System;


namespace ProjectoSamuraiSquad
{
    public class Controller
    {
        private Model model;
        private View view;


        public delegate void AtivacaoInterface();
        public event AtivacaoInterface AtivarInterface;

        //Eventos e delegados para comunicação

        public void IniciarPrograma()
        {
            view = new View(model);
            model = new Model(view);


            AtivarInterface += view.AtivarInterface;

            view.enviarDadosReparacao += model.UpdateInfoReparacao;
            view.enviarDadosUtilizador += model.updateInfoUtilizador;

            view.solicitarOrcamento += model.EnviarDadosOrcamento;

            model.enviarDadosOrcamento += view.ApresentarOrcamento;

            view.pedirPdf += model.CriarPdf;

            //view.ordemEncerrar += Encerrar();
            //model.pdfGerado += Encerrar();
            AtivarInterface();
        }


        public void Encerrar()
        {
            view.MostrarMSGFinal();
            Environment.Exit(0);
        }
    }
}