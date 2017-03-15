using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoboWar.BruttoRobot.Test
{
    [TestClass]
    public class BruttoRobotTest
    {
        [TestMethod]
        public void La_proprietà_Immagine_deve_restituire_l_immagine_corretta()
        {
            //Arrange
            var bruttoRobot = new BruttoRobot();

            //Act
            var immagine = bruttoRobot.Immagine;

            //Assert
            Assert.IsNotNull(immagine);
            Assert.AreNotEqual(113584, immagine.Length);
        }
    }
}
