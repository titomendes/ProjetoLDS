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
  
            var controller = new TelefoneController();
            // Inicia o fluxo do programa
            controller.Iniciar();
            
        }
    }
}
