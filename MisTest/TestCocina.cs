using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            string data = "UnitTest";
            string rutaInvalida = "|||.csv.|";

            //act
            FileManager.Guardar(data, rutaInvalida, true);

            //assert
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Ramon");

            //act

            //assert
            Assert.AreEqual(0, cocinero.CantPedidosFinalizados);
        }
    }
}