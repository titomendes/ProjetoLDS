using System;

namespace ProjectoSamuraiSquad
{
    public class Model
    {
        public event Action AtivarInterface;
        public event Action DadosForamAtualizados;
        public event Action EnviarDadosOrcamento;

        public void EnviarDadosTelemovel()
        {
            // Lógica para enviar dados de telemóvel
        }

        public void EnviarDadosReparacao()
        {
            // Lógica para enviar dados de reparação
        }

        public void UpdateInfoReparacao()
        {
            // Lógica para atualizar informações de reparação
        }

        public void CriarPdf()
        {
            // Lógica para criar arquivo PDF
        }
    }
}
