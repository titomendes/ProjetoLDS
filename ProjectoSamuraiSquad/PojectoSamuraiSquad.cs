using System;

namespace ProjectoSamuraiSquad
{
    public class Program
    {
        static void Main(string[] args)
        {
            Model model = new Model();
            View view = new View(model);
            Controller controller = new Controller(model, view);

            // Lógica do programa
        }
    }
}


// Teste de push