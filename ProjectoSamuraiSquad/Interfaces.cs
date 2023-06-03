using ExemploEventosDelegados.Models;

namespace ExemploEventosDelegados.Interfaces
{
    public interface IModeloPdf  //interface do modelo para enviar parao pdf, só é preciso o nome
    {
        string nome { get; set; }
    }

    public interface IModelo   //cada modelo tem de ter nome e uma lista de reparações
    {                           //com a interface pode se criar outros tipos de modelo, desde que implementem este contrato
                                //coloquei a função de adiconarTipoDeReparação no contrato porque se não para a usar tinha de fazer um cast para cada modelo para poder usar a função
        string nome { get; set; }
        Dictionary<string, decimal> PrecosDeReparacao { get; set; }

        void AdicionarTipoDeReparacao(string tipoReparacao, decimal preco);

        public Reparacao selecionaReparacao(int n, Dictionary<string, decimal> listaReparacoes);

    }
    public interface IMarca
    {
        string nome { get; set; }
        List<IModelo> Modelos { get; set; }
    }

    public interface IMarcaPdf
    {
        string nome { get; set; }
    }
}