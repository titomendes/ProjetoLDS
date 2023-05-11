using System;
using System.Collections.Generic;
using System.Data;
using ExemploEventosDelegados.Controllers;
using ExemploEventosDelegados.Models;
using ExemploEventosDelegados.Views;

namespace ExemploEventosDelegados
{
    class Program
    {
        static void Main(string[] args)
        {
                // Criação da view e do controller
                var view = new TelefoneView();
                var model = new TelefoneModel();
                var controller = new TelefoneController( view, model);

                // Inicia o fluxo do programa
                controller.Iniciar();
            
        }
    }
}
