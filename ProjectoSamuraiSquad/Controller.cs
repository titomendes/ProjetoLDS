﻿using System;
using System.Collections.Generic;
using ExemploEventosDelegados.Models;
using ExemploEventosDelegados.Views;
using ExemploEventosDelegados.Exceptions;

namespace ExemploEventosDelegados.Controllers
{
    public class TelefoneController
    {
        private List<IMarca> telefones;
        private TelefoneView view;
        private TelefoneModel model;
        private bool sair = false;

        private IModelo modelo;    // objeto modelo do tipo IModelo
        private ITexto texto;

        public delegate void Interface ();
        public event Interface AtivarInterface;


        public TelefoneController(TelefoneView view, TelefoneModel model, IModelo modelo)
        {
            this.view = view;
            this.model = model;
            this.modelo = modelo;    //inicializar o modelo para aceder a função de selecionar a reparacao
        }

        public void Iniciar()
        {
            // Iniciar o view e o model para terem conhecimento um do outro
            view.Iniciar(model);
            model.Iniciar(view);
     
            AtivarInterface += view.InicarInterface;
            
            view.PedirListaTelefones += model.EnviarLista;

            view.VerificaMarca += model.VerificaMarca;
            view.VerificaModelo += model.VerificaModelo;


            view.ReparacaoSelecionada += modelo.selecionaReparacao;   //evento de reparacao selecionada

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
