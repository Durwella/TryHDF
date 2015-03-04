using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF5DotNet;

namespace TryHDF
{
    [TestClass]
    public class Reading
    {
        /*
         * > h5dump h5ex_d_sofloat.h5
            HDF5 "C:\...\h5ex_d_sofloat.h5" {
            GROUP "/" {
               DATASET "DS1" {
                  DATATYPE  H5T_IEEE_F64LE
                  DATASPACE  SIMPLE { ( 32, 64 ) / ( 32, 64 ) }
                  DATA {
                  (0,0): 3.32923, 1.76923, 2.43923, 3.29923, 4.22923, 5.18923, 6.15923,
                  ...
                  (3,56): 56.0678, 57.0678, 58.0678, 59.0678, 60.0678, 61.0678, 62.0678,
         */
        [TestMethod]
        public void CanOpenFile()
        {
            var f = H5F.open(
                "h5ex_d_sofloat.h5",
                H5F.OpenMode.ACC_RDONLY);
            {
                var d = H5D.open(f, "DS1");
                {
                    var s = H5D.getSpace(d);
                    var dimensions = H5S.getSimpleExtentDims(s);
                    CollectionAssert.AreEqual(new long[] { 32, 64 }, dimensions);

                    var type = H5D.getType(d);
                    var doubleType = H5T.getNativeType(H5T.H5Type.NATIVE_DOUBLE, H5T.Direction.DEFAULT);
                    Assert.IsTrue(H5T.equal(doubleType, type));

                    var buffer = new double[32, 64];
                    H5D.read<double>(d, type, new H5Array<double>(buffer));
                    Assert.AreEqual(3.32923, buffer[0, 0], 0.00001);
                    Assert.AreEqual(56.0678, buffer[3, 56], 0.0001);

                    H5S.close(s);
                }
                H5D.close(d);
            }
            H5F.close(f);
        }
    }
}
