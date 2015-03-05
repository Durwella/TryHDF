using HDF5DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TryHDF
{
    [TestClass]
    public class Writing
    {
        [TestMethod]
        public void CanWriteDoubles()
        {
            var f = H5F.create("sample.h5", H5F.CreateMode.ACC_TRUNC);
            {
                var dimensions = new long[] {2, 3, 4};
                var s = H5S.create_simple(rank: dimensions.Length, dims: dimensions);
                {
                    var type = H5T.copy(H5T.H5Type.NATIVE_DOUBLE);
                    var d = H5D.create(f, "DS1", type, s);
                    {
                        var buffer = new double[2, 3, 4];
                        for (int i = 0; i < 2; i++)
                            for (int j = 0; j < 3; j++)
                                for (int k = 0; k < 4; k++)
                                    buffer[i, j, k] = Math.Pow(j + 2, k + 1) / (i + 1);
                        H5D.write<double>(d, type, new H5Array<double>(buffer));
                    }
                    H5D.close(d);
                }
                H5S.close(s);
            }
            H5F.close(f);
            Assert.IsTrue(File.Exists("sample.h5"));
        }
    }
}
