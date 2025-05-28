#region Copyright notice and license
// Protocol Buffers - Google's data interchange format
// Copyright 2015 Google Inc.  All rights reserved.
//
// Use of this source code is governed by a BSD-style
// license that can be found in the LICENSE file or at
// https://developers.google.com/open-source/licenses/bsd
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Google.Protobuf.TestProtos.Proto2;
using NUnit.Framework;
using UnitTest.Issues.TestProtos;
using static Google.Protobuf.TestProtos.Proto2.UnittestExtensions;
using Proto2 = Google.Protobuf.TestProtos.Proto2;

namespace Google.Protobuf
{
    /// <summary>
    /// Test for Issue #### - link
    /// GetHashCode Collision and uniform distribution Test 
    /// </summary>
    public partial class TestGetHashCodeCollisionsTest
    {

        [Test]
        public void GetHashCodeCollisions_Boolean_SingleField()
        {

            var message1 = new AllFieldRepeatedClass() { Boolean1 = false };
            var message2 = new AllFieldRepeatedClass() { Boolean1 = true };

            List<int> hashList = new()
            {
                message1.GetHashCode(),
                message2.GetHashCode(),
            };

            var groups = hashList.GroupBy(h => h).Count();

            Assert.AreEqual(2, groups);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetHashCodeCollisions_Boolean_MultipleFields(bool value)
        {
            

            var message1 = new AllFieldRepeatedClass();
            var message2 = new AllFieldRepeatedClass() { Boolean1 = value};
            var message3 = new AllFieldRepeatedClass() { Boolean1 = value, Boolean2 = value};
            var message4 = new AllFieldRepeatedClass() { Boolean1 = value, Boolean2 = value, Boolean3 = value};
            var message5 = new AllFieldRepeatedClass() { Boolean1 = value, Boolean2 = value, Boolean3 = value,Boolean4 = value};
            var message6 = new AllFieldRepeatedClass() { Boolean1 = value, Boolean2 = value, Boolean3 = value,Boolean4 = value, Boolean5 = value};
            var message7 = new AllFieldRepeatedClass() { Boolean1 = value, Boolean2 = value, Boolean3 = value,Boolean4 = value, Boolean5 = value, Boolean6 = value};

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

        [Test]
        public void GetHashCodeCollisions_Boolean_MultipleFieldsRandom()
        {

            Stack<bool> st = new Stack<bool>(); //make this a fixed list of bools
            Random rd = new Random(); 
            for (int i = 0; i < 21; i++)
            {
                st.Push(rd.Next(0, 2) == 0);
            }

            var message1 = new AllFieldRepeatedClass();
            var message2 = new AllFieldRepeatedClass() { Boolean1 = st.Pop() };
            var message3 = new AllFieldRepeatedClass() { Boolean1 = st.Pop(), Boolean2 = st.Pop()};
            var message4 = new AllFieldRepeatedClass() { Boolean1 = st.Pop(), Boolean2 = st.Pop(), Boolean3 = st.Pop()};
            var message5 = new AllFieldRepeatedClass() { Boolean1 = st.Pop(), Boolean2 = st.Pop(), Boolean3 = st.Pop(),Boolean4 = st.Pop()};
            var message6 = new AllFieldRepeatedClass() { Boolean1 = st.Pop(), Boolean2 = st.Pop(), Boolean3 = st.Pop(),Boolean4 = st.Pop(), Boolean5 = st.Pop()};
            var message7 = new AllFieldRepeatedClass() { Boolean1 = st.Pop(), Boolean2 = st.Pop(), Boolean3 = st.Pop(),Boolean4 = st.Pop(), Boolean5 = st.Pop(), Boolean6 = st.Pop()};

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

        [Test]
        public void GetHashCodeCollisions_Int_SingleFieldRandom()
        {
            int n = 1 << 20;// 1M
            var random = new Random(); //poc -> replace with fix values
            
            var hashList = new int[n];
            var List = new int[n];
            var obj = new AllFieldRepeatedClass();

            for (int i = 0; i < n; i++)
            {
                obj.Int1 = random.Next(-n / 2, n / 2);
                List[i] = obj.Int1;
                hashList[i] = obj.GetHashCode();
            }

            //var hashList = list.Select(o => o.GetHashCode());

            List<int> distributionHash = hashList.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionRandom = List.GroupBy(h => h).Select(g => g.Count()).ToList();

            double avgHash = distributionHash.Average();
            double avgRandom = distributionRandom.Average();

            double standardDeviationHash = Math.Sqrt(distributionHash.Average(v => Math.Pow(v - avgHash, 2)));
            double standardDeviationList = Math.Sqrt(distributionRandom.Average(v => Math.Pow(v - avgRandom, 2)));

            Assert.Less(standardDeviationHash - standardDeviationList, 0.01d); //find out a good margin and explanation of it

        }

        [Test]
        public void GetHashCodeCollisions_Int_MultiplesFieldsSame_individualDistributionsAcumulatedErr()
        {
            int n = 1 << 20;// 1M
            var random = new Random(); //poc -> replace with fix values

            var hashList2 = new int[n];
            var hashList3 = new int[n];
            var hashList4 = new int[n];
            var hashList5 = new int[n];
            var hashList6 = new int[n];
            var hashList7 = new int[n];
            var List = new int[n];

            var message1 = new AllFieldRepeatedClass();
            var message2 = new AllFieldRepeatedClass();
            var message3 = new AllFieldRepeatedClass();
            var message4 = new AllFieldRepeatedClass();
            var message5 = new AllFieldRepeatedClass();
            var message6 = new AllFieldRepeatedClass();
            var message7 = new AllFieldRepeatedClass();
            int next = 0;

            for (int i = 0; i < n; i++)
            {
                next = random.Next(-n / 2, n / 2);
                message2.Int1 = next;
                message3.Int1 = next; message3.Int2 = next;
                message4.Int1 = next; message4.Int2 = next; message4.Int3 = next;
                message5.Int1 = next; message5.Int2 = next; message5.Int3 = next; message5.Int4 = next;
                message6.Int1 = next; message6.Int2 = next; message6.Int3 = next; message6.Int4 = next; message6.Int5 = next;
                message7.Int1 = next; message7.Int2 = next; message7.Int3 = next; message7.Int4 = next; message7.Int5 = next; message7.Int6 = next;
                
                List[i] = next;
                hashList2[i] = message2.GetHashCode();
                hashList3[i] = message3.GetHashCode();
                hashList4[i] = message4.GetHashCode();
                hashList5[i] = message5.GetHashCode();
                hashList6[i] = message6.GetHashCode();
                hashList7[i] = message7.GetHashCode();
            }

            List<int> fullList = List.Concat(List).Concat(List).Concat(List).Concat(List).Concat(List).ToList();
            List<int> fullHashList = hashList2.Concat(hashList3).Concat(hashList4).Concat(hashList5).Concat(hashList6).Concat(hashList7).ToList();

            List<int> distributionHash2 = hashList2.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionHash3 = hashList3.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionHash4 = hashList4.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionHash5 = hashList5.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionHash6 = hashList6.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionHash7 = hashList7.GroupBy(h => h).Select(g => g.Count()).ToList();

            List<int> distributionFullHash = fullHashList.GroupBy(h => h).Select(g => g.Count()).ToList();


            List<int> distributionRandom = List.GroupBy(h => h).Select(g => g.Count()).ToList();
            List<int> distributionFullListRandom = fullList.GroupBy(h => h).Select(g => g.Count()).ToList();


            double avgHash2 = distributionHash2.Average();
            double avgHash3 = distributionHash3.Average();
            double avgHash4 = distributionHash4.Average();
            double avgHash5 = distributionHash5.Average();
            double avgHash6 = distributionHash6.Average();
            double avgHash7 = distributionHash7.Average();

            double avgfullhash = distributionFullHash.Average();

            double avgRandom = distributionRandom.Average();
            double avgFullListRandom = distributionFullListRandom.Average();


            double standardDeviationHash2 = Math.Sqrt(distributionHash2.Average(v => Math.Pow(v - avgHash2, 2)));
            double standardDeviationHash3 = Math.Sqrt(distributionHash3.Average(v => Math.Pow(v - avgHash3, 2)));
            double standardDeviationHash4 = Math.Sqrt(distributionHash4.Average(v => Math.Pow(v - avgHash4, 2)));
            double standardDeviationHash5 = Math.Sqrt(distributionHash5.Average(v => Math.Pow(v - avgHash5, 2)));
            double standardDeviationHash6 = Math.Sqrt(distributionHash6.Average(v => Math.Pow(v - avgHash6, 2)));
            double standardDeviationHash7 = Math.Sqrt(distributionHash7.Average(v => Math.Pow(v - avgHash7, 2)));

            double standardDeviationFullhash = Math.Sqrt(distributionFullHash.Average(v => Math.Pow(v - avgfullhash, 2)));

            double standardDeviationList = Math.Sqrt(distributionRandom.Average(v => Math.Pow(v - avgRandom, 2)));
            double standardDeviationFullList = Math.Sqrt(distributionFullListRandom.Average(v => Math.Pow(v - avgFullListRandom, 2)));


            double standarDeviationError = 0.0d;

            standarDeviationError += Math.Abs(standardDeviationHash2 - standardDeviationList);
            standarDeviationError += Math.Abs(standardDeviationHash3 - standardDeviationList);
            standarDeviationError += Math.Abs(standardDeviationHash4 - standardDeviationList);
            standarDeviationError += Math.Abs(standardDeviationHash5 - standardDeviationList);
            standarDeviationError += Math.Abs(standardDeviationHash6 - standardDeviationList);
            standarDeviationError += Math.Abs(standardDeviationHash7 - standardDeviationList);

            double standarDeviationFullList = Math.Abs(standardDeviationFullhash - standardDeviationFullList);


            Assert.Less(standarDeviationError, 0.01d); //find out a good margin and explanation of it
            Assert.Less(standarDeviationFullList, 0.01d); //find out a good margin and explanation of it




            //split fullDistribution into his own test for example GetHashCodeCollisions_Int_MultiplesFieldsSame_concatDistributions
        }



    }
}
