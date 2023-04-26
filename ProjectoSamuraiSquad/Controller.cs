using System;

namespace ProjectoSamuraiSquad
{
    public class Controller
    {
        private Model model;
        private View view;

        public Controller(Model model, View view)
        {
            this.model = model;
            this.view = view;

            model.AtivarInterface += View_AtivarInterface;
            model.DadosForamAtualizados += View_DadosForamAtualizados;
            model.EnviarDadosOrcamento += View_EnviarDadosOrcamento;
        }

        private void View_AtivarInterface()
        {
            // Lógica para ativar a interface
        }

        private void View_DadosForamAtualizados()
        {
            // Lógica para tratar quando os dados foram atualizados
        }

        private void View_EnviarDadosOrcamento()
        {
            // Lógica para enviar dados de orçamento
        }
    }
}
