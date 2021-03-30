using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationTest
{
    class OtherTests
    {
        [TestMethod]
        public void TestBit_SkipClear_True()
        {
            int lit1 = 0;

            int result = com.OperationService.OperationHelpers.BitTest(lit1, 0);

            Assert.AreEqual(2, result);
        }
    }
}
