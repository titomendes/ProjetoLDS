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
            var modelo = new TelefoneModelo();     //criação do modelo para inicializar, o construtor do controller espera um IModelo        
            var controller = new TelefoneController(view, model, modelo);
            // Inicia o fluxo do programa
            controller.Iniciar();
            
        }
    }
}
