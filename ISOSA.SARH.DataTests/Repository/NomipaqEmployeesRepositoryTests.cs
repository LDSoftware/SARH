using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISOSA.SARH.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ISOSA.SARH.Data.Repository.Tests
{
    [TestClass()]
    public class NomipaqEmployeesRepositoryTests
    {
        [TestMethod()]
        public void GetAllTest()
        {
            //Arrange
            NomipaqEmployeesRepository repo = new NomipaqEmployeesRepository("Data Source=localhost,32771;Initial Catalog=ctINDUSTRIAS_SAND;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Testing!");

            //Act
            var result = repo.SearhItemsFor(o => o.codigoempleado.Equals("00002"));



            //Assert
            Assert.AreNotEqual(0, result.Count());
        }
    }
}