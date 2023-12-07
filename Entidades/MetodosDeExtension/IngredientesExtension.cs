using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoTotal = costoInicial;
            double aumentoPorcentual = 0;
            foreach (EIngrediente ing in ingredientes)
            {
                aumentoPorcentual += (int)ing;
            }
            costoTotal *= 1 + aumentoPorcentual/100;
            return costoTotal;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON,
            };

            int numeroDeIngredientes = rand.Next(1, ingredientes.Count + 1);
            return ingredientes.Take(numeroDeIngredientes).ToList();
        }
    }
}
