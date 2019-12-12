using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISOSA.SARH.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using ISOSA.SARH.Data.Domain.Assignation;

namespace ISOSA.SARH.Data.Repository.Tests
{
    [TestClass()]
    public class EmployeeObjectAsignationRepositoryTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            //Arrange
            EmployeeObjectAsignationRepository repo = 
                new EmployeeObjectAsignationRepository("Data Source=localhost,32771;Initial Catalog=ctINDUSTRIAS_SAND;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Testing!");

            //Act

            var model = new EmployeeObjectAsignation() 
            {

            };

            repo.Create(model);


            //Assert
            Assert.AreNotEqual(null, repo);
        }
    }
}