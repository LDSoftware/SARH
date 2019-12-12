using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISOSA.SARH.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ISOSA.SARH.Data.Repository.Tests
{
    [TestClass()]
    public class EmployeeOrganigramaRepositoryTests
    {
        [TestMethod()]
        public void EmployeeOrganigrama_GetAllTest()
        {

            //Arrange
            EmployeeOrganigramaRepository repo = new EmployeeOrganigramaRepository("Data Source=localhost,32771;Initial Catalog=ISOSA_Security;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Testing!");

            //Act
            var result = repo.GetAll();

            //Assert
            Assert.AreNotEqual(0, result.Count());
        }
    }
}