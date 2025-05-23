#region Copyright notice and license
// Protocol Buffers - Google's data interchange format
// Copyright 2015 Google Inc.  All rights reserved.
//
// Use of this source code is governed by a BSD-style
// license that can be found in the LICENSE file or at
// https://developers.google.com/open-source/licenses/bsd
#endregion

using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.TestProtos.Proto2;
using Proto2 = Google.Protobuf.TestProtos.Proto2;
using NUnit.Framework;
using UnitTest.Issues.TestProtos;
using static Google.Protobuf.TestProtos.Proto2.UnittestExtensions;

namespace Google.Protobuf
{
    /// <summary>
    /// Test for Issue #### - link
    /// GetHashCode Collision and uniform distribution Test 
    /// </summary>
    public partial class TestGetHashCodeCollisionsTest
    {
        [Test]
        public void GetHashCodeCollisionsBoolean()
        {
            

            var message1 = new AllFieldRepeatedClass();
            var message2 = new AllFieldRepeatedClass() { Boolean1 = true};
            var message3 = new AllFieldRepeatedClass() { Boolean1 = true, Boolean2 = true};
            var message4 = new AllFieldRepeatedClass() { Boolean1 = true, Boolean2 = true, Boolean3 = true};
            var message5 = new AllFieldRepeatedClass() { Boolean1 = true, Boolean2 = true, Boolean3 = true,Boolean4 = true};
            var message6 = new AllFieldRepeatedClass() { Boolean1 = true, Boolean2 = true, Boolean3 = true,Boolean4 = true, Boolean5 = true};
            var message7 = new AllFieldRepeatedClass() { Boolean1 = true, Boolean2 = true, Boolean3 = true,Boolean4 = true, Boolean5 = true, Boolean6 = true};

            List<int> hashList = new()
            {
                message1.GetHashCode(),
                message2.GetHashCode(),
                message3.GetHashCode(),
                message4.GetHashCode(),
                message5.GetHashCode(),
                message6.GetHashCode(),
                message7.GetHashCode(),
            };

            var groups = hashList.GroupBy(h => h).Count();

            Assert.AreEqual(7, groups);
        }

     
    }
}
