using System;


namespace ProjectoSamuraiSquad
{
    public class View

    {
        private Model model;

        public View(Model model)
        {
            this.model = model;
        }

        public void IniciarListaTelemovel()
        {
            model.EnviarDadosTelemovel();
        }

        public void IniciarListaReparacao()
        {
            model.EnviarDadosReparacao();
        }

        public void UpdateInfoReparacao()
        {
            model.UpdateInfoReparacao();
        }

        public void CriarPdf()
        {
            model.CriarPdf();
        }
    }
}

