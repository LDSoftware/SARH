using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISOSA.SARH.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSA.SARH.Data.Repository.Tests
{
    [TestClass()]
    public class SafeEquipmentAssignedRepositoryTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            //Arrange
            SafeEquipmentAssignedRepository db = new SafeEquipmentAssignedRepository("Data Source=localhost,32768;Initial Catalog=ISOSA_SARH;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Testing!");

            db.Create(new Domain.Catalog.SafeEquipmentAssigned() { Description = "Casco de seguridad" });


            //Act
            var result = db.GetAll();


            //Assert
            Assert.AreNotEqual(null, result);
        }
    }
}